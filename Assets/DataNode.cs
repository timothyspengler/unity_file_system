/*
    Comp 585 -- GUI
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
    public bool IsFolder;
    public bool IsDrive;
    public bool IsSelected = false;
    public bool IsExpanded = false;
    public DirectoryInfo[] Folders;
    public FileInfo[] Files;
    public GameObject Prefab;



    // Prints the folders that are contained in the Node
    public void PrintDirectories() {
        foreach(var i in Folders) 
            Debug.Log(i);
    }

    // Prints the files that are contained in the Node
    public void PrintFiles() {
        foreach (var i in Files)
            Debug.Log(i);
    }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    void Update() {

    }

   

    // Update is called once per frame

}
