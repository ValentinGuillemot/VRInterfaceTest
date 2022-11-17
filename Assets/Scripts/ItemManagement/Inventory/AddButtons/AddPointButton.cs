using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddPointButton : AddToInventoryButton
{
    protected override bool CheckElementType(InventoryElement p_element)
    {
        return (p_element as InventoryPoint != null);
    }
}
