/* 
    Timothy Spengler
    Comp 585 -- GUI
    11/16/19
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class destroyer : MonoBehaviour 
{
    // Global Variables
    public Transform spawnPos;
    DataNode dn; //= //this.gameObject.transform.GetComponent<DataNode>();
    spawner spawn = new spawner();
    void Start() {
        dn = this.gameObject.transform.GetComponent<DataNode>();
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
                if (hit.collider.gameObject == gameObject) 
                {

                    dn.PrintDirectories();
                    spawn.SpawnNextDirectory(dn.Folders,dn.Files);
                }
            }
        }
    }

    public void DestroyAllCurrentObjects() {
        foreach(GameObject o in Object.FindObjectsOfType<GameObject>()) {
            if(o.tag == "Player") {
                Destroy(o);

            }
        }
        
    }

    public void SpawnPreviousDirectory() {

    }

    void OnMouseOver() 
    {
        //Debug.Log(dn.FullName + " " + dn.IsDrive);
        // testing for info
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