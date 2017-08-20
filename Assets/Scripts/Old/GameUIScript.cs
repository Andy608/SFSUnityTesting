using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Sfs2X;

public class GameUIScript : MonoBehaviour 
{
	public Button logoutButton;

	private SmartFox sfs;

	void Start () 
	{
		if (!SFSConnection.isInitialized)
		{
			SceneManager.LoadScene("Login");
			return;
		}

		sfs = SFSConnection.connection;

		logoutButton.onClick.AddListener(OnLogoutButtonClick);
	}

	private void OnLogoutButtonClick()
	{
		sfs.RemoveAllEventListeners();
		sfs.Disconnect();
		SceneManager.LoadScene("Login");
	}
}
