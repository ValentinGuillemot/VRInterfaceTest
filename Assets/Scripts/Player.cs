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

    private void Start()
    {
        leftController.OnPickUpItem += inventory.OpenInventory;
        rightController.OnPickUpItem += inventory.OpenInventory;

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
}
