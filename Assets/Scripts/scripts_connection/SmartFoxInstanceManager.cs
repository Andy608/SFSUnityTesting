using UnityEngine;
using Sfs2X;
using Sfs2X.Util;

public class SmartFoxInstanceManager : MonoBehaviour
{
    private static SmartFoxInstanceManager instance;
    private SmartFox sfs;
    
    public bool isConnected;

    protected void Start()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    protected void Update()
    {
        if (sfs != null)
        {
            isConnected = sfs.IsConnected;
        }
        else
        {
            isConnected = false;
        }
    }

    public static SmartFoxInstanceManager getInstance()
    {
        if (instance == null)
        {
            initInstance();
        }

        return instance;
    }

    private static void initInstance()
    {
        instance = new GameObject("SmartfoxConnection").AddComponent(typeof(SmartFoxInstanceManager)) as SmartFoxInstanceManager;
    }

    public static bool isInitialized()
    {
        return (instance != null);
    }

    public bool isSmartFoxInitialized()
    {
        return (sfs != null);
    }

    public SmartFox getSmartFox()
    {
        return sfs;
    }

    public void resetEventListeners()
    {
        if (isSmartFoxInitialized())
        {
            sfs.RemoveAllEventListeners();
        }
    }

    public ConfigData generateConfigData()
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

    public void disconnect()
    {
        getInstance().resetEventListeners();

        if (isSmartFoxInitialized() && sfs.IsConnected)
        {
            sfs.Disconnect();
        }

        sfs = null;
    }

    private void OnApplicationQuit()
    {
        Debug.Log("Ending after " + Time.time + " seconds.");
        resetEventListeners();
        disconnect();
    }
}
