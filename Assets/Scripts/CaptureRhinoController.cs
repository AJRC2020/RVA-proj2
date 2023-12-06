using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using Vuforia;

public class CaptureRhinoController : MonoBehaviour
{
    public GameObject projectilePrefab;
    public float arrowSpeed = 10.0f;
    public UnityEngine.UI.Image progressBar;
    public float arrowDamage = 0.2f;

    private Camera cam;
    private bool isCaptured = false;
    private float hp = 1;

    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isCaptured)
        {
            updateUI();
        }
    }

    public void CreateArrow()
    {
        if (!isCaptured)
        {
            GameObject arrow = Instantiate(projectilePrefab, cam.transform.position, Quaternion.identity);

            Vector3 dir = cam.transform.forward;

            arrow.GetComponent<Rigidbody>().AddForce(dir * arrowSpeed);
        }
    }

    private void updateUI()
    {
        progressBar.fillAmount = hp;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 6)
        {
            hp -= arrowDamage;
            Destroy(other.gameObject);
        }
    }
}
