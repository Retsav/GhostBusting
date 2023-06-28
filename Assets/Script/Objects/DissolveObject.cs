using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class DissolveObject : MonoBehaviour
{
    private Transform dissolvedObject;
    private float dissolveTimer;
    private float dissolveTimerMax = 1f;
    private void Start()
    {
        PickableObject.Instance.OnExorcisedObject += Instance_OnExorcisedObject;
    }

    private void Instance_OnExorcisedObject(object sender, PickableObject.OnExorcisedObjectEventArgs e)
    {
        Debug.Log("Created object!");
        Transform dissolvedObject = Instantiate(e.pickableObjectSO.exorcisedPrefab, transform);
        if(dissolveTimer < dissolveTimerMax)
        {
            dissolveTimer += Time.deltaTime;
            dissolvedObject.gameObject.GetComponent<Material>().SetFloat("_Dissolve", dissolveTimer);
        } else
        {
            Destroy(dissolvedObject);
            dissolveTimer = 0f;
        }
    }

    public void TryDissolveObject(PickableObjectSO pickableObjectSO)
    {
        Transform dissolvedObject = Instantiate(pickableObjectSO.exorcisedPrefab, transform);
        if (dissolveTimer < dissolveTimerMax)
        {
            dissolveTimer += Time.deltaTime;
            dissolvedObject.gameObject.GetComponent<Material>().SetFloat("_Dissolve", dissolveTimer);
        }
        else
        {
            Destroy(dissolvedObject);
            dissolveTimer = 0f;
        }
    }
}