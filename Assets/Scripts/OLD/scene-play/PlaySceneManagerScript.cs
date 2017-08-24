using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlaySceneManagerScript : MonoBehaviour
{
    public Button logoutButton;
    
	void Start ()
    {
        logoutButton.onClick.AddListener(OnLogoutButtonClicked);
    }

    private void OnLogoutButtonClicked()
    {
        SmartFoxInstanceManager.getInstance().disconnect();

        SceneManager.LoadScene(SloverseSceneUtil.START_SCENE);
    }
}
