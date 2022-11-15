using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    Inventory inventory;

    [SerializeField]
    SimulatedController leftController;

    [SerializeField]
    SimulatedController rightController;

    private void Start()
    {
        leftController.OnPickUpItem += inventory.OpenInventory;
        rightController.OnPickUpItem += inventory.OpenInventory;   
    }
}
