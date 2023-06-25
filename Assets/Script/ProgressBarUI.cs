using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBarUI : MonoBehaviour
{
    [SerializeField] private GameObject hasProgressGameObject;
    [SerializeField] private Image barImage;
    [SerializeField] private Image backgroundImage;

    private IHasProgress hasProgress;

    private void Start()
    {
        hasProgress = hasProgressGameObject.GetComponent<IHasProgress>();
        if (hasProgress == null)
        {
            Debug.LogError("Przypisano zly GameObject do hasProgressGameObject. Musi miec komponent IHasProgress.");
        }
        hasProgress.OnProgressChanged += HasProgress_OnProgressChanged;
        barImage.fillAmount = 0f;
        Hide();
        OccultTable.Instance.OnFinishedExorcising += Instance_OnFinishedExorcising;
        OccultTable.Instance.OnStateChanged += Instance_OnStateChanged;
        OccultTable.Instance.OnSuccessfulFinishedExorcising += Instance_OnSuccessfulFinishedExorcising;
    }

    private void Instance_OnSuccessfulFinishedExorcising(object sender, System.EventArgs e)
    {
        Hide();
    }

    private void Instance_OnStateChanged(object sender, OccultTable.OnStateChangedEventArgs e)
    {
        if(e.state == OccultTable.State.Exorcising)
        {
            Show();
        }
    }

    private void Instance_OnFinishedExorcising(object sender, System.EventArgs e)
    {
        Hide();
    }

    private void HasProgress_OnProgressChanged(object sender, IHasProgress.OnProgressChangedEventArgs e)
    {
        barImage.fillAmount = e.progressNormalized;
        if (e.progressNormalized == 0f || e.progressNormalized == 1f)
        {
            Hide();
        }
        else
        {
            Show();
        }
    }

    private void Show()
    {
        gameObject.SetActive(true);
    }

    private void Hide()
    {
        gameObject.SetActive(false);
    }
}
