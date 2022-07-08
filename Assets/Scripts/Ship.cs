using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ship : MonoBehaviour
{
    public int teamID;
    Team allies;

    //  Shooting
    public GameObject projectilePrefab;
    [SerializeField] float fireRate = 1.0f; //  Seconds between each shot
    float shotingCountdown;
    Vector3 aimDirection;

    //  Enemy
    GameObject target;

    //  Movement
    public float acceleration = 2.0f;
    public float maxSpeed = 20.0f;
    [HideInInspector] public Vector3 velocity;

    //  Stats
    public float health = 500.0f;
    bool isAlive = true;

    void Start()
    {
        allies = GameManager.gameManager.teams[teamID];         //  Remember your team
        allies.ships.Add(this);                                 //  Assigns this ship to specified team
        allies.numShips++;                                      //  Increase counter for the team
        this.gameObject.transform.SetParent(allies.transform);  //  Set the ship as team's child

        //  TO DO       add new team automatically when there's none

        velocity = new Vector3(0.0f, 0.0f, 0.0f);

        shotingCountdown = fireRate;

        FindTarget();
    }

    float targetUpdateTimer = 0.0f;
    void Update()
    {
        try
        {
            Movement();
            Shooting();
        }
        catch   //  Needed cuz sometimes ship fail to find a target on the first frame
        {
            Debug.LogWarning("Ship scipped an update due to an unknown bug");
        }

        targetUpdateTimer += Time.deltaTime;
        if(targetUpdateTimer >= 0.0f)
        {
            FindTarget();
            targetUpdateTimer = 0.0f;
        }
    }



    void Movement()
    {
        //Wander();

        if (health > 100)
            MoveTowards(target);
        else
            MoveFrom(target);

        if (velocity.magnitude > maxSpeed)  //  Limiting speed
            velocity = velocity.normalized * maxSpeed;

        transform.position += velocity * Time.deltaTime;
    }

    void Wander()
    {
        velocity += new Vector3(Random.Range(-acceleration, acceleration), Random.Range(-acceleration, acceleration), Random.Range(-acceleration, acceleration)) * Time.deltaTime;



    }

    void MoveTowards(GameObject targ)
    {
        Vector3 dir = targ.transform.position - transform.position;

        velocity += dir.normalized * acceleration * Time.deltaTime;
    }

    void MoveFrom(GameObject targ)
    {
        Vector3 dir = - targ.transform.position + transform.position;

        velocity += dir.normalized * acceleration * Time.deltaTime;
    }



    void Shooting()
    {
        shotingCountdown -= Time.deltaTime;

        if(shotingCountdown <= 0 && GameManager.gameManager.numTeams > 1)   //  Will only shoot when reloaded and has an enemy
        {
            shotingCountdown = fireRate;
            AimAtTarget();
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
                    target = s.gameObject;
                }
            }
        }
        if (!target)
            Debug.LogWarning("Ship couldn't find a target");
    }

    void AimAtTarget()
    {
        aimDirection = target.transform.position - transform.position;
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

        if(health <= 0 && isAlive)
        {
            Die();
        }
    }

    void Die()
    {
        allies.ships.Remove(this);
        //Debug.Log("Ship destroyed");
        allies.numShips--;
        isAlive = false;

        Destroy(gameObject);
    }
}
