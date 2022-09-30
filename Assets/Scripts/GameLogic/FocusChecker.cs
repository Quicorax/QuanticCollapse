using UnityEngine;

public class FocusChecker : MonoBehaviour
{
    void OnApplicationFocus(bool hasFocus)
    {
        Debug.Log(hasFocus);

        if (hasFocus)
            ServiceLocator.GetService<SaveLoadService>().FocusLostCall();
    }
}
