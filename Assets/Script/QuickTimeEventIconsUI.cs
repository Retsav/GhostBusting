using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuickTimeEventIconsUI : MonoBehaviour
{
    [SerializeField] private Transform iconTemplate;
    private PickableObjectSO pickableObjectSO;


    private void Start()
    {
        OccultTable.Instance.OnSelectedTableGiven += OccultTable_OnSelectedTableGiven;
        OccultTable.Instance.OnObjectDropped += Instance_OnObjectDropped;
    }

    private void Awake()
    {
        iconTemplate.gameObject.SetActive(false);
    }

    private void Instance_OnObjectDropped(object sender, System.EventArgs e)
    {
        GiveQTEVisual();
    }

    private void OccultTable_OnSelectedTableGiven(object sender, OccultTable.OnPickableTableEventArgs e)
    {
        this.pickableObjectSO = e.pickableObjectSO;
    }

    private void GiveQTEVisual()
    {
        foreach (Transform child in transform)
        {
            if (child == iconTemplate) continue;
            Destroy(child.gameObject);
        }
        foreach(QTE_SO qteSO in pickableObjectSO.qteList.quickTimeEventActionList)
        {
            Transform iconTransform = Instantiate(iconTemplate, transform);
            iconTransform.gameObject.SetActive(true);
            iconTransform.GetComponent<QuickTimeEventSingleIconUI>().SetQTEObjectSO(qteSO);
        }
    }
}
