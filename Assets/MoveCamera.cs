using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCamera : MonoBehaviour
{
    // how fast you move
    private int movementspeed;
    GameObject mainCamera;

    void Start() 
    {
        movementspeed = 25;
        mainCamera = GameObject.Find("Main Camera");
    }

    void Update() 
    {
        // move left
        if (Input.GetKey(KeyCode.LeftArrow))
            transform.Translate(Vector3.left * movementspeed * Time.deltaTime);

        // move right
        if (Input.GetKey(KeyCode.RightArrow))
            transform.Translate(Vector3.right * movementspeed * Time.deltaTime);

        // move forward
        if (Input.GetKey(KeyCode.W))
            transform.Translate(Vector3.forward * movementspeed * Time.deltaTime);

        // move backwards
        if (Input.GetKey(KeyCode.S))
            transform.Translate(Vector3.back * movementspeed * Time.deltaTime);

        // move up
        if (Input.GetKey(KeyCode.UpArrow))
            transform.Translate(Vector3.up * movementspeed * Time.deltaTime);

        // move down
        if (Input.GetKey(KeyCode.DownArrow))
            transform.Translate(Vector3.down * movementspeed * Time.deltaTime);

        // rotate right
        if (Input.GetKey(KeyCode.E))
            transform.RotateAround(transform.position, Vector3.up, Time.deltaTime * 90f);

        //rotate left
        if (Input.GetKey(KeyCode.Q))
            transform.RotateAround(transform.position, Vector3.up, Time.deltaTime * -90f);

        // rotate downward
        if (Input.GetKey(KeyCode.R))
            transform.RotateAround(transform.position, Vector3.left, Time.deltaTime * 90f);

        // rotate upward
        if (Input.GetKey(KeyCode.D))
            transform.RotateAround(transform.position, Vector3.left, Time.deltaTime * -90f);
    }

    public void CallLerp(Vector3 endPosition)
    {
        Vector3 cameraPosition = mainCamera.transform.position;
        //GameObject.Find("Cache").GetComponent<Cache>().GetCameraObject();
        Debug.Log(cameraPosition);
        Debug.Log(endPosition);
        //Vector3 pos = cache.GetComponent<Cache>().GetCameraObject();

        mainCamera.transform.position = Vector3.MoveTowards(cameraPosition, endPosition, Time.deltaTime * .001f);
    }
}
