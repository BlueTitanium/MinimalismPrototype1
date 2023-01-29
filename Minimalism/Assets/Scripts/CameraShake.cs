using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public static CameraShake cs;
    public CinemachineVirtualCamera cam;
    public float shakeLeft = 0;
    // Start is called before the first frame update
    void Start()
    {
        cs = this;
    }

    // Update is called once per frame
    void Update()
    {
        if(shakeLeft > 0)
        {
            shakeLeft -= Time.deltaTime;
        } else
        {
            cam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_AmplitudeGain = 0;
        }
    }

    public void cameraShake(float time, float amplitude)
    {
        cam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_AmplitudeGain = amplitude;
        shakeLeft = time;
    }
}
