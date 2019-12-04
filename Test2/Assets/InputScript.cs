using System.Collections;
using UnityEngine.UI;
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
    public GameObject triggerCube;
    public bool allDead = false;
    public GameObject audioSounds;
    public AudioClip clip;
    LineRenderer lr;
    public bool shootSound = false;

    public List<GameObject> Enemies = new List<GameObject>();
    //public GameObject Enemy2;
    //public GameObject Enemy3;
    //public GameObject Enemy4;
    //public GameObject Enemy5;

    public bool grab = true;
    float disGrab;

    bool toggleRay = true;

    //int timer = 0;

    public Text timerText;
    public Text shotCounter; 
    private float startTime;

    private int counter; 
    float t; // We gon set this bitch to the time elapsed since start of the game

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
        startTime = Time.time; // time is started as soon as the application starts. But we want to start the timer a little later. 
        audioSounds.GetComponent<AudioSource>();
        
        counter = 0;
        lr = GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    void Update()
    {

        if (allDead == false) // if even one target is alive
        {
            t = Time.time - startTime; // the amount of time since the time has started (still dont understand this completely)

            string minutes = ((int)t / 60).ToString();
            string seconds = (t % 60).ToString("f2");

            timerText.text = minutes + ":" + seconds;

            shotCounter.text = "Shot Counter: " + counter;
        }

        //timer++;
        if (toggleRay == true)
        {
            //Debug.DrawRay(transform.position, transform.forward, Color.green);
            lr.enabled = true;
        }
        else
        {
            lr.enabled = false;
        }

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
                       // bullet mark texture appears here maybe??? 
                    //}
                //

            }
        }
       // }


        if (grabbed != null) // where the gun will spawn in relation to the oculus controller
        {
            grabbed.transform.position = controller.transform.position + controller.transform.forward; //controller.transform.forward;// - new Vector3(0, 0.2f, 0.5f);// * disGrab;
            grabbed.transform.rotation = controller.transform.rotation;
        }


       
            if (triggerUp) // when trigger is released
            {
            
            counter++;
            shootSound = true;
            
            RaycastHit hit;
            
            if (Physics.Raycast(transform.position, transform.forward, out hit, Mathf.Infinity))
                {

                if (hit.collider.tag == "Enemies")
                    {
                  
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
                        else if (hit.collider.name == "Cubetest")
                        {
                            counter = 0;
                            Enemies[0].SetActive(true);
                            Enemies[1].SetActive(true);
                            Enemies[2].SetActive(true);
                            Enemies[3].SetActive(true);
                            Enemies[4].SetActive(true);
                       
                            allDead = false;
                            triggerCube.transform.position = new Vector3(0, 90.25f, 3.87f);
                            
                            
                            startTime = Time.time;
                        }

                        // if all the dummies are destroyed, then setActive(true). 

                    }

                }
        }
       
        
            if (Enemies[0].activeInHierarchy == false && Enemies[1].activeInHierarchy == false && Enemies[2].activeInHierarchy == false && Enemies[3].activeInHierarchy == false && Enemies[4].activeInHierarchy == false)
            {
                allDead = true;
                triggerCube.transform.position = new Vector3(0, 3.25f, 3.87f);
           

        }


        if (touchPad)
        {
            toggleRay = !toggleRay; // the togggle of line renderer
            //RaycastHit hit;
            //if (Physics.Raycast(transform.position, transform.forward, out hit, Mathf.Infinity))
            //{
            //   // if (hit.collider.gameObject.tag == "Floor") // movement code
            //   // {
            //   //     player.transform.position = hit.point + hit.normal * 4.0f;
            //   // }
                
            //}
        }

        if(shootSound == true)
        {
            audioSounds.SetActive(true);
            timer++;
            
        }
        if(timer > 30)
        {
            shootSound = false;
            audioSounds.SetActive(false);
            timer = 0;
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
