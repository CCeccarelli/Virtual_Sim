using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InputScript : MonoBehaviour
{
    //Check if used has controller
    public static UnityAction<bool> onHasController = null;

    private bool hasController = false;
    private bool inputActive = true;


    public GameObject player;
    public GameObject grabbed;
    public GameObject gun;
    public GameObject controller;

    public List<GameObject> Enemies = new List<GameObject>();
    //public GameObject Enemy2;
    //public GameObject Enemy3;
    //public GameObject Enemy4;
    //public GameObject Enemy5;

    public bool grab = true;
    float disGrab;

    int timer = 0;

    private void Awake()
    {
        OVRManager.HMDMounted += PlayerFound;
        OVRManager.HMDUnmounted += PlayerLost;
    }

    private void OnDestroy()
    {
        OVRManager.HMDMounted -= PlayerFound;
        OVRManager.HMDUnmounted -= PlayerFound;
    }
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        timer++;
        Debug.DrawRay(transform.position, transform.forward, Color.green);

        if (!inputActive)
        {
            return;
        }

        hasController = CheckForController(hasController);

        bool triggerDown = OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger);

        bool triggerUp = OVRInput.GetUp(OVRInput.Button.PrimaryIndexTrigger);

        bool touchPad = OVRInput.GetDown(OVRInput.Button.PrimaryTouchpad);

        //Grab objects
        //if (grabbed == null)
        //{
        if (grab == true)
        {
            RaycastHit hit;


            if (Physics.Raycast(transform.position, transform.forward, out hit, Mathf.Infinity))
            {
                //if (hit.collider != null)
                //{
                    //if (hit.collider.gameObject.tag == "Grabbable")
                    //{
                        grabbed = gun;
                        disGrab = hit.distance;
                    //}
                //

            }
        }
       // }


        if (grabbed != null)
        {
            grabbed.transform.position = controller.transform.position + controller.transform.forward;// * disGrab;
            grabbed.transform.rotation = controller.transform.rotation;
        }



        if (triggerUp)
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, transform.forward, out hit, Mathf.Infinity))
            {
                if (hit.collider.tag == "Enemies")
                {
                    //grabbed = null;
                    
                        if (hit.collider.name == "Target_Dummy1")
                        {
                            Enemies[0].SetActive(false);
                            //Enemies.RemoveAt(i);
                            //Destroy(Enemies[i]);
                        }
                        else if (hit.collider.name == "Target_Dummy2")
                        {
                            Enemies[1].SetActive(false);
                            //Enemies.RemoveAt(i);
                            //Destroy(Enemies[i]);
                        }
                        else if (hit.collider.name == "Target_Dummy3")
                        {
                            Enemies[2].SetActive(false);
                            //Enemies.RemoveAt(i);
                            //Destroy(Enemies[i]);
                        }
                        else if (hit.collider.name == "Target_Dummy4")
                        {
                            Enemies[3].SetActive(false);
                            //Enemies.RemoveAt(i);
                            //Destroy(Enemies[i]);
                        }
                        else if (hit.collider.name == "Target_Dummy5")
                        {
                            Enemies[4].SetActive(false);
                            //Enemies.RemoveAt(i);
                            //Destroy(Enemies[i]);
                        }
                    
                }
            }
               // if (hit.collider.gameObject.tag == "Enemy2")
               // {
               //     Enemy2.SetActive(false);
               // }
               // if (hit.collider.gameObject.tag == "Enemy3")
               // {
               //     Enemy3.SetActive(false);
               // }
               // if (hit.collider.gameObject.tag == "Enemy4")
               // {
               //     Enemy4.SetActive(false);
               // }
               // if (hit.collider.gameObject.tag == "Enemy5")
               // {
               //     Enemy5.SetActive(false);
               // }
               //else if(Enemy1.activeSelf == false && Enemy2.activeSelf == false && Enemy3.activeSelf == false && Enemy4.activeSelf == false && Enemy5.activeSelf == false && timer >= 600)
               // {
               //     Enemy1.SetActive(true);
               //     Enemy2.SetActive(true);
               //     Enemy3.SetActive(true);
               //     Enemy4.SetActive(true);
               //     Enemy5.SetActive(true);
               //     timer = 0;
               // }

            
        }


        if (touchPad)
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, transform.forward, out hit, Mathf.Infinity))
            {
                if (hit.collider.gameObject.tag == "Floor")
                {
                    player.transform.position = hit.point + hit.normal * 4.0f;
                }
                
            }
        }
                
        

    }

    private bool CheckForController(bool currentValue)
    {
        bool controllerCheck = OVRInput.IsControllerConnected(OVRInput.Controller.RTrackedRemote) 
            || OVRInput.IsControllerConnected(OVRInput.Controller.LTrackedRemote);

        if(currentValue == controllerCheck)
        {
            return currentValue;
        }

        if (onHasController !=null)
        {
            onHasController(controllerCheck);
        }
        return controllerCheck;
    }

    private void PlayerFound()
    {
        inputActive = true;
    }

    private void PlayerLost()
    {
        inputActive = false;
    }
}
