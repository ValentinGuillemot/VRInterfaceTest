using System.Collections.Generic;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    struct ControllerSaveData
    {
        public Vector3 Pos;
        public Quaternion Rot;
        public GameObject Item;

        public void SaveData(SimulatedController p_controller)
        {
            Pos = p_controller.gameObject.transform.position;
            Rot = p_controller.gameObject.transform.rotation;
            Item = p_controller.HeldItem;
        }
    }

    struct InventorySaveData
    {
        public List<Weapon> Weapons;
        public List<InventoryPoint> Points;
        public List<Instrument> Instruments;

        public bool IsOpen;

        public void SaveData(Inventory p_inventory)
        {
            IsOpen = p_inventory.gameObject.activeInHierarchy;
            Weapons = p_inventory.GetWeaponsList();
            Points = p_inventory.GetPointsList();
            Instruments = p_inventory.GetInstrumentsList();
        }
    }

    struct ContainerSaveData
    {
        public PickupContainer Container;
        public GameObject ObjectHeld;
    }

    struct SaveData
    {
        public Vector3 PlayerPosition;
        public Quaternion PlayerRotation;

        public ControllerSaveData leftControllerData;
        public ControllerSaveData rightControllerData;

        public InventorySaveData inventoryData;

        public List<ContainerSaveData> containers;

        public void SaveContainersData()
        {
            PickupContainer[] allContainers = FindObjectsOfType<PickupContainer>();
            foreach (PickupContainer container in allContainers)
            {
                ContainerSaveData newData = new ContainerSaveData();
                newData.Container = container;
                //newData.Object
                containers.Add(newData);
            }
        }
    }

    [SerializeField]
    Player player;

    [SerializeField]
    SimulatedController leftController;

    [SerializeField]
    SimulatedController rightController;

    [SerializeField]
    Inventory inventory;

    private SaveData lastSave;

    public void Save()
    {
        lastSave.PlayerPosition = player.transform.position;
        lastSave.PlayerRotation = player.transform.rotation;

        lastSave.leftControllerData.SaveData(leftController);
        lastSave.rightControllerData.SaveData(rightController);

        lastSave.inventoryData.SaveData(inventory);

        lastSave.SaveContainersData();
    }

    public void Load()
    {
        
    }
}
