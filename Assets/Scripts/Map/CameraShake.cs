using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraShake : MonoBehaviour
{
    public static CameraShake Instance { get; private set; }

    private CinemachineVirtualCamera vCam;

    void Awake()
    {
        Instance = this;
        vCam = GetComponent<CinemachineVirtualCamera>();
    }

    public void ShakeCamera(float shakeIntensity, float shakeTime)
    {
        // StopAllCoroutines();
        StartCoroutine(shake(shakeIntensity, shakeTime));
    }

    IEnumerator shake(float shakeIntensity, float shakeTime) {
        CinemachineBasicMultiChannelPerlin vCamPerlin = vCam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

        vCamPerlin.m_AmplitudeGain = shakeIntensity;
        yield return new WaitForSeconds(shakeTime);
        vCamPerlin.m_AmplitudeGain = 0;
    }   
}
