using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Vuforia;

public class CaptureController : MonoBehaviour
{
    public float luminosityThreshold = 0.2f;
    public float blackThreshold = 0.1f;
    public RawImage cameraFeed;

    private Camera cam;
    private RenderTexture rt;
    private bool isCaptured = false;

    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;
        rt = new RenderTexture(Screen.width, Screen.height, 24);
        cam.targetTexture = rt;
        cameraFeed.texture = rt;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isCaptured)
        {
            CaptureLogic();
        }
    }

    private void CaptureLogic()
    {
        cam.Render();

        Texture2D tex = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
        RenderTexture.active = rt;
        tex.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
        tex.Apply();

        cameraFeed.texture = tex;

        float luminosityLevel = GetLuminosityLevels(tex.GetPixels());

        if (luminosityLevel < luminosityThreshold)
        {
            luminosityLevel = luminosityThreshold;
        }

        float luminosityPercentage = (1 / (luminosityThreshold - 1)) * luminosityLevel - (1 / (luminosityThreshold - 1));

        if (luminosityPercentage == 1)
        {
            isCaptured = true;
        }

        Debug.Log("Luminosity Percentage = " + luminosityPercentage * 100 + "%    Luminosity Level = " + luminosityLevel);
    }

    private float GetLuminosityLevels(Color[] pixels)
    {
        float totalLuminosity = 0;
        int pixelCount = 0;

        foreach (Color pixel in pixels)
        {
            if (pixel.grayscale > blackThreshold)
            {
                float linearLuminosity = pixel.grayscale;

                totalLuminosity += linearLuminosity;
                pixelCount++;
            }
        }

        float averagedLuminosity = totalLuminosity / pixelCount;

        return averagedLuminosity;
    }
}
