using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectParticleSystem : MonoBehaviour
{
    [SerializeField] private PickableObject pickableObject;
    [SerializeField] private GameObject particleSystem;


    private void Update()
    {
        if(pickableObject.IsHunted())
        {
            particleSystem.SetActive(true);
        } else
        {
            particleSystem.SetActive(false);
        }
    }

}
