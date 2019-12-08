using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cache : DataNode
{
    Vector3 CameraPosition;

    // Start is called before the first frame update
    new void Start()
    {
    }

    public void SetCameraPosition(Vector3 position) 
    {
        CameraPosition = position;
    }

    public Vector3 GetCameraObject() 
    {
        return CameraPosition;
    }

}
