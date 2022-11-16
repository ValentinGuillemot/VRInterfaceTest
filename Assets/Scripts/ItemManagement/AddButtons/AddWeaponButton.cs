using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddWeaponButton : AddToInventoryButton
{
    public override void Select(SimulatedController p_controller, GameObject p_controllerItem)
    {
        if (p_controllerItem == null)
            return;

        InventoryElement pickedUpElement = p_controllerItem.GetComponent<PickableItem>().InventoryData;
        if ((pickedUpElement as Weapon) != null)
        {
            inventory.AddInventoryElement(pickedUpElement as Weapon, p_controllerItem);
            p_controller.RemoveItem();
            SoundManager.Instance.PlayValidInputSound();
        }
        else
            SoundManager.Instance.PlayWrongInputSound();
}
}
