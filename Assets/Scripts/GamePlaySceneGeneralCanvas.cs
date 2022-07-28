using UnityEngine;

public class GamePlaySceneGeneralCanvas : MonoBehaviour
{
    SceneNavigation navigationLogic;

    public void RetreatFromMission()
    {
        if (navigationLogic == null)
            navigationLogic = new SceneNavigation();

        navigationLogic.NavitageTo("Initial_Scene");
    }
}
