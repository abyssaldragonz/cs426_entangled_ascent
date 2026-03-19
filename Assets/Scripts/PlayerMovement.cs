using UnityEngine;
using System.Collections;
using UnityEngine.InputSystem;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UIElements;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private AudioListener audioListener;
    [SerializeField] private Camera playerCamera;
    [SerializeField] private GameObject particles;
    [SerializeField] private TextMeshProUGUI livesPanel;


    // public float rotationSpeed = 90;
    public float force = 700f;
    public float speed = 2f;


    // keep track of camera rotation
    private float pitch = 0.0f;
    private float yaw = 0.0f;


    Rigidbody rb;
    Transform t;
    

    // Lives and game state tracking
    int catLives = 9;
    private bool isGrounded;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        t = GetComponent<Transform>();
        isGrounded = false;
        Debug.Log("STARTING PLAYER.");
    }

    void Update()
    {
        // ========== Key Movements =====================================
        if (Input.GetKey(KeyCode.W))
        {
            rb.linearVelocity += this.transform.forward * speed * Time.deltaTime;
        }
        else if (Input.GetKey(KeyCode.S))
        {
            rb.linearVelocity -= this.transform.forward * speed * Time.deltaTime;
        }

        if (Input.GetKey(KeyCode.A))
        {
            // t.rotation *= Quaternion.Euler(0, -rotationSpeed * Time.deltaTime, 0);
            rb.linearVelocity -= this.transform.right * speed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.D))
        {
            // t.rotation *= Quaternion.Euler(0, rotationSpeed * Time.deltaTime, 0);
            rb.linearVelocity += this.transform.right * speed * Time.deltaTime;
        }
        // ===============================================================


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


        // ========== Jump with SPACE ====================================
        if (isGrounded && Input.GetKeyDown(KeyCode.Space))
        {
            rb.AddForce(Vector3.up * 15f, ForceMode.Impulse);
            Debug.Log("Jump!");
        }
        // ===============================================================


        // ========== PHASING THROUGH THE FORCE FIELDS ===================
        if (Input.GetKeyDown(KeyCode.Q))
        {
            var closestObj = findClosestEnergy();

            if (!closestObj) // is null
                return;

            // closestObj.transform.GetChild(0).gameObject.GetComponent<ParticleSystem>().Play();
            GameObject newParticles = Instantiate(particles);
            newParticles.transform.position = closestObj.transform.position + new Vector3(0,5,0);
            Debug.Log($"Q pressed! Destroying field at " + closestObj.transform.position);
            Destroy(closestObj); // remove the force field that is closest to player
        }
        // ==============================================================
    }

    // ========== HELPER FUNCTION FOR PHASING ===========================
    private GameObject findClosestEnergy()
    {
        var arr = GameObject.FindGameObjectsWithTag("Tunnelable");
        var pos = transform.position;

        float dist = 85;
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

    // ========== FUNCTION FOR LOSING LIFE ==============================
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