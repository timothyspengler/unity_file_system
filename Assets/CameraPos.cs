using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraPos : MonoBehaviour
{
    // Start is called before the first frame update
    public void ResetMainCamera() {
        GameObject getCamera = GameObject.Find("Main Camera");
        Vector3 cameraPosition = GameObject.Find("Cache").GetComponent<Cache>().GetCameraObject();

        getCamera.transform.rotation = Quaternion.Euler(15, 0, 0);
        getCamera.transform.position = Vector3.Lerp(getCamera.transform.position, cameraPosition, 1.0f);
    }
}
