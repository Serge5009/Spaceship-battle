using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;
    public static GameManager gameManager   //  Singleton for Game Manager
    {
        get
        {
            _instance = GameObject.FindObjectOfType<GameManager>();
            if (_instance == null)
            {
                Debug.LogError("No Game Manager found!");
            }
            DontDestroyOnLoad(_instance.gameObject);
            return _instance;
        }
    }


    public int numTeams;
    public List<Team> teams;

    bool isBattle = true;


    void Start()
    {
        numTeams = teams.Count;
        Debug.Log("There are " + numTeams + " teams");
    }

    void Update()
    { 
        if(numTeams == 1 && isBattle)
        {
            Debug.Log("Game ended");
            isBattle = false;

            int winID = 0;
            foreach(Team t in teams)
            {
                if (t.numShips > 0)
                {
                    Debug.Log("Team " + winID + " is a winner!");
                }
                winID++;
            }
        }
    }

    public void OnTeamLost()
    {
        numTeams--;
    }
}
