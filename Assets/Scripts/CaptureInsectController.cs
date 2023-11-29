using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CaptureInsectController : MonoBehaviour
{
    public float volumeThreshold = 1.0f;
    public int waveLength = 256;
    public float behindDistance = -3;

    public Transform camera;

    private AudioClip waveAudio;

    // Start is called before the first frame update
    void Start()
    {
        StartMicrophone();
    }

    // Update is called once per frame
    void Update()
    {
        if (camera.position.z < behindDistance)
        {
            Debug.Log("Behind");

            float volume = GetVolume(Microphone.GetPosition(Microphone.devices[0]), waveAudio);

            if (volume > volumeThreshold)
            {
                Debug.Log("Captured Volume = " + volume);
            }
        }
    }

    void StartMicrophone()
    {
        string micName = Microphone.devices[0];
        waveAudio = Microphone.Start(micName, true, 20, AudioSettings.outputSampleRate);
    }

    float GetVolume(int clipPosition, AudioClip clip)
    {
        int startingPosition = clipPosition - waveLength;

        if (startingPosition < 0)
        {
            return 0.0f;
        }

        float[] waveData = new float[waveLength];
        clip.GetData(waveData, startingPosition);

        float volumeTotal = 0;

        foreach (var wave in waveData)
        {
            volumeTotal += Mathf.Abs(wave);
        }

        return volumeTotal;
    }
}
