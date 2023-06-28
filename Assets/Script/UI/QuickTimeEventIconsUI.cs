using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class QuickTimeEventIconsUI : MonoBehaviour
{
    [SerializeField] private Transform iconTemplate;
    private PickableObjectSO pickableObjectSO;


    private void Start()
    {
        OccultTable.Instance.OnSelectedTableGiven += OccultTable_OnSelectedTableGiven;
        OccultTable.Instance.OnCorrectButtonPressed += Instance_OnCorrectButtonPressed;
        OccultTable.Instance.OnObjectDropped += Instance_OnObjectDropped;
        OccultTable.Instance.OnObjectPicked += Instance_OnObjectPicked;
        OccultTable.Instance.OnFinishedExorcising += Instance_OnFinishedExorcising;
    }


    private void Instance_OnFinishedExorcising(object sender, System.EventArgs e)
    {
        foreach (Transform child in transform)
        {
            if (child == iconTemplate) continue;
            Destroy(child.gameObject);
        }
    }

    private void Instance_OnCorrectButtonPressed(object sender, OccultTable.OnCorrectButtonPressedEventArgs e)
    {
        foreach (Transform child in transform)
        {
            if (child.GetChild(1).GetComponent<Image>().sprite == e.qteSO.keySprite)
            {
                child.GetChild(2).GetComponent<Image>().DOFade(1f, 0.2f).OnComplete(() =>
                {
                   Destroy(child.gameObject);
                });
            }
        }
    }

    private void Instance_OnObjectPicked(object sender, System.EventArgs e)
    {
        foreach (Transform child in transform)
        {
            if (child == iconTemplate) continue;
            Destroy(child.gameObject);
        }
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
