using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    Vector3 velocity;

    void Start()
    {
        velocity = new Vector3(0.0f, 0.0f, 0.0f);
    }

    void Update()
    {
        transform.position += velocity * Time.deltaTime;
    }

    public void SetSpeed(Vector3 newSpeed)
    {
        velocity = newSpeed;
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Projectile collision");
        Destroy(gameObject);
    }
}
