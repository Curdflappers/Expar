using UnityEngine;
using System.Collections;

public class OnCameraExit : MonoBehaviour
{
    /// <summary>
    /// Remove all items outside the camera. Because the camera is stationary, this is okay.
    /// </summary>
    /// <param name="other"></param>
    void OnTriggerExit2D(Collider2D other)
    {
        // do nothing
    }
}
