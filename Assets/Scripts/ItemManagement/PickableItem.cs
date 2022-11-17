using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickableItem : SimulatorInteractable
{
    Collider _collider;

    [SerializeField]
    InventoryElement inventoryData;

    public InventoryElement InventoryData => inventoryData;

    private void Start()
    {
        _collider = GetComponent<Collider>();
    }

    public override void Select(SimulatedController p_controller, GameObject p_controllerItem)
    {
        // When the item is picked up, it can no longer be interacted with as a selectable item, so the collider gets deactivated
        _collider.enabled = false;
        p_controller.Pickup(this);
    }

    /// <summary>
    /// Drop item, which allows it to be interacted with and picked up again
    /// </summary>
    public void Drop()
    {
        if (_collider)
            _collider.enabled = true;
    }

}
