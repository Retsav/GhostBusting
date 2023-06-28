using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DifficultyManager : MonoBehaviour
{
    [SerializeField] private AnimationCurve difficultyCurve;
    private float difficultyPointMax = 15f;

    public float GetSpeedCurve()
    {
        var point = ScoreController.Instance.GetScore() / difficultyPointMax;
        Debug.Log("Mod. trudnosci + " + point);
        var speedIntersection = difficultyCurve.Evaluate(point);
        return speedIntersection;
    }
}
