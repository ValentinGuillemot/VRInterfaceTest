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
        if (_elementObject == null)
            return;

        GameObject pickup = Instantiate(_elementObject);
        p_controller.Pickup(pickup.GetComponent<PickableItem>());
        OnRemoveFromInventory?.Invoke(this);

        Destroy(gameObject);
    }

    public void SetupUI(InventoryElement p_element)
    {
        nameTMP.text = p_element.ElementName;
        elementImage.sprite = p_element.ElementSprite;

        _elementObject = p_element.ElementObject;
    }
}
