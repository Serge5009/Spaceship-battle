using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ship : MonoBehaviour
{
    public int teamID;

    //  Placeholders:
    public float hp;
    public float damage;

    void Start()
    {
        GameManager.gameManager.teams[teamID].ships.Add(this);  //  Assigns this ship to specified team

        velocity = new Vector3(0.0f, 0.0f, 0.0f);
    }

    void Update()
    {
        Movement();
    }



    //public float speed;
    Vector3 velocity;
    void Movement()
    {
        velocity += new Vector3(Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f)) * Time.deltaTime;
        transform.position += velocity * Time.deltaTime;
    }
}
