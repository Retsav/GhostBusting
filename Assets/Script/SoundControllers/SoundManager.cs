using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }
    [SerializeField] private AudioClipRefsSO audioClipRefsSO;
    [SerializeField] private OccultTable occultTable;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        PlayerController.Instance.OnPickedSomething += Instance_OnPickedSomething;
        BaseTable.OnAnyObjectPlacedHere += BaseTable_OnAnyObjectPlacedHere;
        occultTable.OnSuccessfulFinishedExorcising += OccultTable_OnSuccessfulFinishedExorcising;
        occultTable.OnFinishedExorcising += OccultTable_OnFinishedExorcising;
        occultTable.OnStartedExorcising += OccultTable_OnStartedExorcising;
        occultTable.OnCorrectButtonPressed += OccultTable_OnCorrectButtonPressed;
    }

    private void OccultTable_OnCorrectButtonPressed(object sender, OccultTable.OnCorrectButtonPressedEventArgs e)
    {
        PlaySound(audioClipRefsSO.buttonClicks, occultTable.transform.position);
    }

    private void OccultTable_OnStartedExorcising(object sender, System.EventArgs e)
    {
        PlaySound(audioClipRefsSO.evilLaugh, occultTable.transform.position);
    }

    private void OccultTable_OnFinishedExorcising(object sender, System.EventArgs e)
    {
        PlaySound(audioClipRefsSO.particleExplosion, occultTable.transform.position);
        PlaySound(audioClipRefsSO.qteFail, occultTable.transform.position);
    }

    private void OccultTable_OnSuccessfulFinishedExorcising(object sender, System.EventArgs e)
    {
        PlaySound(audioClipRefsSO.particleExplosion, occultTable.transform.position);
        PlaySound(audioClipRefsSO.qteSuccess, occultTable.transform.position);
    }

    private void BaseTable_OnAnyObjectPlacedHere(object sender, System.EventArgs e)
    {
        BaseTable baseTable = sender as BaseTable;
        PlaySound(audioClipRefsSO.objectDrop, baseTable.transform.position);
    }

    private void Instance_OnPickedSomething(object sender, System.EventArgs e)
    {
        PlaySound(audioClipRefsSO.objectPickup, PlayerController.Instance.transform.position);
    }

    public void PlayFootstepsSound(Vector3 position, float volume)
    {
        PlaySound(audioClipRefsSO.footsteps, position, volume);
    }

    private void PlaySound(AudioClip[] audioClipArray, Vector3 position, float volume = 1f)
    {
        PlaySound(audioClipArray[Random.Range(0, audioClipArray.Length)], position, volume);
    }

    private void PlaySound(AudioClip audioClip, Vector3 position, float volume = 1f)
    {
        AudioSource.PlayClipAtPoint(audioClip, position, volume);
    }
}
