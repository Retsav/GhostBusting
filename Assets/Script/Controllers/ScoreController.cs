using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreController : MonoBehaviour
{
    public static ScoreController Instance { get; private set; }
    private void Awake()
    {
        Instance = this;
    }


    private int score;

    public void ScoreIncrease() {
        score++;    
    }

    public void ScoreDecrease() {
        if (score <= 0) return;
        score--;
    }

    public int GetScore()
    {
        return score;
    }
}
