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
        var speedIntersection = difficultyCurve.Evaluate(point);
        return speedIntersection;
    }
}
