using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToggleSwitch : MonoBehaviour
{
    public GameObject panel;
    public void OnToggleSwitch()
    {
        bool switchIsOn = GameObject.Find("Toggle").GetComponent<Toggle>().isOn;

        if (switchIsOn)
            panel.SetActive(true);
        else panel.SetActive(false);

    }
}
