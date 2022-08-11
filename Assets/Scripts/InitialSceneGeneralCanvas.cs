using UnityEngine;

public class InitialSceneGeneralCanvas : MonoBehaviour
{
    MasterSceneManager masterSceneManager;
    public void EngageOnMission()
    {
        if (masterSceneManager == null)
            masterSceneManager = FindObjectOfType<MasterSceneManager>();

        masterSceneManager.NavigateToGamePlayScene();
    }


}
