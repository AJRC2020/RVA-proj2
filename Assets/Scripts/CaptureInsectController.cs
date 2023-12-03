using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CaptureInsectController : MonoBehaviour
{
    public float volumeThreshold = 1.0f;
    public int waveLength = 256;
    public float behindDistance = -3;

    public Transform camera;

    public Image progressBar;
    public Image checkmark;
    public Image cross;

    private AudioClip waveAudio;
    private bool isCaptured = false;
    private float soundPercentage = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        StartMicrophone();
    }

    // Update is called once per frame
    void Update()
    {
        if (!isCaptured)
        {
            Capture();
            UpdateUI();
        }
    }

    void Capture()
    {
        if (camera.position.z < behindDistance)
        {
            Debug.Log("Behind");

            float volume = GetVolume(Microphone.GetPosition(Microphone.devices[0]), waveAudio);

            soundPercentage = Mathf.Clamp01(volume / volumeThreshold);

            if (volume > volumeThreshold)
            {
                Debug.Log("Captured Volume = " + volume);
                isCaptured = true;
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

    void UpdateUI()
    {
        if (camera.position.z < behindDistance)
        {
            checkmark.enabled = true;
            cross.enabled = false;
            progressBar.fillAmount = soundPercentage;
            progressBar.color = Color.Lerp(Color.green, Color.red, soundPercentage);
        }
        else
        {
            checkmark.enabled = false;
            cross.enabled = true;
            progressBar.fillAmount = 0.0f;
        }
    }
}
