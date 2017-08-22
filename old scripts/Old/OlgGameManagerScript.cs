using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Sfs2X;
using Sfs2X.Core;
using Sfs2X.Entities;
using Sfs2X.Entities.Data;
using Sfs2X.Entities.Variables;
using Sfs2X.Requests;

public class GameManagerScript : MonoBehaviour 
{
	private SmartFox sfs;

	public GameObject playerPrefab;

	private GameObject localPlayer;
	private PlayerControllerScript localPlayerController;
	private Dictionary<SFSUser, GameObject> remotePlayers = new Dictionary<SFSUser, GameObject>();
	private List<UserVariable> movementVariables = new List<UserVariable>();

	void Start () 
	{
		if (!SFSConnection.isInitialized)
		{
			SceneManager.LoadScene("Login");
			return;
		}

		sfs = SFSConnection.connection;

		sfs.AddEventListener(SFSEvent.OBJECT_MESSAGE, OnObjectMessage);
		sfs.AddEventListener(SFSEvent.CONNECTION_LOST, OnConnectionLost);
		sfs.AddEventListener(SFSEvent.USER_VARIABLES_UPDATE, OnUserVariableUpdate);
		sfs.AddEventListener(SFSEvent.USER_ENTER_ROOM, OnUserEnterRoom);
		sfs.AddEventListener(SFSEvent.USER_EXIT_ROOM, OnUserExitRoom);

		SpawnLocalPlayer(GetRandomColor(), new Vector3());
	}

	private void OnObjectMessage(BaseEvent baseEvent)
	{
		ISFSObject dataObj = (SFSObject)baseEvent.Params["message"];
		SFSUser sender = (SFSUser)baseEvent.Params["sender"];

		if (dataObj.ContainsKey("cmd"))
		{
			switch (dataObj.GetUtfString("cmd"))
			{
			case "rm":
				Debug.Log("Removing player: " + sender.Id);
				RemoveRemotePlayer(sender);
				break;
			}
		}
	}

	private void OnConnectionLost(BaseEvent baseEvent)
	{
		sfs.RemoveAllEventListeners();
		SceneManager.LoadScene("Login");
	}

	private void OnUserVariableUpdate(BaseEvent baseEvent)
	{
		ArrayList changedVars = (ArrayList)baseEvent.Params["changedVars"];

		SFSUser user = (SFSUser)baseEvent.Params["user"];

		if (user == sfs.MySelf) return;

		Debug.Log("USER: " + user);

		if (!remotePlayers.ContainsKey(user))
		{
			Color color = new Color(1.0f, 1.0f, 1.0f, 1.0f);

			if (user.ContainsVariable("colorR") && user.ContainsVariable("colorG") && user.ContainsVariable("colorB"))
			{
				color.r = (float)user.GetVariable("colorR").GetDoubleValue();
				color.g = (float)user.GetVariable("colorG").GetDoubleValue();
				color.b = (float)user.GetVariable("colorB").GetDoubleValue();
			}

			Vector3 pos = new Vector3(0, 0, 0);

			if (user.ContainsVariable("x") && user.ContainsVariable("y") && user.ContainsVariable("z"))
			{
				pos.x = (float)user.GetVariable("x").GetDoubleValue();
				pos.y = (float)user.GetVariable("y").GetDoubleValue();
				pos.z = (float)user.GetVariable("z").GetDoubleValue();
			}

			SpawnRemotePlayer(user, color, pos);
		}

		if (changedVars.Contains("x") && changedVars.Contains("y") && changedVars.Contains("z"))
		{
			remotePlayers[user].GetComponent<RemoteInterpolationScript>().SetTransform(
				new Vector3(
					(float)user.GetVariable("x").GetDoubleValue(),
					(float)user.GetVariable("y").GetDoubleValue(),
					(float)user.GetVariable("z").GetDoubleValue()
				), true);
		}
	}

