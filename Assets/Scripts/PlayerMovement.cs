using UnityEngine;
using System.Collections;
using UnityEngine.InputSystem;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UIElements;
using System;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private AudioListener audioListener;
    [SerializeField] private Camera playerCamera;
    [SerializeField] private GameObject particles;
    [SerializeField] private TextMeshProUGUI livesPanel;

    // movement related input
    // InputAction moveAction;
    // InputAction jumpAction;
    // InputAction interactAction;

    // keep track of camera rotation
    private float pitch = 0.0f;
    private float yaw = 0.0f;


    Rigidbody rb;
    Transform t;
    

    // Lives and game state tracking
    private int catLives = 9;
    private bool isGrounded;
    
    private const float force = 750f;
    private const float speed = 5f;

    void Start()
    {
        // moveAction = InputSystem.actions.FindAction("Move");
        // jumpAction = InputSystem.actions.FindAction("Jump");
        // interactAction = InputSystem.actions.FindAction("Action");
        
        rb = GetComponent<Rigidbody>();
        t = GetComponent<Transform>();
        isGrounded = false;
        Debug.Log("STARTING PLAYER.");
    }

    void Update()
    {
        if (Keyboard.current != null) {
            // ========== Key Movements ======================================
            if (Keyboard.current.wKey.isPressed)
            {
                rb.linearVelocity += this.transform.forward * speed * Time.deltaTime;
            }
            else if (Keyboard.current.sKey.isPressed)
            {
                rb.linearVelocity -= this.transform.forward * speed * Time.deltaTime;
            }

            if (Keyboard.current.aKey.isPressed)
            {
                // t.rotation *= Quaternion.Euler(0, -rotationSpeed * Time.deltaTime, 0);
                rb.linearVelocity -= this.transform.right * speed * Time.deltaTime;
            }
            if (Keyboard.current.dKey.isPressed)
            {
                // t.rotation *= Quaternion.Euler(0, rotationSpeed * Time.deltaTime, 0);
                rb.linearVelocity += this.transform.right * speed * Time.deltaTime;
            }
            // ===============================================================


            // ========== Jump with SPACE ====================================
            if (isGrounded && Keyboard.current.spaceKey.isPressed)
            {
                rb.AddForce(Vector3.up * force * Time.deltaTime, ForceMode.Impulse);
                Debug.Log("Jump!");
            }
            // ===============================================================


            // ========== PHASING THROUGH THE FORCE FIELDS ===================
            else if (Keyboard.current.qKey.isPressed)
            {
                var closestObj = findClosestEnergy();

                if (!closestObj) // is null
                    return;

                // closestObj.transform.GetChild(0).gameObject.GetComponent<ParticleSystem>().Play();
                Debug.Log($"Q pressed! Destroying field at " + closestObj.transform.position);
                Destroy(closestObj); // remove the force field that is closest to player
            }
            // ===============================================================
        }
        
        // ========== Camera Rotation ====================================
        UnityEngine.Cursor.lockState = CursorLockMode.Locked;
        pitch += 5f * Input.GetAxis("Mouse X");
        yaw -= 5f * Input.GetAxis("Mouse Y");
         if (yaw < -15)
            yaw = -15;
        if (yaw > 55)
            yaw = 55;
        transform.eulerAngles = new Vector3(yaw, pitch, 0.0f); // rotate player horizontally
        // playerCamera.transform.eulerAngles = new Vector3(yaw, playerCamera.transform.eulerAngles.y, playerCamera.transform.eulerAngles.z); // rotate camera vertically
        // playerCamera.transform.RotateAround(transform.position, Vector3.up, h * Time.deltaTime);
        // ===============================================================
    }


    // ========== HELPER FUNCTION FOR PHASING ============================
    private GameObject findClosestEnergy()
    {
        var arr = GameObject.FindGameObjectsWithTag("Tunnelable_Wall");
        var pos = transform.position;

        float dist = 100;
        GameObject nearest = null;
        foreach(var go in arr)
        {
            var d = (go.transform.position - pos).sqrMagnitude; 
            if (d < dist)
            {
                nearest = go;
                dist = d;
            }
        }

        return nearest;
    }


    // ========== FUNCTION FOR LOSING LIFE ===============================
    public void LoseLife()
    {
        catLives--;
        livesPanel.text = "Lives Left: " + catLives;
    }

    void OnCollisionEnter(Collision coll)
    {
        if(coll.gameObject.tag == "Floor")
        {
            isGrounded = true;
        }
    }

    void OnCollisionExit(Collision coll)
    {
        if(coll.gameObject.tag == "Floor")
        {
            isGrounded = false;
        }
    }
}