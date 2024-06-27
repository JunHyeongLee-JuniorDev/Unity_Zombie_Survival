using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }

        else
        {
            Destroy(gameObject);
        }    
    }

    public int Score = 0;
    public bool isGameover { get; private set; }

    private void Start()
    {
        //FindObjectOfType<PlayerHealth>().OnDead +=
    }

    private void EndGame()
    {
        isGameover = true;
        // UI Update
        //UIController.
    }

    private void AddScore(int newScore)
    {
        if(!isGameover)
        {
            Score += newScore;
            //UI Update
            UIController.
        }
    }

}