	private void OnUserEnterRoom(BaseEvent baseEvent)
	{
		if (localPlayer != null)
		{
			List<UserVariable> playerInfo = new List<UserVariable>();
			playerInfo.Add(new SFSUserVariable("x", (double)localPlayer.transform.position.x));
			playerInfo.Add(new SFSUserVariable("y", (double)localPlayer.transform.position.y));
			playerInfo.Add(new SFSUserVariable("z", (double)localPlayer.transform.position.z));

			Color color = localPlayer.GetComponent<SpriteRenderer>().color;
			playerInfo.Add(new SFSUserVariable("colorR", (double)color.r));
			playerInfo.Add(new SFSUserVariable("colorG", (double)color.g));
			playerInfo.Add(new SFSUserVariable("colorB", (double)color.b));
			sfs.Send(new SetUserVariablesRequest(playerInfo));
		}
	}

	private void OnUserExitRoom(BaseEvent baseEvent)
	{
		SFSUser user = (SFSUser)baseEvent.Params["user"];
		RemoveRemotePlayer(user);
	}

	private void SpawnLocalPlayer(Color color, Vector3 position)
	{
		if (localPlayer != null)
		{
			Destroy(localPlayer);
		}
		else
		{
			localPlayer = GameObject.Instantiate(playerPrefab, position, Quaternion.identity);
			localPlayer.GetComponent<SpriteRenderer>().color = color;
			localPlayer.AddComponent<PlayerControllerScript>();
			localPlayerController = localPlayer.GetComponent<PlayerControllerScript>();
			localPlayer.GetComponentInChildren<TextMesh>().text = sfs.MySelf.Name;

			List<UserVariable> colorVariable = new List<UserVariable>();
			colorVariable.Add(new SFSUserVariable("colorR", (double)color.r));
			colorVariable.Add(new SFSUserVariable("colorG", (double)color.g));
			colorVariable.Add(new SFSUserVariable("colorB", (double)color.b));
			sfs.Send(new SetUserVariablesRequest(colorVariable));
		}
	}

	private void SpawnRemotePlayer(SFSUser user, Color color, Vector3 position)
	{
		if (remotePlayers.ContainsKey(user) && remotePlayers[user] != null)
		{
			Destroy(remotePlayers[user]);
			remotePlayers.Remove(user);
		}

		GameObject remotePlayer = GameObject.Instantiate(playerPrefab, position, Quaternion.identity) as GameObject;
		remotePlayer.GetComponent<SpriteRenderer>().color = color;
		remotePlayer.AddComponent<RemoteInterpolationScript>();
		remotePlayer.GetComponent<RemoteInterpolationScript>().SetTransform(position, true);
		remotePlayer.GetComponentInChildren<TextMesh>().text = user.Name;

		remotePlayers.Add(user, remotePlayer);
	}

	public void Disconnect()
	{
		sfs.Disconnect();
	}

	void FixedUpdate()
	{
		if (sfs != null)
		{
			sfs.ProcessEvents();

			if (localPlayer != null && localPlayerController != null && localPlayerController.isMoving)
			{
				movementVariables.Clear();
				movementVariables.Add(new SFSUserVariable("x", (double)localPlayer.transform.position.x));
				movementVariables.Add(new SFSUserVariable("y", (double)localPlayer.transform.position.y));
				movementVariables.Add(new SFSUserVariable("z", (double)localPlayer.transform.position.z));
				sfs.Send(new SetUserVariablesRequest(movementVariables));
				localPlayerController.isMoving = false;
			}
		}
	}

	void OnApplicationQuit()
	{
		RemoveLocalPlayer();
	}

	private void RemoveLocalPlayer()
	{
		SFSObject obj = new SFSObject();
		obj.PutUtfString("cmd", "rm");
		sfs.Send(new ObjectMessageRequest(obj, sfs.LastJoinedRoom));
	}

	private void RemoveRemotePlayer(SFSUser user)
	{
		if (user == sfs.MySelf) return;

		if (remotePlayers.ContainsKey(user))
		{
			Destroy(remotePlayers[user]);
			remotePlayers.Remove(user);
		}
	}

	private Color GetRandomColor()
	{
		return new Color(Random.Range(100, 256) / 255.0f, Random.Range(100, 256) / 255.0f, Random.Range(100, 256) / 255.0f, 1.0f);
	}
}
