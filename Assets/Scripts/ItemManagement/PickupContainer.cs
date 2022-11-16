using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PickupContainer : SimulatorInteractable
{
    public UnityEvent OnStoreItem;

    [SerializeField]
    GameObject storedItemHandle;

    GameObject _storedItem;

    [SerializeField]
    InventoryElement acceptedPickup;

    public override void Select(SimulatedController p_controller, GameObject p_controllerItem)
    {
        if (!p_controllerItem)
            return;

        // If an accepted pickup has been setup, only this pickup can be stored
        if (acceptedPickup && acceptedPickup != p_controllerItem.GetComponent<PickableItem>().InventoryData)
        {
            SoundManager.Instance.PlayWrongInputSound();
            return;
        }

        p_controllerItem.transform.parent = storedItemHandle.transform;
        p_controllerItem.transform.localPosition = Vector3.zero;
        p_controllerItem.transform.localRotation = Quaternion.identity;

        _storedItem = p_controllerItem;
        p_controller.RemoveItem();

        OnStoreItem?.Invoke();
    }

    public void SetItem(GameObject p_item)
    {
        if (_storedItem != p_item)
            Destroy(_storedItem);

        _storedItem = p_item;
    }
}
