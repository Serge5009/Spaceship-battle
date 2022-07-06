using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ship : MonoBehaviour
{
    public int teamID;

    public GameObject projectilePrefab;
    [SerializeField] float fireRate = 1.0f; //  Seconds between each shot
    Vector3 aimDirection;
    float shotingCountdown;

    //  Movement
    public float acceleration = 2.0f;
    public float maxSpeed = 20.0f;
    [HideInInspector] public Vector3 velocity;

    //  Stats
    public float health = 500.0f;

    void Start()
    {
        GameManager.gameManager.teams[teamID].ships.Add(this);  //  Assigns this ship to specified team

        velocity = new Vector3(0.0f, 0.0f, 0.0f);

        shotingCountdown = fireRate;
    }

    void Update()
    {
        Movement();
        Shooting();
    }



    void Movement()
    {
        velocity += new Vector3(Random.Range(-acceleration, acceleration), Random.Range(-acceleration, acceleration), Random.Range(-acceleration, acceleration)) * Time.deltaTime;
        
        if (velocity.magnitude > maxSpeed)  //  Limiting speed
            velocity = velocity.normalized * maxSpeed;

        transform.position += velocity * Time.deltaTime;
    }


    void Shooting()
    {
        shotingCountdown -= Time.deltaTime;

        if(shotingCountdown <= 0 && GameManager.gameManager.numTeams > 1)   //  Will only shoot when reloaded and has an enemy
        {
            shotingCountdown = fireRate;
            FindTarget();
            if (aimDirection == Vector3.zero)
                Debug.LogWarning("Aiming vector resulted to 0, 0, 0");
            Shoot();
        }
    }

    void FindTarget()
    {
        float minDist = Mathf.Infinity;
        foreach (Team t in GameManager.gameManager.teams)   //  Looping thru all teams
        {
            if (t.teamID == teamID) //  Skip allied targets
                continue;

            foreach (Ship s in t.ships) //  Looping thru each team ships
            {
                float dist = Vector3.Distance(transform.position, s.transform.position);  //  Find the distance
                if (dist < minDist)
                {
                    minDist = dist;
                    aimDirection = s.transform.position - transform.position;
                }
            }
        }
    }

    void Shoot()
    {
        GameObject proj = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
        Projectile p = proj.GetComponent<Projectile>();
        Vector3 shootVec = aimDirection.normalized * p.speed + velocity;    //  Applying speed and adding momentum
        p.SetSpeed(shootVec);
    }

    public void GetDamage(float dmg)
    {
        health -= dmg;

        //Debug.Log("Damaged");

        if(health <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        GameManager.gameManager.teams[teamID].ships.Remove(this);
        //Debug.Log("Ship destroyed");

        Destroy(gameObject);
    }
}
