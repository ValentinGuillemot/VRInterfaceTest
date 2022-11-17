using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AddToInventoryButton : SimulatorInteractable
{
    [SerializeField]
    protected Inventory inventory;

    public override void Select(SimulatedController p_controller, GameObject p_controllerItem)
    {
        if (p_controllerItem == null)
            return;

        InventoryElement pickedUpElement = p_controllerItem.GetComponent<PickableItem>().InventoryData;
        if (CheckElementType(pickedUpElement))
        {
            inventory.AddInventoryElement(pickedUpElement, p_controllerItem);
            p_controller.RemoveItem();
            SoundManager.Instance.PlayValidInputSound();
        }
        else
            SoundManager.Instance.PlayWrongInputSound();
    }

    /// <summary>
    /// Check if the type of given InventoryElement corresponds the required class
    /// </summary>
    /// <param name="p_element">Element to check</param>
    /// <returns>True if the type is valid, false otherwise</returns>
    protected abstract bool CheckElementType(InventoryElement p_element);
}
