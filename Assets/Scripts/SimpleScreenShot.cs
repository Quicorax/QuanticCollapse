using UnityEngine;

public class SimpleScreenShot : MonoBehaviour
{
    private string Platform = "Generic";
    private int Index = 0;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F11))
            ResolutionScreenShoot();
    }

    void ResolutionScreenShoot()
    {
        Debug.Log("ScreenShot!");
        ScreenCapture.CaptureScreenshot("QuanticCollapse_"+ Platform + "_Screenshot_"+ Index + ".png", 8);
        Index++;
    }
}
