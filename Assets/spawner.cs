/* 
    Spawner spawns all objects of the file System which includes files, folders,
    and drives. Start() begins the file system at Drives, and the user can iteriate
    through directies displaying all files and folders that are displayed.

    whatToSpawnPrefab[2] = Drives Prefab(capsule) 
    whatToSpawnPrefab[1] = Folders Prefab(cubes) 
    whatToSPawnPrefab[0] = Files Prefab(spheres)
 */

using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System;
using UnityEngine;
using TMPro;
using System.Security.AccessControl;


public class spawner : MonoBehaviour
{
    public GameObject[] whatToSpawnPrefab; // Set in Unity IDE
    public Transform spawnPos;  // position of object
    public TextMeshProUGUI txtNode;

    // Variables for Platform Dimensions 
    public float myXPos;    // tracks X position when spawning
    float staticX;          // set in SetSpawnDimensions
    float myZPos;           // tracks Y position when spawning
    float maxLength;        // set in SetSpawnDimensions

    void Start() 
    {
        SetSpawnDimensions(DriveInfo.GetDrives().Length);
        int track = 1;
        foreach (var drive in DriveInfo.GetDrives()) 
        {
            // Must skip F: Drive because it's a DVD drive and Tim's computer doesn't like that
            if (drive.Name != "F:\\") 
                SpawnDriveObjects(drive, track++);
        }
    }

    // pass Game Object and Drive information
   public void SpawnDriveObjects(DriveInfo drive, int index) 
   {
        int y = 1;
        GameObject gObj = Instantiate(whatToSpawnPrefab[2], new Vector3(myXPos, y, myZPos), Quaternion.identity) as GameObject;
        myXPos += 2; 

        // reset x and change z position
        if((index) % maxLength == 0) 
        {
            myXPos = staticX;
            myZPos += 2;
        }

        // Add DataNode component and update the attributes for later usage
        gObj.name = drive.Name;
        gObj.GetComponentInChildren<TextMeshProUGUI>().text = drive.Name;
        gObj.AddComponent<DataNode>();

        DataNode dn = gObj.GetComponent<DataNode>();
        dn.Name = drive.Name;
        dn.Size = drive.TotalSize;
        dn.FullName = drive.RootDirectory.FullName;
        dn.IsFolder = true;
        dn.spawnPos = spawnPos;
        dn.Prefab = whatToSpawnPrefab;
        dn.yPos = y;
        dn.UserHasAccess = true;
        dn.txtNode = txtNode;
        dn.IsDrive = true;
    }

    // Sets and spawns all folder game objects
    public void SpawnFolderObjects(DirectoryInfo dir, int index, GameObject[] Prefab, int oldY, TextMeshProUGUI txtName) 
    {
      
        int y = ToggleY(oldY);
        var gObj =Instantiate(Prefab[1], new Vector3(myXPos, y, myZPos), spawnPos.rotation);
      
        myXPos += 3;
        // reset x and change z position
        if ((index) % maxLength == 0) 
        {
            myXPos = staticX;
            myZPos += 2;
        }

        // Add DataNode component and update the attributes for later usage
        gObj.name = dir.FullName;
        gObj.GetComponentInChildren<TextMeshProUGUI>().text = dir.Name; 
        gObj.AddComponent<DataNode>();
        DataNode dn = gObj.GetComponent<DataNode>();
        dn.Name = dir.Name;
        dn.FullName = dir.FullName;
        dn.IsFolder = true;
        dn.spawnPos = spawnPos;
        dn.yPos = y;
        dn.Prefab = Prefab;
        dn.UserHasAccess = true;
        dn.txtNode = txtName;
        dn.DateCreated = dir.CreationTime;
        dn.LastModified = dir.LastWriteTime;
    }

    // Sets and spawns all file game objects
    public void SpawnFileObjects(FileInfo file, int index, GameObject[] Prefab, int oldY, TextMeshProUGUI txtName) {
        int y = ToggleY(oldY);
        var gObj = Instantiate(Prefab[0], new Vector3(myXPos, y, myZPos), spawnPos.rotation) as GameObject;
    
        myXPos += 3;
        // reset x and change z position
        if ((index) % maxLength == 0) {
            myXPos = staticX;
            myZPos += 2;
        }

        // Add DataNode component and update the attributes for later usage
        gObj.name = file.FullName;
        gObj.GetComponentInChildren<TextMeshProUGUI>().text = file.Name;
        gObj.AddComponent<DataNode>();
   
        DataNode dn = gObj.GetComponent<DataNode>();
        dn.Name = file.Name;
        dn.Size = file.Length;
        dn.FullName = file.FullName;
        dn.Prefab = Prefab;
        dn.IsFolder = false;
        dn.spawnPos = spawnPos;
        dn.yPos = y;
        dn.Prefab = Prefab;
        dn.UserHasAccess = true;
        dn.txtNode = txtName;
        dn.DateCreated = file.CreationTime;
        dn.LastModified = file.LastWriteTime;
    }

    // Generates a Grid depending on the amount of items in the directory 
    public void SetSpawnDimensions(int length) 
    {
        float x = FindNearestSquare(length);
        float z = x;

        // Set global variables
        myXPos = (x / -2) * 1f;
        maxLength = (x * 1f) ;    
        staticX = myXPos;       
        myZPos = (x / -2) ;      

        GameObject getCamera = GameObject.Find("Main Camera");
        Vector3 cameraPosition = new Vector3(x / 1.5f, 8, -12); // adjust camera
        getCamera.transform.position = Vector3.Lerp(getCamera.transform.position, cameraPosition, 1.0f);
        GameObject.Find("Cache").GetComponent<Cache>().SetCameraPosition(cameraPosition);
    }

    /*  Function will find the closest square root to an integer 
        that is less than or equal to n
    */
    private int FindNearestSquare(int n) 
    {
        int square = 4; //smallest square we will use
        int odd = 5;

        while ((square +  odd) <= n) 
        {
                square += odd;
                odd += 2;
        }

        return Convert.ToInt32(Math.Sqrt(square));
    }

    /*  Toggle the Y position of the game objects. Y positions 
        alternate when moving directories to know which game objects
        to remove from display. 
    */
    private int ToggleY(int y) 
    {
        if(y == 1)
            y = 2;
        else
            y = 1;

        return y;
    }

} // end class