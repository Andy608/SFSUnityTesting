using UnityEngine;
using Sfs2X;

public class SFSConnection : MonoBehaviour 
{
	private static SFSConnection mInstance;
	private static SmartFox sfs;

	public static SmartFox connection
	{
		get
		{
			if (mInstance == null)
			{
				mInstance = new GameObject("SFSConnection").AddComponent(typeof(SFSConnection)) as SFSConnection;
			}

			return sfs;
		}

		set
		{
			if (mInstance == null)
			{
				mInstance = new GameObject("SFSConnection").AddComponent(typeof(SFSConnection)) as SFSConnection;
			}

			sfs = value;
		}
	}

	public static bool isInitialized
	{
		get 
		{
			return (sfs != null);
		}
	}

	void OnApplicationQuit()
	{
		Debug.Log("Ending after " + Time.time + " seconds.");
		if (sfs.IsConnected)
		{
			sfs.Disconnect();
		}
	}
}
