using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectedTableVisual : MonoBehaviour
{
    [SerializeField] private BaseTable baseTable;
    [SerializeField] private GameObject[] visualGameObjectArray;
    private void Start()
    {
        PlayerController.Instance.OnSelectedTableChanged += PlayerController_OnSelectedTableChanged;
    }

    private void PlayerController_OnSelectedTableChanged(object sender, PlayerController.OnSelectedTableChangedEventArgs e)
    {
        if (e.selectedTable == baseTable)
        {
            Show();
        }
        else
        {
            Hide();
        }
    }

    private void Show()
    {
        foreach (GameObject visualGameObject in visualGameObjectArray)
        {
            visualGameObject.SetActive(true);
        }
    }

    private void Hide()
    {
        foreach (GameObject visualGameObject in visualGameObjectArray)
        {
            visualGameObject.SetActive(false);
        }
    }
}
