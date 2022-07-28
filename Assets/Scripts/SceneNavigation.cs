using UnityEngine.SceneManagement;

public class SceneNavigation
{
    public void NavitageTo(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}
