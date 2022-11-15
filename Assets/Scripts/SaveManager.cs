using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    struct ControllerSaveData
    {
        public Vector3 Pos;
        public Vector3 Rot;
        public Vector3 Item;
    }

    struct InventorySaveData
    {
        public List<Weapon> Weapons;
        public List<InventoryPoint> Points;
        public List<Instrument> Instruments;

        public bool IsOpen;
    }

    struct ContainerSaveData
    {
        PickupContainer container;
        GameObject objectHeld;
    }

    struct SaveData
    {
        public Vector3 PlayerPosition;

        public ControllerSaveData leftController;
        public ControllerSaveData rightController;

        InventorySaveData inventoryData;

        List<ContainerSaveData> containers;
    }

    
}
