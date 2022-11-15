using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventoryElementUI : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI nameTMP;

    [SerializeField]
    Image elementImage;

    GameObject _elementObject;

    public void SetupUI(InventoryElement p_element)
    {
        nameTMP.text = p_element.ElementName;
        elementImage.sprite = p_element.ElementSprite;

        _elementObject = p_element.ElementObject;
    }
}
