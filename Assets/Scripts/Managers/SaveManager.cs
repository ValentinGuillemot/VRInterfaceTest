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

        /// <summary>
        /// Store position, rotation and object held by a controller
        /// </summary>
        /// <param name="p_controller"></param>
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

        /// <summary>
        /// Reset position, rotation and object of given controller to saved values
        /// </summary>
        /// <param name="p_controller"></param>
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

        /// <summary>
        /// Save a copy of items list of inventory
        /// </summary>
        /// <param name="p_inventory"></param>
        public void SaveData(Inventory p_inventory)
        {
            IsOpen = p_inventory.gameObject.activeInHierarchy;
            Weapons = p_inventory.CopyWeaponsList();
            Points = p_inventory.CopyPointsList();
            Instruments = p_inventory.CopyInstrumentsList();
        }

        /// <summary>
        /// Recreate inventory with item lists previously saved
        /// </summary>
        /// <param name="p_inventory"></param>
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

        /// <summary>
        /// Reset TMPro text to stored value
        /// </summary>
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

        /// <summary>
        /// Store what items containers are currently holding (can be null)
        /// </summary>
        public void SaveContainersData()
        {
            containers = new List<ContainerSaveData>();
            PickupContainer[] allContainers = FindObjectsOfType<PickupContainer>();
            foreach (PickupContainer container in allContainers)
            {
                ContainerSaveData newData = new ContainerSaveData();
                newData.Container = container;
                newData.ObjectHeld = container.GetItem();
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

    /// <summary>
    /// Save current status of tracked objects in scene (player, controllers, inventory, objects)
    /// </summary>
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

    /// <summary>
    /// Reset status of tracked objects in scene to previously stored values
    /// </summary>
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
