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

        public void SetupUI(GameObject p_UIPrefab)
        {
            for (int i = 0; i < Elements.Count; ++i)
                AddElementToUI(p_UIPrefab, Elements[i]);
        }

        public void Add(GameObject p_UIPrefab, T p_element)
        {
            Elements.Add(p_element);
            AddElementToUI(p_UIPrefab, p_element);
        }

        private void AddElementToUI(GameObject p_UIPrefab, InventoryElement p_element)
        {
            GameObject newElementObject = Instantiate(p_UIPrefab, ElementUIHandle.transform);
            newElementObject.transform.SetSiblingIndex(ElementUIHandle.transform.childCount - 2);
            newElementObject.name = p_element.ElementName;

            InventoryElementUI newElementUI = newElementObject.GetComponent<InventoryElementUI>();
            newElementUI.SetupUI(p_element);
            newElementUI.OnRemoveFromInventory += RemoveFromInventory;
        }

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

    void SetupInventory()
    {
        weapons.SetupUI(InventoryElementUIPrefab);
        points.SetupUI(InventoryElementUIPrefab);
        instruments.SetupUI(InventoryElementUIPrefab);
    }

    public void OpenInventory()
    {
        gameObject.SetActive(true);
    }

    public void CloseInventory()
    {
        gameObject.SetActive(false);
        weapons.ContentHandle.SetActive(false);
        points.ContentHandle.SetActive(false);
        instruments.ContentHandle.SetActive(false);
    }

    public void AddInventoryElement<T>(T p_inventoryElement, GameObject p_elementObject) where T : InventoryElement
    {
        Weapon newWeapon = p_inventoryElement as Weapon;
        if (newWeapon)
        {
            weapons.Add(InventoryElementUIPrefab, newWeapon);
            Destroy(p_elementObject);
            return;
        }

        InventoryPoint newPoint = p_inventoryElement as InventoryPoint;
        if (newPoint)
        {
            points.Add(InventoryElementUIPrefab, newPoint);
            Destroy(p_elementObject);
            return;
        }

        Instrument newInstrument = p_inventoryElement as Instrument;
        if (newInstrument)
        {
            instruments.Add(InventoryElementUIPrefab, newInstrument);
            Destroy(p_elementObject);
            return;
        }
    }
}
