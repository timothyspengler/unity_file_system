/*
    Comp 585 -- GUI 
    DataNode contains all relevant information about Drives, Files, and Folders.
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class DataNode : spawner
{
    public string Name;
    public string FullName;
    public long Size;
    public bool IsFolder = false;
    public bool IsDrive = false;
    public bool IsSelected = false; // not used yet
    public bool IsExpanded = false; // not used yets
    public GameObject[] Prefab;
    public int yPos;
    public string PrevDirectory;
    public Time DateCreated;
    public Time LastModified;
    public bool UserHasAccess; // assume its true until its not
   
    public void Start() {
        // Needs to be here or will use spawner's Start()
        // Do nothing tho
    }
}
