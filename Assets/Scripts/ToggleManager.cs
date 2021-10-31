using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Toggle))]
public class ToggleManager : MonoBehaviour
{
    Toggle toggle;
    GameController controller;

    void Awake()
    {
        toggle = GetComponent<Toggle>();
    }

    public void ChangeState_ShowSteps()
    {
        controller = FindObjectOfType<GameController>();
        if (toggle.isOn)
            controller.showSteps = true;
        else
            controller.showSteps = false;
        controller.ValuesSync();
    }
}
