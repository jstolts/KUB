﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(SlowMotion))]
[RequireComponent(typeof(CameraShake))]
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Transform))]
[RequireComponent(typeof(BoxCollider2D))]


public class j_PlayerController : MonoBehaviour {

    [Header("For shooting")]
    public float shootingSpeed;
    private int ID;

    [SerializeField] private Transform ThisTransform;
    [SerializeField] private Rigidbody2D ThisRB = null;
    [SerializeField] private Quaternion newRot = Quaternion.Euler(10f, 0f, 0f);


    [SerializeField] private bool switching = false;

    [Header("Other cube properties")]
    [SerializeField]
    private Vector2 scaleSize;
    [SerializeField] public Vector3 decreasingValue;
    public float rotationSpeedZ = 1f;

    [Header("For Scaling over time")]
    public float scaleDecrease;
    public float timeFloat = 120;

    [Header("Objects outside the game")]
    public GameObject targetArrow;
    public Transform otherPlayer;

    [Header("Juicy")]
    public SlowMotion slowMotion;
    public CameraShake cameraShake;
    public bool isAlive;


    [Header("Players")]
    public GameObject[] gameobjects;


    [SerializeField] bool StopMotion = false;



    private enum PlayerStates
    {
        flying, rotating, rotating_otherDirection, dead_fire, dead_electro, dead_collision, dead_projectile,
        dead_color, victory_FirstLevel, victory_SecondLevel, victory_ThirdLevel, victory_Normal,
        victory_Fast, victory_Slow, start, start_tip
    }

    private PlayerStates myState;
    // Use this for initialization
    void Start ()
    {
        myState = PlayerStates.start;
    }
	
	// Update is called once per frame
	void Update () {



        print(myState);

        if           (myState == PlayerStates.rotating)                { rotating(); }
        else if      (myState == PlayerStates.flying)                  { flying(); }
        else if      (myState == PlayerStates.dead_collision)          { dead_collision(); }


        // This is for the scaling down over time

        timeFloat -= Time.deltaTime;
        if (timeFloat > 0)

        if (Input.GetButton("Jump"))
        {
            
            myState = PlayerStates.rotating;
        }

        else if (Input.GetButtonUp("Jump"))
        {
                myState = PlayerStates.flying;
                ThisRB.constraints = 0;
        }
    }

    void start()
    {
        ThisTransform = GetComponent<Transform>();
        ThisRB = GetComponent<Rigidbody2D>();
        ThisRB.isKinematic = true;
        transform.Rotate(0, 0, Time.deltaTime * rotationSpeedZ);
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag == "Normal_Obstacle" && myState == PlayerStates.rotating || myState == PlayerStates.flying)
        {
            myState = PlayerStates.dead_collision;
            Debug.LogWarning("To tight buddy");
            //Play audio;
        }

        else if (col.tag == "Projectile" && myState == PlayerStates.rotating || myState == PlayerStates.flying)
        {
            myState = PlayerStates.dead_projectile;
            Debug.Log("You shot me dooown, but i get uuup");
            //Play audio;
        }

        else if (col.tag == "fire" && myState == PlayerStates.rotating || myState == PlayerStates.flying)
        {
            myState = PlayerStates.dead_fire;
            Debug.Log("Burn baby burn!");
            //Play audio;
        }

        else if (col.tag == "electro" && myState == PlayerStates.rotating || myState == PlayerStates.flying)
        {
            myState = PlayerStates.dead_electro;
            Debug.Log("Burn baby burn!");
            //Play audio;
        }
    }

    void rotating()
    {
        ThisRB.isKinematic = true;
        targetArrow.SetActive(true);
        transform.Rotate(0, 0, Time.deltaTime * rotationSpeedZ);
        ThisRB.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezePositionY;
    }

    void flying()
    {
        targetArrow.SetActive(false);
        ThisRB.isKinematic = false;
        ThisRB.AddForce(transform.up * shootingSpeed, ForceMode2D.Impulse);
        ThisRB.constraints = RigidbodyConstraints2D.FreezeRotation;
        // slowMotion.slowDownActive = true;
    }

    void dead_collision()
    {
        targetArrow.SetActive(false);
        cameraShake.shouldShake = true;

        ThisRB.isKinematic = true;
        //Update DeathCounter
        
        //Update UI

        //Play Sound


        isAlive = false;

        if (Input.GetButtonDown("Jump") && isAlive ==false)
        {
            Application.LoadLevel(Application.loadedLevel);
        }

       
    }





}
