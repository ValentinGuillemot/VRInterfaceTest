using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class InventoryElement : ScriptableObject
{
    public string ElementName;

    public Sprite ElementSprite;

    public GameObject ElementObject;
}
