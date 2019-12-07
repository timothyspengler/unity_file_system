using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;
using TMPro;

public class PreviousDirectory : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void SpawnHomeDirectory() 
    {
        GameObject getSpawner = GameObject.Find("Spawner");
        spawner setSpawner = getSpawner.GetComponent<spawner>();
        GameObject[] previous = GameObject.FindGameObjectsWithTag("Player");
        foreach (var prev in previous) {
                Destroy(prev);
        }
        setSpawner.SetSpawnDimensions(DriveInfo.GetDrives().Length);
        int track = 1;
        foreach (var drive in DriveInfo.GetDrives()) {
            // Must skip F: Drive because it's a DVD drive and Tim's computer doesn't like that
            if (drive.Name != "F:\\")
                setSpawner.SpawnDriveObjects(drive, track++);
        }
    }
}
