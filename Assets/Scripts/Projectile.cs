using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    //  Stats
    [SerializeField] float damage = 5.0f;   
    public float speed = 20.0f;             //  Speed is grabbed by ship when spawning the bullet and not used anymore
    [SerializeField] float lifespan = 5.0f; //  In seconds
    float originalLifespan;

    Vector3 velocity;


    void Start()
    {
        originalLifespan = lifespan;
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

    
    private void OnTriggerEnter(Collider other)
    {
        if (originalLifespan - lifespan <= 0.2f)    //  preventing early collision
            return;

 
        try
        {
            Ship target = other.gameObject.GetComponent<Ship>();

            target.GetDamage(damage);
        }
        catch
        {
            Debug.LogWarning("Bulled failed to find a target");
        }

        Destroy(gameObject);
    }
}
