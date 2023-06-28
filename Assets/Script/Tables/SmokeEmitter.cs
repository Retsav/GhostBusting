using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmokeEmitter : MonoBehaviour
{
    [SerializeField] private OccultTable occultTable;
    [SerializeField] private GameObject particleObject;

    ParticleSystem particleSystemVar;

    private void Awake()
    {
        particleSystemVar = particleObject.GetComponent<ParticleSystem>();
    }

    private void Start()
    {
        occultTable.OnSuccessfulFinishedExorcising += OccultTable_OnSuccessfulFinishedExorcising;
        occultTable.OnFinishedExorcising += OccultTable_OnFinishedExorcising;
    }

    private void OccultTable_OnFinishedExorcising(object sender, System.EventArgs e)
    {
        ChangeColor(Color.red);
        EmitParticles();
    }

    private void OccultTable_OnSuccessfulFinishedExorcising(object sender, System.EventArgs e)
    {
        ChangeColor(Color.green);
        EmitParticles();
    }


    private void EmitParticles()
    {
        particleSystemVar.Play();
    }

    private void ChangeColor(Color color)
    {
        ParticleSystem.MainModule mps = particleSystemVar.main;
        mps.startColor = color;
    }
}
