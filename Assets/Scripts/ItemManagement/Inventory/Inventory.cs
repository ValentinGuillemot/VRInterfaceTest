using System;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    [Serializable]
    struct InventoryCategory<T> where T : InventoryElement
    {
        public List<T> Elements;
        public GameObject ContentHandle;
        public GameObject ElementUIHandle;

        /// <summary>
        /// Initialize UI for each InventoryElements
        /// </summary>
        /// <param name="p_UIPrefab">Prefab of InventoryElementUI</param>
        public void SetupUI(GameObject p_UIPrefab)
        {
            for (int i = 0; i < Elements.Count; ++i)
                AddElementToUI(p_UIPrefab, Elements[i]);
        }

        /// <summary>
        /// Add element to both list of elements and UI
        /// </summary>
        /// <param name="p_UIPrefab"></param>
        /// <param name="p_element"></param>
        public void Add(GameObject p_UIPrefab, T p_element)
        {
            Elements.Add(p_element);
            AddElementToUI(p_UIPrefab, p_element);
        }

        /// <summary>
        /// Create a new subCategory in the UI and add it to the list
        /// </summary>
        /// <param name="p_UIPrefab"></param>
        /// <param name="p_element"></param>
        private void AddElementToUI(GameObject p_UIPrefab, InventoryElement p_element)
        {
            GameObject newElementObject = Instantiate(p_UIPrefab, ElementUIHandle.transform);
            newElementObject.transform.SetSiblingIndex(ElementUIHandle.transform.childCount - 2);
            newElementObject.name = p_element.ElementName;

            InventoryElementUI newElementUI = newElementObject.GetComponent<InventoryElementUI>();
            newElementUI.SetupUI(p_element);
            newElementUI.OnRemoveFromInventory += RemoveFromInventory;
        }

        /// <summary>
        /// Remove element from both element list and UI
        /// </summary>
        /// <param name="p_element"></param>
        public void RemoveFromInventory(InventoryElementUI p_element)
        {
            for (int i = 0; i < Elements.Count; ++i)
            {
                if (ElementUIHandle.transform.GetChild(i).GetComponent<InventoryElementUI>() == p_element)
                {
                    Elements.RemoveAt(i);
                    return;
                }
            }
        }

        /// <summary>
        /// Clear all elements and UI
        /// </summary>
        public void Clear()
        {
            Elements.Clear();

            int inventoryCount = ElementUIHandle.transform.childCount - 1;
            for (int i = 0; i < inventoryCount; i++)
            {
                Destroy(ElementUIHandle.transform.GetChild(i).gameObject);
            }
        }
    }

    [SerializeField]
    InventoryCategory<Weapon> weapons;
    
    [SerializeField]
    InventoryCategory<InventoryPoint> points;
    
    [SerializeField]
    InventoryCategory<Instrument> instruments;

    [SerializeField]
    GameObject InventoryElementUIPrefab;

    private void Start()
    {
        SetupInventory();
        gameObject.SetActive(false);
    }

    /// <summary>
    /// Setup inventory for all types of elements
    /// </summary>
    void SetupInventory()
    {
        weapons.SetupUI(InventoryElementUIPrefab);
        points.SetupUI(InventoryElementUIPrefab);
        instruments.SetupUI(InventoryElementUIPrefab);
    }

    /// <summary>
    /// Activate inventory game object
    /// </summary>
    public void OpenInventory()
    {
        gameObject.SetActive(true);
    }

    /// <summary>
    /// Hide inventory game object and all categories objects as well
    /// </summary>
    public void CloseInventory()
    {
        gameObject.SetActive(false);
        weapons.ContentHandle.SetActive(false);
        points.ContentHandle.SetActive(false);
        instruments.ContentHandle.SetActive(false);
    }

    /// <summary>
    /// Check an inventory element based on its type and add it to the correct category
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="p_inventoryElement"></param>
    /// <param name="p_elementObject"></param>
    public void AddInventoryElement(InventoryElement p_inventoryElement, GameObject p_elementObject)
    {
        Weapon newWeapon = p_inventoryElement as Weapon;
        if (newWeapon)
        {
            weapons.Add(InventoryElementUIPrefab, newWeapon);
            if (p_elementObject)
                Destroy(p_elementObject);
            return;
        }

        InventoryPoint newPoint = p_inventoryElement as InventoryPoint;
        if (newPoint)
        {
            points.Add(InventoryElementUIPrefab, newPoint);
            if (p_elementObject)
                Destroy(p_elementObject);
            return;
        }

        Instrument newInstrument = p_inventoryElement as Instrument;
        if (newInstrument)
        {
            instruments.Add(InventoryElementUIPrefab, newInstrument);
            if (p_elementObject)
                Destroy(p_elementObject);
            return;
        }
    }

    /// <summary>
    /// Create copy of weapon list
    /// </summary>
    /// <returns>Created copy</returns>
    public List<Weapon> CopyWeaponsList()
    {
        return new List<Weapon>(weapons.Elements);
    }

    /// <summary>
    /// Create copy of inventory points list
    /// </summary>
    /// <returns>Created copy</returns>
    public List<InventoryPoint> CopyPointsList()
    {
        return new List<InventoryPoint>(points.Elements);
    }

    /// <summary>
    /// Create copy of instruments list
    /// </summary>
    /// <returns>Created copy</returns>
    public List<Instrument> CopyInstrumentsList()
    {
        return new List<Instrument>(instruments.Elements);
    }

    /// <summary>
    /// Empty inventory and clear the UI
    /// </summary>
    public void ClearInventory()
    {
        weapons.Clear();
        points.Clear();
        instruments.Clear();

        CloseInventory();
    }
}
