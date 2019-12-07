/* 
    Comp 585 -- GUI
    11/16/19
 */

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;
using TMPro;

public class destroyer : MonoBehaviour 
{
    // Global Variables
    public Transform spawnPos;
    private Renderer render;
    private Color normalColor;

    //Navigation
    public TextMeshProUGUI txtNode;
    public Text txtName;
    public bool detailed;

    DataNode dn;
    DataNode cache;

    void Start() {
        dn = this.gameObject.transform.GetComponent<DataNode>();
        render = GetComponent<Renderer>();
        normalColor = render.material.color;
        txtNode = dn.txtNode;
        cache = GameObject.Find("Cache").GetComponent<DataNode>();
        detailed = false;
    }

    // Update is called once per frame
    void Update() {

        if (Input.GetMouseButtonDown(0)) {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit)) {
                if (hit.collider.gameObject == this.gameObject) {
                    if (this.dn.IsFolder) {
                        try {
                            DirectoryInfo dirs = new DirectoryInfo(dn.FullName);
                            int totalItems = dirs.GetDirectories().Length + dirs.GetFiles().Length;

                            dn.SetSpawnDimensions(totalItems);
                            int index = 1;

                            // Spawn Folders
                            foreach (var dir in dirs.EnumerateDirectories()) 
                                dn.SpawnFolderObjects(dir, index++, dn.Prefab, dn.yPos, dn.txtNode);
                            
                            // Spawn Files
                            foreach (var file in dirs.EnumerateFiles()) 
                                dn.SpawnFileObjects(file, index++, dn.Prefab, dn.yPos, dn.txtNode);

                            cache.Prefab = dn.Prefab;
                            cache.yPos = dn.yPos;
                            cache.txtNode = dn.txtNode;
                            cache.FullName = dn.FullName;
                            DestroyDirectory(dn.yPos);
                        }
                        catch(UnauthorizedAccessException) {
                            dn.UserHasAccess = false;
                            render.material.color = Color.red; //user doesn't have access
                       
                        }
                    }
                }
            }
        }
    }
    
    // On hover Display to Panel information about Object
    void OnMouseOver() {
        render.material.color = Color.magenta; // user has access
        txtNode.text = dn.Name;
    }

    // Destroys objects based on Y Position 
    private void DestroyDirectory(int y) {
        GameObject[] previous = GameObject.FindGameObjectsWithTag("Player"); 
        foreach(var prev in previous) {
            if (prev.transform.position.y == y) // Directories are spawned on different y positions
                Destroy(prev); // hide objects
        }
    }

    // Reset back to Drive Directory. Utilized by Home Button
    public void SpawnHomeDirectory() {
        spawner getSpawner = GameObject.Find("Spawner").GetComponent<spawner>();
        GameObject[] previous = GameObject.FindGameObjectsWithTag("Player");

        foreach (var prev in previous) 
            Destroy(prev);

        getSpawner.SetSpawnDimensions(DriveInfo.GetDrives().Length);
        int track = 1;

        foreach (var drive in DriveInfo.GetDrives()) {
            // Must skip F: Drive because it's a DVD drive and Tim's computer doesn't like that
            if (drive.Name != "F:\\")
                getSpawner.SpawnDriveObjects(drive, track++);
        }

        ResetMainCamera();
    }

    // Goes back to previous directory. Utilized by Step-Back Button
    public void GoBackDirectory() {
        string newDirectory;
        spawner getSpawner = GameObject.Find("Spawner").GetComponent<spawner>();
        GameObject[] gObj = GameObject.FindGameObjectsWithTag("Player"); // get objects
        DataNode previous;

        if (gObj.Length != 0) {
            previous = gObj[0].GetComponent<DataNode>();
            newDirectory = ChangeDirectoryName(previous.name);
        }
        else {
            Debug.Log("asdfadf");
            previous = GameObject.Find("Cache").GetComponent<DataNode>();
            newDirectory = NameForEmptyDirectory(previous.FullName);
        }
        
        //newDirectory = ChangeDirectoryName(previous.name);

      
        //string newDirectory = ChangeDirectoryName(previous.name);

        if(newDirectory.Length != 0) {
            DirectoryInfo dirs = new DirectoryInfo(newDirectory);
            int totalItems = dirs.GetDirectories().Length + dirs.GetFiles().Length;

            getSpawner.SetSpawnDimensions(totalItems);
            int index = 1;

            // Spawn Folders
            foreach (var dir in dirs.EnumerateDirectories())
                getSpawner.SpawnFolderObjects(dir, index++, previous.Prefab, previous.yPos, previous.txtNode);

            // Spawn Files
            foreach (var file in dirs.EnumerateFiles())
                getSpawner.SpawnFileObjects(file, index++, previous.Prefab, previous.yPos, previous.txtNode);

            DestroyDirectory(previous.yPos);
        }
        else {
            SpawnHomeDirectory();
        }
    }


    public void ResetMainCamera() {
        GameObject getCamera = GameObject.Find("Main Camera");
        Vector3 cameraPosition = new Vector3(0, 8, -12);
        float totalDistance = cameraPosition.x - getCamera.transform.position.x;
        getCamera.transform.position = Vector3.Lerp(getCamera.transform.position, cameraPosition, 1.0f);
    }

    // Slices String to previous forward slash
    private string ChangeDirectoryName(string name) {
        int index;

        index = name.LastIndexOf(@"\");
        name = name.Substring(0, index);

        // check if it's drive directory
        if (name[name.Length - 1].ToString() == ":") 
            return ""; // set length to zero 
        
        index = name.LastIndexOf(@"\");
        name = name.Substring(0, index);

        // Check if it's a drive directory again
        if (name[name.Length - 1].ToString() == ":") 
            name += @"\"; // add back the slash

        return name;
    }

    // only have to go back one slash from empty directory
    private string NameForEmptyDirectory(string name) {
        int index = name.LastIndexOf(@"\");
        name = name.Substring(0, index);

        // Check if it's a drive directory again
        if (name[name.Length - 1].ToString() == ":")
            name += @"\"; // add back the slash

        return name;
    }

    void OnMouseExit() 
    {
        if (!dn.UserHasAccess) 
            render.material.color = Color.red;
        else 
            render.material.color = normalColor;
        
    }


} // end class