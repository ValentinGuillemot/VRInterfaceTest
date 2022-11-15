using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickableItem : SimulatorInteractable
{
    Collider _collider;

    private void Start()
    {
        _collider = GetComponent<Collider>();
    }

    public override void Select(SimulatedController p_controller, GameObject p_controllerItem)
    {
        _collider.enabled = false;
        p_controller.Pickup(this);
    }

}
