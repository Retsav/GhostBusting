using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OccultTableVisual : MonoBehaviour
{
    [SerializeField] private GameObject PentagramOnGameObject;
    [SerializeField] private GameObject PentagramOffGameObject;
    private void Start()
    {
        OccultTable.Instance.OnStateChanged += Instance_OnStateChanged;
    }

    private void Instance_OnStateChanged(object sender, OccultTable.OnStateChangedEventArgs e)
    {
        bool showVisual = e.state == OccultTable.State.Exorcising;
        PentagramOnGameObject.SetActive(showVisual);
        PentagramOffGameObject.SetActive(!showVisual);
    }
}
