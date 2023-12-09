using System.ComponentModel;
using DefaultNamespace;
using UnityEngine;
using UnityEngine.UI;

public class CaptureCactusController : MonoBehaviour
{
    public float shakeThreshold = 1.5f;
    public float fillSpeed = 0.05f;
    public float timeThreshold = 1.0f;
    public float fillPenalty = 0.1f;
    public Slider progressBar;

    private float totalFill;
    private float timeLapse;
    private bool isCaptured;

    public GameObject PricklashCaptureUI;
    public delegate void Captured(GameObject monsterUI,Target target);
    public static event Captured OnCaptured;
    

    void Update()
    {
        if (!isCaptured && PricklashCaptureUI.activeSelf)
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
            //Debug.Log("Captured");
            isCaptured = true;
            OnCaptured(PricklashCaptureUI,Target.Pricklash);
        }
    }
    


    private void UpdateUI()
    {
        progressBar.value = totalFill;
    }
}
