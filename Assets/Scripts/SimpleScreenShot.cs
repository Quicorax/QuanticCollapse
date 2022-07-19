using UnityEngine;

public class SimpleScreenShot : MonoBehaviour
{
    int screenShootCout;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F11))
            ResolutionScreenShoot();
    }

    void ResolutionScreenShoot()
    {
        screenShootCout++;
        string photoName = screenShootCout.ToString() + "_ScreenShot.png";

        ScreenCapture.CaptureScreenshot("C:/Users/Quim/Desktop/" + photoName, 2);
    }
}
