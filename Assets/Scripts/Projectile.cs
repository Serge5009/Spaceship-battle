using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    //  Stats
    [SerializeField] float damage = 5.0f;
    public float speed = 20.0f;
    [SerializeField] float lifespan = 5.0f;    //  In seconds

    Vector3 velocity;


    void Start()
    {

    }

    void Update()
    {
        transform.position += velocity * Time.deltaTime;
        lifespan -= Time.deltaTime;
        if (lifespan <= 0)
            Destroy(gameObject);
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
