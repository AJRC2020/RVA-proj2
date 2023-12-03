using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CaptureCactusController : MonoBehaviour
{
    public float shakeThreshold = 1.5f;
    public float fillSpeed = 0.05f;
    public float timeThreshold = 1.0f;
    public float fillPenalty = 0.1f;
    public Image progressBar;

    private float totalFill = 0.0f;
    private float timeLapse = 0.0f;
    private bool isCaptured = false;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (!isCaptured)
        {
            CaptureLogic();
            UpdateUI();
        }
    }

    private void CaptureLogic()
    {
        Vector3 shake = Input.acceleration;
        float shakeMag = shake.magnitude;

        if (shakeMag > shakeThreshold)
        {
            totalFill += fillSpeed * Time.deltaTime;
            totalFill = Mathf.Clamp01(totalFill);
            timeLapse = 0.0f;
        }
        else
        {
            timeLapse += Time.deltaTime;
        }

        if (timeLapse > timeThreshold)
        {
            totalFill -= fillPenalty * Time.deltaTime;
            totalFill = Mathf.Clamp01(totalFill);
        }

        if (totalFill == 1.0f)
        {
            Debug.Log("Captured");
            isCaptured = true;
        }

        Debug.Log("Magnitude = " + shakeMag + " Total Fill = " + totalFill);
    }

    private void UpdateUI()
    {
        var purple = new Color(0.5f, 0.0f, 1.0f, 1.0f);
        progressBar.fillAmount = totalFill;
        progressBar.color = Color.Lerp(Color.blue, purple, totalFill);
    }
}
