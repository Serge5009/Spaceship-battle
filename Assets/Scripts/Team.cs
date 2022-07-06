using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Team : MonoBehaviour
{
    public int teamID = -1;

    int numShips;
    public List<Ship> ships;

    private void Awake()
    {
        GameManager.gameManager.teams.Add(this);
    }

    void Start()
    {

        //  This code assigns an ID for the team according to it's ID in 
        int idForTeam = 0;
        foreach (Team t in GameManager.gameManager.teams)
        {
            if (t == this)
                teamID = idForTeam; 
            idForTeam++;
        }
        if (teamID == -1)
            Debug.LogError("Failed to find an ID for this team");
    }

    void Update()
    {
        
    }
}
