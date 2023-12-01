using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TargetController : MonoBehaviour
{
    public TextMeshProUGUI message;
    public RawImage raw;
    public GameObject monsterUI;
    public GameObject monster;

    public void EnableMessage()
    {
        message.enabled = true;
    }

    public void DisableMessage()
    {
        message.enabled = false;
    }

    public void EnableRhinoUI()
    {
        message.enabled = false;
        raw.enabled = true;
        monsterUI.SetActive(true);
        monster.SetActive(true);
    }

    public void DisableRhinoUI()
    {
        message.enabled = true;
        raw.enabled = false;
        monster.SetActive(false);
        monsterUI.SetActive(false);
    }

    public void EnableGeneric()
    {
        message.enabled = false;
        monsterUI.SetActive(true);
        monster.SetActive(true);
    }

    public void DisableGeneric()
    {
        message.enabled = true;
        monsterUI.SetActive(false);
        monster.SetActive(false);
    }
}
