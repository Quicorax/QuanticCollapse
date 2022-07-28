using UnityEngine;

public class InitialSceneGeneralCanvas : MonoBehaviour
{
    SceneNavigation navigationLogic;

    public void EngageOnMission()
    {
        if(navigationLogic == null)
            navigationLogic = new SceneNavigation();

        navigationLogic.NavitageTo("GamePlay_Scene");
    }


}
