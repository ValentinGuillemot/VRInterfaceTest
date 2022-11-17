using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class SimulatorButton : SimulatorInteractable
{
    Button _linkedButton;

    private void Start()
    {
        _linkedButton = GetComponent<Button>();
    }

    public override void Select(SimulatedController p_controller, GameObject p_controllerItem)
    {
        _linkedButton.onClick?.Invoke();
    }
}
