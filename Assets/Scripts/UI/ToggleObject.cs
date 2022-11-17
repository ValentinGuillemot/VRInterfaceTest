using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleObject : MonoBehaviour
{
    /// <summary>
    /// Toggle whether game object is active or not
    /// </summary>
    public void Toggle()
    {
        gameObject.SetActive(!gameObject.activeSelf);
    }
}
