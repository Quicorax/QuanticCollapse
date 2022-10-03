using UnityEngine;

public class FocusChecker : MonoBehaviour
{
    void OnApplicationFocus(bool hasFocus)
    {
        if (!hasFocus)
            ServiceLocator.GetService<SaveLoadService>().FocusLostCall();
    }
}
