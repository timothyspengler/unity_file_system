/* 
    Timothy Spengler
    Comp 585 -- GUI
    11/16/19
 */

using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System;
using UnityEngine;


public class spawner : MonoBehaviour
{
    // Global Variables
    public GameObject[] whatToSpawnPrefab;
    public Transform spawnPos;  // position of object
    DataNode dn;

    // Platform Dimensions 
    int grid;
    float myXPos; // tracks X position when spawning
    float staticX;
    float myZPos;  // tracks Y position when spawning
    float maxLength;
    public int directorySize;
    // Must skip F: Drive because it's a DVD drive and it doesn't like that
    void Start() 
    {
        spawnPlatform(DriveInfo.GetDrives().Length);
        int track = 1;
        foreach (var drive in DriveInfo.GetDrives()) {
            if (drive.Name != "F:\\") 
                SpawnDriveObjects(drive, track++);
        }

        // -- PROFESSORS CODE END -- 
    }

    // probably will need. 
    void Update() 
    {

    }

    // pass Game Object and Drive information from
    void SpawnDriveObjects(DriveInfo drive, int index) 
    {
        int y = 1;
        //int z = -4;
        var gObj = Instantiate(whatToSpawnPrefab[0], new Vector3(myXPos, y, myZPos), spawnPos.rotation) as GameObject;
        myXPos += 2; 

        // reset x and change z position
        
        if((index) % maxLength == 0) 
        {
            myXPos = staticX;
            myZPos += 2;
        }

        // Position the game object in world space
        //gObj.transform.position = new Vector3(x, y, z);
        gObj.name = drive.Name;

        // Add DataNode component and update the attributes for later usage
        gObj.AddComponent<DataNode>();
        DataNode dn = gObj.GetComponent<DataNode>();
        dn.Name = drive.Name;
        dn.Size = drive.TotalSize;
        dn.FullName = drive.RootDirectory.FullName;
        dn.IsDrive = true;
        //dn.Folders = new DirectoryInfo(dn.FullName);
        dn.Folders = drive.RootDirectory.GetDirectories();
        dn.Files = drive.RootDirectory.GetFiles();
    }

    // Generates a platform depending on the amount of items in the directory 
    void spawnPlatform(int length) {
        float x = FindNearestSquare(length);
        float z = x;
        float y = .2f;  // independent to x and z

        myXPos = (x / -2) * 1f;      // for placing objects
        maxLength = (x * 1f) ;    // for placing objects
        Debug.Log(maxLength);
        staticX = myXPos;       // for placing objects
        myZPos = (x / -2) ;      // for placing objects

        // Create Platform and set position and dimensions. 
      //  GameObject platform = GameObject.CreatePrimitive(PrimitiveType.Cube);

       // platform.transform.position = new Vector3(0, 0, 0);
        //platform.transform.localScale = new Vector3(x * 2f, y, x * 2f);

    }

    public void SpawnNextDirectory(DirectoryInfo[] folders, FileInfo[] files) {
        Debug.Log("we got here ");
        Debug.Log(folders[0]);
    }

    /*  Function will find the closest square root to an integer 
        that is less than or equal to n
    */
    private int FindNearestSquare(int n) {
        
        int square = 4; //smallest square we will use
        int odd = 5;

        while ((square +  odd) <= n) {
                square += odd;
                odd += 2;
      
        }
       // square += odd; //needs this to prevent objects from falling
        //return square;
        return Convert.ToInt32(Math.Sqrt(square));
    }
} // end class