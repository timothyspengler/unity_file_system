/* 
    Comp 585 -- GUI
 */

using System;
using UnityEngine;
using System.IO;
using UnityEngine.UI;
using TMPro;

public class Directories : MonoBehaviour 
{
    private Renderer render;
    private Color normalColor;

    //Navigation
    public TextMeshProUGUI txtNode;
    public TextMeshProUGUI txtNodeDetailed;
    public Text txtName;
    public bool detailed;

    DataNode dn;
    DataNode cache; // saves previous directory in case we get lost (empty directory)
    DataNode driveCache;

    void Start() 
    {
        dn = this.gameObject.transform.GetComponent<DataNode>();
        render = GetComponent<Renderer>();
        normalColor = render.material.color;
        txtNode = dn.txtNode;
        txtNodeDetailed = dn.txtNodeDetailed;
        cache = GameObject.Find("Cache").GetComponent<DataNode>();
        driveCache = GameObject.Find("DriveCache").GetComponent<DataNode>();
        detailed = false;
    }

    // Update is called once per frame
    void Update() 
    {
        if (Input.GetMouseButtonDown(0)) 
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit)) 
            {
                if (hit.collider.gameObject == this.gameObject) 
                {
                    if (this.dn.IsFolder) 
                    {
                        try 
                        {
                            DirectoryInfo dirs = new DirectoryInfo(dn.FullName);
                            int totalItems = dirs.GetDirectories().Length + dirs.GetFiles().Length;

                            dn.SetSpawnDimensions(totalItems);
                            int index = 1;

                            // Spawn Folders
                            foreach (var dir in dirs.EnumerateDirectories())
                                dn.SpawnFolderObjects(dir, index++, dn.Prefab, dn.yPos, dn.txtNode, dn.txtNodeDetailed);

                            // Spawn Files
                            foreach (var file in dirs.EnumerateFiles())
                                dn.SpawnFileObjects(file, index++, dn.Prefab, dn.yPos, dn.txtNode, dn.txtNodeDetailed);

                            if (this.dn.IsDrive)
                            {
                                driveCache.FullName = dn.FullName;
                            }
                            
                       
                                // Cache previous directory
                                cache.Prefab = dn.Prefab;
                                cache.yPos = dn.yPos;
                                cache.txtNode = dn.txtNode;
                            cache.txtNodeDetailed = dn.txtNodeDetailed;
                                cache.FullName = dn.FullName;
                                cache.Name = dn.Name;
                            
                     

                            GameObject directTxt = GameObject.Find("DirectoryText");
                            directTxt.GetComponent<TextMeshProUGUI>().text = "Location: " + dn.Name;

                            // Destroy Previous Directory
                            DestroyDirectory(dn.yPos);
                        }
                        catch (UnauthorizedAccessException) 
                        {
                            dn.UserHasAccess = false;
                            render.material.color = Color.red; //user doesn't have access
                        }
                    }
                    
                }
            }
        }
    }

    // On hover Display to Panel information about Object
    void OnMouseOver() 
    {
        render.material.color = Color.magenta; // user has access
        txtNode.text = dn.Name;

        if(dn.IsDrive)
        {
            txtNodeDetailed.text = "Type: Drive"  + "\n"
           + "Location:" + dn.FullName + "\n"
           + "Size:" + dn.Size + "\n"
         ;
        }
        else
        {
            String type = (dn.IsFolder ? "Folder" : "File");
            txtNodeDetailed.text = "Type:" + type + "\n"
                + "Location:" + dn.FullName + "\n"
                + "Size:" + dn.Size + "\n"
                + "Created" + dn.DateCreated + "\n"
                + "Modified" + dn.LastModified
              ;
        }
        
    }

    // Destroys objects based on Y Position 
    private void DestroyDirectory(int y) 
    {
        GameObject[] previous = GameObject.FindGameObjectsWithTag("Player"); 

        foreach(var prev in previous) 
        {
            if (prev.transform.position.y == y) // Directories are spawned on different y positions
                Destroy(prev); // hide objects
        }

        GameObject navText = GameObject.Find("txtNode");
        GameObject navTextDetailed = GameObject.Find("txtNodeDetailed");

        navText.GetComponent<TextMeshProUGUI>().text = ""; // reset nav text
    }

    // Reset back to Drive Directory. Utilized by Home Button
    public void SpawnHomeDirectory() 
    {
        spawner getSpawner = GameObject.Find("Spawner").GetComponent<spawner>();
        GameObject directTxt = GameObject.Find("DirectoryText");
        directTxt.GetComponent<TextMeshProUGUI>().text = ""; // reset location text

        DestroyDirectory(1); // y-axis = 1
        DestroyDirectory(2); // y-axis = 2

        getSpawner.SetSpawnDimensions(DriveInfo.GetDrives().Length);
        int track = 1;

        foreach (var drive in DriveInfo.GetDrives()) 
        {
            // Must skip F: Drive because it's a DVD drive and Tim's computer doesn't like that
            if (drive.Name != "F:\\")
                getSpawner.SpawnDriveObjects(drive, track++);
        }

        //txtNode.text = ""; // reset text display 
        ResetMainCamera();
    }

    // Goes back to previous directory. Utilized by Step-Back Button
    public void GoBackDirectory() 
    {
        string newDirectory;
        spawner getSpawner = GameObject.Find("Spawner").GetComponent<spawner>();
        GameObject[] gObj = GameObject.FindGameObjectsWithTag("Player"); // get objects
        DataNode previous;

        if (gObj.Length != 0) 
        {
            previous = gObj[0].GetComponent<DataNode>();
            newDirectory = FormatDirectoryName(previous.name);
        }
        else 
        {
            previous = GameObject.Find("Cache").GetComponent<DataNode>();
            newDirectory = EmptyDirectoryStepBackName(previous.FullName);
        }

        GameObject directTxt = GameObject.Find("DirectoryText");
        directTxt.GetComponent<TextMeshProUGUI>().text = "Location: " + newDirectory;

        if (newDirectory.Length != 0) 
        {
            DirectoryInfo dirs = new DirectoryInfo(newDirectory);

            int totalItems = dirs.GetDirectories().Length + dirs.GetFiles().Length;

            getSpawner.SetSpawnDimensions(totalItems);
            int index = 1;

            // Spawn Folders
            foreach (var dir in dirs.EnumerateDirectories())
                getSpawner.SpawnFolderObjects(dir, index++, previous.Prefab, 
                    previous.yPos, previous.txtNode, previous.txtNodeDetailed);

            // Spawn Files
            foreach (var file in dirs.EnumerateFiles())
                getSpawner.SpawnFileObjects(file, index++, previous.Prefab,
                    previous.yPos, previous.txtNode, previous.txtNodeDetailed);

            DestroyDirectory(previous.yPos);
        }
        else 
        {
            SpawnHomeDirectory();
        }
    }


    public void ResetMainCamera() 
    {
        GameObject getCamera = GameObject.Find("Main Camera");
        Vector3 cameraPosition = GameObject.Find("Cache").GetComponent<Cache>().GetCameraObject();
        
        // float totalDistance = cameraPosition.x - getCamera.transform.position.x;
        getCamera.transform.rotation = Quaternion.Euler(15, 0, 0);
        getCamera.transform.position = Vector3.Lerp(getCamera.transform.position,cameraPosition, 1.0f);
    }

    // Slices String to previous forward slash
    private string FormatDirectoryName(string name) 
    {
        try // try for windows
        {
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
        catch (ArgumentOutOfRangeException) // its a mac computer
        {
            string Drivecache = GameObject.Find("DriveCache").GetComponent<DataNode>().FullName;

            int index;
            index = name.LastIndexOf(@"/");

            if(index != 0)
            {
                name = name.Substring(0, index);

                if(name == Drivecache)
                    return "";

                index = name.LastIndexOf(@"/");
                if (index != 0)
                    name = name.Substring(0, index);
                else
                    name = Drivecache;
                
                return name;
            }
            else
            {
                return "";
            }
        }
    }

    // only have to go back one slash from empty directory
    private string EmptyDirectoryStepBackName(string name) 
    {

        try //assume its windows
        {
            int index = name.LastIndexOf(@"\");
            name = name.Substring(0, index);

            // Check if it's a drive directory again
            if (name[name.Length - 1].ToString() == ":")
                name += @"\"; // add back the slash

            return name;
        }
        catch (ArgumentOutOfRangeException) // its a mac
        {
            string Drivecache = GameObject.Find("DriveCache").GetComponent<DataNode>().FullName;

            if (name != Drivecache)
            {
                int index = name.LastIndexOf(@"/");
                if (index != 0)
                    name = name.Substring(0, index);
                else
                    name = Drivecache;

                return name;
            }
            else
            {
                return "";
            }
        
        }
        
    }

    void OnMouseExit() 
    {
        if (!dn.UserHasAccess) 
            render.material.color = Color.red;
        else 
            render.material.color = normalColor;
    }

} // end class