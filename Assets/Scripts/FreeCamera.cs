using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreeCamera : MonoBehaviour
{
    //  Defauld input:
    //  WASD/arrows - horizontal plane
    //  E - up      Q - down
    //  Mouse - turn camera

    [SerializeField] float maxSpeed = 10.0f;
    [SerializeField] float acceleration = 1.0f;

    [SerializeField] float breakingForce = 1.0f;    //  Slow down without input 

    Vector3 speed;

    private void Start()
    {
        speed = new Vector3();
    }

    void Update()
    {
        float x = Input.GetAxis("Sideways");  //  Getting input
        float y = Input.GetAxis("Vertical");
        float z = Input.GetAxis("Forward");

        speed = transform.right * x + transform.up * y + transform.forward * z; //  Calculating horizontal movement


        this.transform.position += speed * Time.deltaTime;
    }
}
