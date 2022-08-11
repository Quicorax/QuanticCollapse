using UnityEngine;

public class GamePlaySceneGeneralCanvas : MonoBehaviour
{
    MasterSceneManager masterSceneManager;

    public void RetreatFromMission()
    {
        if (masterSceneManager == null)
            masterSceneManager = FindObjectOfType<MasterSceneManager>();

        masterSceneManager.NavigateToInitialScene();
    }
    public void ReplayMission()
    {
        if (masterSceneManager == null)
            masterSceneManager = FindObjectOfType<MasterSceneManager>();

        masterSceneManager.NavigateToGamePlayScene();
    }
}
