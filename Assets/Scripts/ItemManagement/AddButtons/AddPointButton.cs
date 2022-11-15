using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddPointButton : AddToInventoryButton
{
    public override void Select(SimulatedController p_controller, GameObject p_controllerItem)
    {
        if (p_controllerItem == null)
            return;

        InventoryElement pickedUpElement = p_controllerItem.GetComponent<PickableItem>().InventoryData;
        if ((pickedUpElement as InventoryPoint) != null)
        {
            inventory.AddInventoryElement(pickedUpElement as InventoryPoint, p_controllerItem);
            p_controller.RemoveItem();
        }
    }
}
