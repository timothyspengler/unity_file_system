/* 
    Comp 585 -- GUI
    11/16/19
 */

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;


public class destroyer : MonoBehaviour 
{
    // Global Variables
    public Transform spawnPos;
    private Renderer render;
    private Color normalColor;
    DataNode dn;

    void Start() {
        dn = this.gameObject.transform.GetComponent<DataNode>();
        render = GetComponent<Renderer>();

        normalColor = render.material.color;
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

                            //if (totalItems > 0) {
                            dn.SetSpawnDimensions(totalItems);
                            int index = 1;

                            // Spawn Folders
                            foreach (var dir in dirs.EnumerateDirectories()) 
                                dn.SpawnFolderObjects(dir, index++, dn.Prefab, dn.yPos, dn.FullName);
                            
                            // Spawn Files
                            foreach (var file in dirs.EnumerateFiles()) 
                                dn.SpawnFileObjects(file, index++, dn.Prefab[0], dn.yPos, dn.FullName);
    
                            DestroyPreviousDirectory(dn.yPos);
                            //}
                        }
                        catch(UnauthorizedAccessException) {
                            dn.UserHasAccess = false;
                            render.material.color = Color.red;
                       
                        }
                    }
                    }
                }
            }
        }
    

    // On hover Display to Panel information about Object
    void OnMouseOver() {
        render.material.color = Color.magenta; // user has access
    }

    // Destroys objects based on Y Position 
    private void DestroyPreviousDirectory(int y) {
        GameObject[] previous = GameObject.FindGameObjectsWithTag("Player"); 
        foreach(var prev in previous) {
            if(prev.transform.position.y == y) {
                Destroy(prev);
            }
        }                     
    }

    
    public void SpawnPreviousDirectory() {

    }

    private bool CheckAuthorization() {
        // gonna check if user is authorized 
        return false;
    }

   
    void OnMouseExit() 
    {
        if (!dn.UserHasAccess) 
            render.material.color = Color.red;
        else 
            render.material.color = normalColor;

    }


} // end class