using UnityEngine;
using Sfs2X;
using Sfs2X.Util;

public class SmartFoxInstanceManager : MonoBehaviour
{
    private static SmartFoxInstanceManager instance;
    private static SmartFox sfs;

    public static SmartFox connection
    {
        get
        {
            if (instance == null)
            {
                initInstance();
            }

            return sfs;
        }

        set
        {
            if (instance == null)
            {
                initInstance();
            }

            sfs = value;
        }
    }

    private static void initInstance()
    {
        instance = new GameObject("SmartfoxConnection").AddComponent(typeof(SmartFoxInstanceManager)) as SmartFoxInstanceManager;
    }

    public static bool isInitialized()
    {
        return (sfs != null);
    }

    public static void resetEventListeners()
    {
        if (isInitialized())
        {
            sfs.RemoveAllEventListeners();
        }
    }

    public static ConfigData generateConfigData()
    {
        ConfigData config = new ConfigData();

        config.Host = SloverseConnectionUtil.HOST_IP;

#if UNITY_WEBGL
        config.Port = SloverseConnectionUtil.WEBSOCKET_PORT;
        config.Zone = SloverseConnectionUtil.ZONE;
        sfs = new SmartFox(UseWebSocket.WS_BIN);
#else
        config.Port = SloverseConnectionUtil.TCP_PORT;
        config.Zone = SloverseConnectionUtil.ZONE;
        sfs = new SmartFox();
#endif

        sfs.ThreadSafeMode = true;

        return config;
    }

    public static void disconnect()
    {
        if (isInitialized() && sfs.IsConnected)
        {
            sfs.Disconnect();
            sfs = null;
        }
    }

    private void OnApplicationQuit()
    {
        Debug.Log("Ending after " + Time.time + " seconds.");
        resetEventListeners();
        disconnect();
    }
}
