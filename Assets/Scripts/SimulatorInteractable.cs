using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider))]
public abstract class SimulatorInteractable : MonoBehaviour
{
    /// <summary>
    /// Called when a controller selects while hovering interactable
    /// </summary>
    /// <param name="p_controller">Controller that performed the select action.</param>
    /// <param name="p_controllerItem">GameObject currently held by the controller (will be null if no item is currently held).</param>
    public abstract void Select(SimulatedController p_controller, GameObject p_controllerItem);

    public UnityEvent OnSelect;
    public UnityEvent OnStartHover;
    public UnityEvent OnEndHover;
}
