using UnityEngine;
using Unity.Cinemachine;

public class Camera : MonoBehaviour
{
    public CinemachineCamera camera;
    public float shakeTime;
    public static Camera instance;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (instance == null)
        {
            instance = this;
        } else
        {
            Destroy(this);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (shakeTime > 0)
        {
            shakeTime -= Time.deltaTime;

            if (shakeTime <= 0 )
            {
                CinemachineBasicMultiChannelPerlin perlin = camera.GetComponent<CinemachineBasicMultiChannelPerlin>();
                perlin.AmplitudeGain = 0;
            }
        }
    }

    public void Shake(float intensity, float duration)
    {
        shakeTime = duration;
        CinemachineBasicMultiChannelPerlin perlin = camera.GetComponent<CinemachineBasicMultiChannelPerlin>();
        perlin.AmplitudeGain = intensity;
    }
}
