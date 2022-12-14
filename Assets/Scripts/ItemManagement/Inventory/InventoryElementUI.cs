using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventoryElementUI : SimulatorInteractable
{
    [SerializeField]
    TextMeshProUGUI nameTMP;

    [SerializeField]
    Image elementImage;

    GameObject _elementObject;

    public Action<InventoryElementUI> OnRemoveFromInventory;

    public override void Select(SimulatedController p_controller, GameObject p_controllerItem)
    {
        // If there is no object to give or if the controller is already holding an item, cancels the selection
        if (_elementObject == null || p_controllerItem != null)
            return;

        GameObject pickup = Instantiate(_elementObject);
        p_controller.Pickup(pickup.GetComponent<PickableItem>());
        OnRemoveFromInventory?.Invoke(this);

        Destroy(gameObject);
    }

    /// <summary>
    /// Set UI elements (text and image) based on given InventoryElement
    /// </summary>
    /// <param name="p_element"></param>
    public void SetupUI(InventoryElement p_element)
    {
        nameTMP.text = p_element.ElementName;
        elementImage.sprite = p_element.ElementSprite;

        _elementObject = p_element.ElementObject;
    }
}
