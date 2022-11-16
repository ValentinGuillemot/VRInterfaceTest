using System;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SaveManager : MonoBehaviour
{
    [Serializable]
    struct ControllerSaveData
    {
        public Vector3 Pos;
        public Quaternion Rot;
        public GameObject Item;
        public InventoryElement ItemElement;

        public void SaveData(SimulatedController p_controller)
        {
            Pos = p_controller.gameObject.transform.position;
            Rot = p_controller.gameObject.transform.rotation;
            Item = p_controller.HeldItem;
            if (Item)
                ItemElement = Item.GetComponent<PickableItem>().InventoryData;
            else
                ItemElement = null;
        }

        public void LoadData(SimulatedController p_controller)
        {
            p_controller.gameObject.transform.position = Pos;
            p_controller.gameObject.transform.rotation = Rot;
            if (Item != null)
                Destroy(Item);

            if (ItemElement != null)
            {
                Item = Instantiate(ItemElement.ElementObject);
                p_controller.Pickup(Item.GetComponent<PickableItem>());
            }
            else
                p_controller.RemoveItem();

        }
    }

    [Serializable]
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

        public void LoadData(Inventory p_inventory)
        {
            p_inventory.ClearInventory();

            foreach (Weapon weapon in Weapons)
                p_inventory.AddInventoryElement(weapon, null);

            foreach (InventoryPoint point in Points)
                p_inventory.AddInventoryElement(point, null);

            foreach (Instrument instrument in Instruments)
                p_inventory.AddInventoryElement(instrument, null);

            if (IsOpen)
                p_inventory.OpenInventory();
            else
                p_inventory.CloseInventory();
        }
    }

    [Serializable]
    struct ContainerSaveData
    {
        public PickupContainer Container;
        public GameObject ObjectHeld;

        public void LoadData()
        {
            Container.SetItem(ObjectHeld);
        }
    }

    [Serializable]
    struct TextSaveData
    {
        public TextMeshPro textToSave;
        public string text;

        public void LoadData()
        {
            textToSave.text = text;
        }
    }

    [Serializable]
    struct SaveData
    {
        public Vector3 PlayerPosition;
        public Quaternion PlayerRotation;

        public ControllerSaveData leftControllerData;
        public ControllerSaveData rightControllerData;

        public InventorySaveData inventoryData;

        public List<ContainerSaveData> containers;

        public List<TextSaveData> savedTexts;

        public void SaveContainersData()
        {
            containers = new List<ContainerSaveData>();
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

    [SerializeField]
    List<TextMeshPro> textsToSave;

    private SaveData lastSave;

    public static SaveManager Instance;

    private void Awake()
    {
        Instance = this;
    }

    public void Save()
    {
        lastSave.PlayerPosition = player.transform.position;
        lastSave.PlayerRotation = player.transform.rotation;

        lastSave.leftControllerData.SaveData(leftController);
        lastSave.rightControllerData.SaveData(rightController);

        lastSave.inventoryData.SaveData(inventory);

        lastSave.SaveContainersData();

        lastSave.savedTexts = new List<TextSaveData>();
        foreach (TextMeshPro textMesh in textsToSave)
        {
            TextSaveData textSave = new TextSaveData();
            textSave.textToSave = textMesh;
            textSave.text = textMesh.text;
            lastSave.savedTexts.Add(textSave);
        }
    }

    public void Load()
    {
        player.transform.position = lastSave.PlayerPosition;
        player.transform.rotation = lastSave.PlayerRotation;

        lastSave.leftControllerData.LoadData(leftController);
        lastSave.rightControllerData.LoadData(rightController);

        lastSave.inventoryData.LoadData(inventory);

        foreach (ContainerSaveData data in lastSave.containers)
            data.LoadData();

        foreach (TextSaveData data in lastSave.savedTexts)
            data.LoadData();
    }
}
