/* 
    Timothy Spengler
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
    DataNode dn; //= //this.gameObject.transform.GetComponent<DataNode>();
    //spawner spawner;
    void Start() {
        dn = this.gameObject.transform.GetComponent<DataNode>();
        //spawner = this.gameObject.transform.GetComponent<spawner>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0)) {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit)) {
                if (hit.collider.gameObject == gameObject) {

                    DirectoryInfo dirs = new DirectoryInfo(dn.FullName);
                    Debug.Log(dirs.FullName);
                    dn.SetSpawnDimensions(dirs.GetDirectories().Length + dirs.GetFiles().Length);
                    int index = 1;

                    //Spawn Folders
                    foreach (var dir in dirs.EnumerateFiles()) {
                        dn.SpawnFileObjects(dir, index++, dn.Prefab);
                    }
                    //dn.PrintDirectories();
                    //spawner.SpawnNextDirectory(dn.FullName);
                }
            }
        }
    }

    void OnMouseOver() {
        Debug.Log(dn.FullName);
        Debug.Log(dn.IsDrive);

        // testing for info
    }

    public void DestroyAllCurrentObjects() {
        
    }

    public void SpawnPreviousDirectory() {

    }

    

    void OnMouseExit() 
    {
        // Will Clear GUI Screen when mouse moves off object
        //Debug.Log("YEAHASDF");
    }


} // end class

// -- Old Code below --
//     public float lifeTime = 10f;


//if(lifeTime > 0 ) 
// {
//  lifeTime -= Time.deltaTime;
// if(lifeTime <= 0) 
// {
//    Destruction();
//}
//}

//void onCollisionEnter(Collision col) {
//    if (col.gameObject.name == "destroyer") {
//        Destruction();
//    }
//}

//void Destruction() {
//    Destroy(this.gameObject);
//}