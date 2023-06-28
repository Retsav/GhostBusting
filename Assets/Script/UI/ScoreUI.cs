using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreUI : MonoBehaviour
{
    [SerializeField] private TMP_Text textMeshProText;


    private void Update()
    {
        textMeshProText.text = ScoreController.Instance.GetScore().ToString();
    }
}
