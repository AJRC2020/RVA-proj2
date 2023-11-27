using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CaptureController : MonoBehaviour
{
    public float luminosityThreshold = 0.2f;
    public float blackThreshold = 0.1f;

    private Camera cam;
    private RenderTexture rt;
    // Start is called before the first frame update
    void Start()
    {
        GameObject gameObject = new GameObject("");
        cam = gameObject.AddComponent<Camera>();
        cam.CopyFrom(GetComponentInParent<Camera>());
        rt = new RenderTexture(Screen.width, Screen.height, 24);
        cam.targetTexture = rt;
    }

    // Update is called once per frame
    void Update()
    {
        CaptureLogic();
    }

    private void CaptureLogic()
    {
        cam.Render();

        Texture2D tex = new Texture2D(Screen.width, Screen.height);
        RenderTexture.active = rt;
        tex.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
        tex.Apply();

        float luminosityLevel = GetLuminosityLevels(tex.GetPixels());

        if (luminosityLevel < luminosityThreshold)
        {
            luminosityLevel = luminosityThreshold;
        }

        float luminosityPercentage = (1 / (luminosityThreshold - 1)) * luminosityLevel - (1 / (luminosityThreshold - 1));

        Debug.Log("Luminosity Percentage = " + luminosityPercentage * 100 + "%    Luminosity Level = " + luminosityLevel);
    }

    private float GetLuminosityLevels(Color[] pixels)
    {
        float totalLuminosity = 0;
        int pixelCount = 0;
        float gamma = 2.2f;

        foreach (Color pixel in pixels)
        {
            if (pixel.grayscale > blackThreshold)
            {
                float linearLuminosity = pixel.grayscale;
                float gammaCorrectedLuminosity = Mathf.Pow(linearLuminosity, 1 / gamma);

                totalLuminosity += gammaCorrectedLuminosity;
                pixelCount++;
            }
        }

        float averagedLuminosity = totalLuminosity / pixelCount;

        return averagedLuminosity;
    }
}
