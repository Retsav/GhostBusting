using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CinemachineController : MonoBehaviour
{
    private CinemachineVirtualCamera cinemachineVirtualCamera;
    [SerializeField] private Transform lookAtOccultTableGameObject;
    [SerializeField] private Transform lookAtDeafultLookingPointGameObject;
    private float defaultFOV;
    private float changeFOV = 40f;
    private float shakeTimer;
    private float shakeTimerTotal;
    private float startingIntensity;

    private void Awake()
    {
        cinemachineVirtualCamera = GetComponent<CinemachineVirtualCamera>();
        defaultFOV = cinemachineVirtualCamera.m_Lens.FieldOfView;
    }

    private void Start()
    {
        OccultTable.Instance.OnStartedExorcising += Instance_OnStartedExorcising;
        OccultTable.Instance.OnFinishedExorcising += Instance_OnFinishedExorcising;
        OccultTable.Instance.OnSuccessfulFinishedExorcising += Instance_OnSuccessfulFinishedExorcising;
    }

    private void Instance_OnSuccessfulFinishedExorcising(object sender, System.EventArgs e)
    {
        LookAt(lookAtDeafultLookingPointGameObject);
        ChangeFOV(changeFOV, defaultFOV, 1.5f);
    }

    private void Instance_OnFinishedExorcising(object sender, System.EventArgs e)
    {
        LookAt(lookAtDeafultLookingPointGameObject);
        ChangeFOV(changeFOV, defaultFOV, 1.5f);
    }

    private void Instance_OnStartedExorcising(object sender, System.EventArgs e)
    {
        ShakeCamera(2f, 1f);
        LookAt(lookAtOccultTableGameObject);
        ChangeFOV(defaultFOV, changeFOV, 1);
    }

    private void ChangeFOV(float defaultFOV, float changeFOV, float time)
    {
        cinemachineVirtualCamera.m_Lens.FieldOfView = Mathf.Lerp(defaultFOV, changeFOV, time);
    }
    private void LookAt(Transform objectToLook)
    {
        cinemachineVirtualCamera.LookAt = objectToLook;
    }

    public void ShakeCamera(float intensity, float time)
    {
        CinemachineBasicMultiChannelPerlin cinemachineBasicMultiChannelPerlin =
            cinemachineVirtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = intensity;
        startingIntensity = intensity;
        shakeTimer = time;
        shakeTimerTotal = time;
    }

    private void Update()
    {
        if(shakeTimer > 0)
        {
            shakeTimer -= Time.deltaTime;
            if(shakeTimer <= 0)
            {
                CinemachineBasicMultiChannelPerlin cinemachineBasicMultiChannelPerlin =
                    cinemachineVirtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
                cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = 
                    Mathf.Lerp(startingIntensity, 0f, 1 - (shakeTimer / shakeTimerTotal));
            }
        }
    }
}
