using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreeCamera : MonoBehaviour
{
    [SerializeField] GameObject playerCamera;

    //  Mouse

    public float viewSensitivity = 500.0f;      //  Sensitivity settigs
    public bool b_invertX, b_invertY;           //  Inversion settings

    float xRotation = 0.0f;                     //  Current camera rotation


    //  Defauld input:
    //  WASD/arrows - horizontal plane
    //  E - up      Q - down
    //  Mouse - turn camera

    [SerializeField] float maxSpeed = 15.0f;
    [SerializeField] float acceleration = 8.0f;

    [SerializeField] float brakingForce = 6.0f;    //  Gradual slowdown 

    Vector3 speed;

    private void Start()
    {
        speed = new Vector3(0.0f, 0.0f, 0.0f);
        Cursor.lockState = CursorLockMode.Locked;   //  Disabling cursor
        if (!playerCamera)
            Debug.Log("Missing player camera object for FreeCamera script");
    }

    void Update()
    {
        //  Mouse
        float mouseX = Input.GetAxis("Mouse X") * viewSensitivity * Time.deltaTime; //  Getting mouse position for player
        float mouseY = Input.GetAxis("Mouse Y") * viewSensitivity * Time.deltaTime; //  Getting mouse position for camera

        if (b_invertY)                                                  //  Camera rotation inversion
            xRotation += mouseY;
        else
            xRotation -= mouseY;

        xRotation = Mathf.Clamp(xRotation, -90f, 90f);                  //  Limiting camera rotation
        playerCamera.transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);  //  Rotating camera
        transform.Rotate(Vector3.up * mouseX);                          //  Rotating player



        //  Keyboard
        float x = Input.GetAxis("Sideways");  //  Getting input
        float y = Input.GetAxis("Vertical");
        float z = Input.GetAxis("Forward");

        speed +=    transform.right * x * acceleration * Time.deltaTime + 
                    transform.up * y * acceleration * Time.deltaTime + 
                    transform.forward * z * acceleration * Time.deltaTime; //  Calculating raw horizontal movement

        if (speed.magnitude > maxSpeed) //  Limiting the speed
            speed = speed.normalized * maxSpeed;

        speed -= speed.normalized * brakingForce * Time.deltaTime;  //  Slowing down with time

        this.transform.position += speed * Time.deltaTime;
    }
}
