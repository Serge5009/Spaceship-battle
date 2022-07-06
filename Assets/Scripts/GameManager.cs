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


    int numTeams;
    public List<Team> teams;


    void Start()
    {
        Debug.Log("There are " + teams.Count + " teams");
        numTeams = teams.Count;
    }

    void Update()
    {
        if(numTeams == 1)
        {
            Debug.Log("Game ended");
        }
    }
}
