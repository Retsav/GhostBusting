using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuickTimeEventSingleIconUI : MonoBehaviour
{
    [SerializeField] private Image image;

    public void SetQTEObjectSO(QTE_SO qteObjectSO)
    {
        image.sprite = qteObjectSO.keySprite;
        Debug.Log("Ustawiam sprite na " + qteObjectSO.keySprite);
    }
}
