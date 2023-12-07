using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CaptureInsectController : MonoBehaviour
{
    public float volumeThreshold = 1.0f;
    public int waveLength = 256;
    public float offsetAngle = 15;

    public Transform parent;

    public Image progressBar;
    public Image checkmark;
    public Image cross;

    private AudioClip waveAudio;
    private bool isCaptured = false;
    private float soundPercentage = 0.0f;
    private float limit1;
    private float limit2;

    // Start is called before the first frame update
    void Start()
    {
        StartMicrophone();
        limit1 = 90 - offsetAngle;
        limit2 = 270 + offsetAngle;
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
        float y = parent.localRotation.eulerAngles.y;

        if (y < limit1 || y > limit2)
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

        Debug.Log("Y Rotation = " + y);
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
        float y = parent.localRotation.eulerAngles.y;

        if (y < limit1 || y > limit2)
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
