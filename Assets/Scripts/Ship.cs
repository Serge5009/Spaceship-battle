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

    //  Behavior
    GameObject target;
    float distanceToTarget;
    [SerializeField] bool aimWithPrediction = true;

    [SerializeField] float preferredDistance = 20.0f;
    float basePreferred = 0.0f;
    [SerializeField] float preferredGap = 5.0f;

    //  Movement
    public float acceleration = 2.0f;
    public float maxSpeed = 20.0f;
    [HideInInspector] public Vector3 velocity;

    //  Stats
    public float health = 500.0f;
    float healthInnitial = 0.0f;
    bool isAlive = true;

    //  Player controls
    [HideInInspector] public bool isPlayer = false;

    void Start()
    {
        allies = GameManager.gameManager.teams[teamID];         //  Remember your team
        allies.ships.Add(this);                                 //  Assigns this ship to specified team
        allies.numShips++;                                      //  Increase counter for the team
        this.gameObject.transform.SetParent(allies.transform);  //  Set the ship as team's child

        //  TO DO       add new team automatically when there's none

        RandomizeStats();

        healthInnitial = health;
        basePreferred = preferredDistance;

        velocity = new Vector3(0.0f, 0.0f, 0.0f);

        shotingCountdown = fireRate;

        FindTarget();
    }

    void RandomizeStats()   //  This function is needed to desync ships stats and make game more dynamic
    {
        //  All values are randomly shifted within +-5% range
        preferredDistance += Random.Range(preferredDistance * -0.05f, preferredDistance * 0.05f);
        preferredGap += Random.Range(preferredGap * -0.05f, preferredGap * 0.05f);
        fireRate += Random.Range(fireRate * -0.05f, fireRate * 0.05f);
        maxSpeed += Random.Range(maxSpeed * -0.05f, maxSpeed * 0.05f);
        acceleration += Random.Range(acceleration * -0.05f, acceleration * 0.05f);
        fireRate += Random.Range(fireRate * -0.05f, fireRate * 0.05f);
        targetUpdateTimer += Random.Range(targetUpdateTimer * -0.05f, targetUpdateTimer * 0.05f);
    }

    float shipUpdateTimer = 0.0f;
    float targetUpdateTimer = 1.0f;
    void Update()
    {
        try
        {
            Movement();
            if(target && GameManager.gameManager.isBattle)
            {
                Shooting();
            }
        }
        catch   //  Needed cuz sometimes ship fail to find a target on the first frame
        {
            Debug.LogWarning("Ship scipped an update due to an unknown bug");
        }



        shipUpdateTimer += Time.deltaTime;
        if(shipUpdateTimer >= targetUpdateTimer && GameManager.gameManager.isBattle)
        {
            FindTarget();
            FindPrefferedDistance();
            shipUpdateTimer = 0.0f;
        }

        Debug.DrawRay(transform.position, aimDirection, Color.red);
        Debug.DrawRay(transform.position, velocity, Color.blue);


    }



    void Movement()
    {
        //Wander();
        if(distanceToTarget > preferredDistance + preferredGap && target)
        {
            MoveTowards(target);
        }
        else if (distanceToTarget < preferredDistance - preferredGap && target)
        {
            MoveFrom(target);
        }
        else
        {
            Wander();
        }

        velocity -= velocity / 60 * Time.deltaTime;                 //  Ships slow down like there's air friction (I know there's no air in space)

        velocity -= transform.position / 180.0f * Time.deltaTime;    //  All ships are slightly attracted to the world center

        if (velocity.magnitude > maxSpeed)                          //  Limiting speed
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

    void FindPrefferedDistance()
    {
        preferredDistance = basePreferred + (healthInnitial - health) * 0.1f;    //  Ship will try to stay further if looses hp

        if (preferredDistance < preferredGap + 1.0f)    //  Shouldn't bee too close
            preferredDistance = preferredGap + 1.0f;

        basePreferred -= preferredDistance / 120;    //  The function is called once every second
                                            //  this means that ships will tend to get closer with time (1m / 10sec)
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
        else
        {
            distanceToTarget = Vector3.Distance(transform.position, target.transform.position);
        }
    }

    void AimAtTarget()
    {
        if(aimWithPrediction)   //  Seems to be not perfect)
        {
            float bulletSpeed = projectilePrefab.GetComponent<Projectile>().speed;
            float disranceToTar = (target.transform.position - transform.position).magnitude;
            float bulletTime = disranceToTar / bulletSpeed;

            Vector3 targetPos = target.transform.position;
            Vector3 targetSpeed = target.GetComponent<Ship>().velocity;
            Vector3 newTargetPos = targetPos + targetSpeed * bulletTime;

            //Debug.Log("Bullet speed " + bulletSpeed + "; time " + bulletTime + "; distance " + disranceToTar + ";\naim to " + newTargetPos);

            aimDirection = newTargetPos - transform.position;
        }
        else
        {
            aimDirection = target.transform.position - transform.position;
        }
    }

    void Shoot()
    {
        GameObject proj = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
        Projectile p = proj.GetComponent<Projectile>();
        if(isPlayer)
        {
            Vector3 shootVec = aimDirection.normalized * p.speed + velocity;    //  Applying speed and adding momentum
            p.SetSpeed(shootVec);
        }
        else
        {
            Vector3 shootVec = aimDirection.normalized * p.speed;    //  No momentum used for AI 
            p.SetSpeed(shootVec);
        }
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
