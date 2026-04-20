using UnityEngine;
using System.Collections;
using UnityEngine.InputSystem;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;
using System;
using Unity.Cinemachine;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip runningClip;
    [SerializeField] private AudioClip jumpClip;
    [SerializeField] private AudioClip energyOrbClip;
    [SerializeField] private AudioClip phasingClip;
    [SerializeField] private CinemachineCamera playerCamera;
    [SerializeField] private GameObject particles;
    [SerializeField] private RectTransform livesPanel;
    [SerializeField] public GameObject item_icon; // icon to show up in UI
    [SerializeField] private RectTransform tunnelPanel;

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
    [SerializeField] private RectTransform damageOverlay; 
    private int catLives = 9;
    private bool isGrounded;
    private bool hasEnergy;
    [SerializeField] private AudioClip hurtClip;
    private Animator anim;
    
    private const float force = 15f;
    private const float speed = 25f;

    void Start()
    {
        // moveAction = InputSystem.actions.FindAction("Move");
        // jumpAction = InputSystem.actions.FindAction("Jump");
        // interactAction = InputSystem.actions.FindAction("Action");
        
        rb = GetComponent<Rigidbody>();
        t = GetComponent<Transform>();
        isGrounded = false;
        hasEnergy = false;
        anim = transform.GetChild(0).GetComponent<Animator>();

        // adding lives
        for (int i = 0; i < 9; i++)
        {
            GameObject newItem = Instantiate(item_icon, livesPanel);
            // newItem.transform.SetParent(livesPanel, true);
        }
        
        UnityEngine.Cursor.lockState = CursorLockMode.Locked;
        Debug.Log("STARTING PLAYER.");
    }

    void Update()
    {
        anim.SetBool("walking", 
            Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D));

        // reset walking animations
        if (Keyboard.current == null) {
            anim.ResetTrigger("walking");
            audioSource.Stop();
        }
            
        if (Keyboard.current != null) {
            audioSource.Play();
            
            // ========== Key Movements ======================================
            Vector3 moveDir = Vector3.zero;

            if (Keyboard.current.wKey.isPressed)
                moveDir += transform.forward;

            if (Keyboard.current.sKey.isPressed)
                moveDir -= transform.forward;

            if (Keyboard.current.aKey.isPressed)
                moveDir -= transform.right;

            if (Keyboard.current.dKey.isPressed)
                moveDir += transform.right;

            moveDir = moveDir.normalized;
            

            Vector3 currentVel = rb.linearVelocity;
            // Calculate movement direction from WASD input and directly set Rigidbody velocity
            rb.linearVelocity = new Vector3(moveDir.x * speed, currentVel.y, moveDir.z * speed);
            // ===============================================================


            // ========== Jump with SPACE ====================================
            if (isGrounded && Keyboard.current.spaceKey.wasPressedThisFrame)
            {
                rb.AddForce(Vector3.up * force, ForceMode.Impulse);
                audioSource.PlayOneShot(jumpClip);
                Debug.Log("Jump!");
            }
            // ===============================================================


            // ========== PHASING THROUGH THE FORCE FIELDS ===================
            else if (Keyboard.current.qKey.wasPressedThisFrame && hasEnergy)
            {
                audioSource.PlayOneShot(phasingClip);

                var closestObj = findClosestEnergy();

                if (!closestObj) // is null
                    return;

                // closestObj.transform.GetChild(0).gameObject.GetComponent<ParticleSystem>().Play();
                Debug.Log($"Q pressed! Destroying field at " + closestObj.transform.position);
                tunnelPanel.gameObject.SetActive(false);
                hasEnergy = false;
                
                StartCoroutine(RemoveField(closestObj));
            }
            // ===============================================================
        }

        
        
        // ========== Camera Rotation ====================================
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

        float dist = 1500;
        GameObject nearest = null;
        foreach(var go in arr)
        {
            var d = (go.transform.position - pos).sqrMagnitude; 
            // Debug.Log($"Field detected at " + d);
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
        audioSource.PlayOneShot(hurtClip);

        // damage overlay
        StartCoroutine(DoDamageTick());
        

        catLives--;
        TextMeshProUGUI textBox = livesPanel.GetComponentInChildren<TextMeshProUGUI>();
        textBox.text = "Lives Left: " + catLives;
        
        // remove from panel
        Destroy(livesPanel.GetChild(1).gameObject);

        // check if zero
        if (catLives == 0)
        {
            // show restart menu
            SceneManager.LoadScene("RestartScene");
            return;
        }
    }

    // ========== COLLISION AND TRIGGER DETECTIONS =======================
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "EnergyOrb")
        {
            tunnelPanel.gameObject.SetActive(true);
            hasEnergy = true;
            audioSource.PlayOneShot(energyOrbClip);
            Debug.Log("Energy Orb collected!");
        }
    }

    void OnCollisionEnter(Collision coll)
    {
        if(coll.gameObject.tag == "Floor" || coll.gameObject.tag == "Bouncy")
        {
            isGrounded = true;
        }
    }

    void OnCollisionExit(Collision coll)
    {
        if(coll.gameObject.tag == "Floor" || coll.gameObject.tag == "Bouncy")
        {
            isGrounded = false;
        }
    }

    // ========== COROUTINE CODE =========================================
    IEnumerator DoDamageTick()
    {
        damageOverlay.gameObject.SetActive(true); 
        // Debug.Log("Coroutine started at: " + Time.time);
        
        yield return new WaitForSeconds(0.5f);
        
        damageOverlay.gameObject.SetActive(false); 
        // Debug.Log("Coroutine finished at: " + Time.time);
    }

    IEnumerator RemoveField(GameObject energyField)
    {
        // energyField.SetActive(false); 
        energyField.GetComponent<ForceField>().changeMaterial(false);
        energyField.GetComponent<BoxCollider>().enabled = false;
        // Debug.Log("Coroutine started at: " + Time.time);
        
        yield return new WaitForSeconds(2);
        
        // bring it back
        // energyField.SetActive(true);
        energyField.GetComponent<ForceField>().changeMaterial(true);
        energyField.GetComponent<BoxCollider>().enabled = true;
        // Debug.Log("Coroutine finished at: " + Time.time);
    }
}