using UnityEngine;

public class FocusChecker : MonoBehaviour
{
    void Start()
    {
        Application.focusChanged += OnApplicationFocusEvent;
    }

    private void OnApplicationFocusEvent(bool focused)
    {
        if (!focused)
            ServiceLocator.GetService<SaveLoadService>().FocusLostCall();
    }
}
