using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [SerializeField]
    Inventory inventory;

    [SerializeField]
    SimulatedController leftController;

    [SerializeField]
    SimulatedController rightController;

    [SerializeField]
    InputActionReference inventoryButton;

    [SerializeField]
    PickableItem savePickup;

    private void Start()
    {
        leftController.OnPickUpItem += inventory.OpenInventory;
        rightController.OnPickUpItem += inventory.OpenInventory;
        leftController.OnPickUpItem += SaveAfterPickingUpItem;
        rightController.OnPickUpItem += SaveAfterPickingUpItem;

        inventoryButton.action.Enable();
        inventoryButton.action.performed += ToggleInventory;
    }

    private void ToggleInventory(InputAction.CallbackContext p_ctx)
    {
        if (inventory.gameObject.activeSelf)
            inventory.CloseInventory();
        else
            inventory.OpenInventory();
    }

    // Save data only first time stone is being picked up
    private void SaveAfterPickingUpItem()
    {
        if (leftController.HeldItem == savePickup.gameObject || rightController.HeldItem == savePickup.gameObject)
        {
            SaveManager.Instance.Save();
            leftController.OnPickUpItem -= SaveAfterPickingUpItem;
            rightController.OnPickUpItem -= SaveAfterPickingUpItem;
        }
    }
}
