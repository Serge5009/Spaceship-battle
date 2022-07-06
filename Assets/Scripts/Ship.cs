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
    }

    void Update()
    {
        
    }
}
