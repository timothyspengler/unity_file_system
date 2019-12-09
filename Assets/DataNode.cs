/*
    Comp 585 -- GUI 
    DataNode contains all relevant information about Drives, Files, and Folders.
 */

using UnityEngine;
using System;

public class DataNode : spawner
{
    public string Name;
    public string FullName;
    public long Size;
    public bool IsFolder;
    public bool IsDrive;
    public bool IsSelected; // not used yet
    public bool IsExpanded; // not used yets
    public GameObject[] Prefab;
    public int yPos;
    public DateTime DateCreated;
    public DateTime LastModified;
    public bool UserHasAccess; 

    public void Start() 
    {
        // Needs to be here or will use spawner's Start()
        // Do nothing tho
    }
}
