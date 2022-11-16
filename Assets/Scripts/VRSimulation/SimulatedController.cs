using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SimulatedController : MonoBehaviour
{
    [SerializeField]
    InputActionReference moveAction;

    [SerializeField]
    InputActionReference lookAction;

    [SerializeField]
    InputActionReference selectAction;

    [SerializeField]
    LineRenderer ray;

    [SerializeField]
    float rayMaxDistance = 10f;

    [SerializeField]
    LayerMask rayMask;

    [SerializeField]
    Material regularRayMat;

    [SerializeField]
    Material selectingRayMat;

    Vector3 _movement = Vector3.zero;

    bool _hasControl = false;

    SimulatorInteractable _currentlySelectedInteractable;

    GameObject _currentlyHeldItem;

    public GameObject HeldItem => _currentlyHeldItem;

    public Action OnPickUpItem;

    public bool Control
    {
        get => _hasControl;
        set => _hasControl = value;
    }

    Vector2 _rotationAngles = Vector2.zero;

    [SerializeField]
    float _angularSpeed = 5f;

    private bool _forceCheck = false;

    void Start()
    {
        moveAction.action.Enable();
        lookAction.action.Enable();
        selectAction.action.Enable();

        moveAction.action.performed += MoveController;
        moveAction.action.canceled += MoveController;
        lookAction.action.performed += RotateController;
        selectAction.action.performed += SelectInteractable;

        ray.SetPosition(1, Vector3.forward * rayMaxDistance);
    }

    private void Update()
    {
        if (!_hasControl)
            return;

        if (_forceCheck || _movement != Vector3.zero)
        {
            transform.position += transform.rotation * _movement * Time.deltaTime;
            CheckForInteractable();
        }
    }

    void MoveController(InputAction.CallbackContext p_ctx)
    {
        if (!_hasControl)
            return;

        _movement = p_ctx.ReadValue<Vector2>();
    }

    void RotateController(InputAction.CallbackContext p_ctx)
    {
        if (!_hasControl)
            return;

        Vector2 lookDelta = p_ctx.ReadValue<Vector2>();
        float timeSpeed = Time.deltaTime * _angularSpeed;
        _rotationAngles.x += lookDelta.x * timeSpeed;
        _rotationAngles.x = _rotationAngles.x % 360f;
        if (_rotationAngles.x < 0f)
            _rotationAngles.x = 360f - _rotationAngles.x;

        _rotationAngles.y = Mathf.Clamp(_rotationAngles.y - lookDelta.y * timeSpeed, -90f, 90f);

        // cameraAngles.x is the rotation to the sides (so rotation on the Y axis)
        // cameraAngles.y is the rotation up and down (so rotation on the X axis)
        transform.localRotation = Quaternion.Euler(_rotationAngles.y, _rotationAngles.x, 0f);

        CheckForInteractable();
    }

    void CheckForInteractable()
    {
        if (Physics.Raycast(ray.transform.position, ray.transform.forward, out RaycastHit hit, rayMaxDistance, rayMask))
        {
            Vector3 toHit = hit.point - ray.transform.position;
            ray.SetPosition(1, Vector3.forward * toHit.magnitude);

            SimulatorInteractable target = hit.collider.GetComponent<SimulatorInteractable>();
            if (target)
            {
                ray.material = selectingRayMat;
                if (target != _currentlySelectedInteractable)
                {
                    if (_currentlySelectedInteractable != null)
                        _currentlySelectedInteractable.OnEndHover?.Invoke();
                    target.OnStartHover?.Invoke();
                    _currentlySelectedInteractable = target;
                }
            }
        }
        else 
        {
            ray.material = regularRayMat;
            ray.SetPosition(1, Vector3.forward * rayMaxDistance);

            if (_currentlySelectedInteractable != null)
            {
                _currentlySelectedInteractable.OnEndHover?.Invoke();
                _currentlySelectedInteractable = null;
            }
        }

        
    }

    void SelectInteractable(InputAction.CallbackContext p_ctx)
    {
        if (!_hasControl || _currentlySelectedInteractable == null)
            return;

        _currentlySelectedInteractable.OnSelect?.Invoke();
        _currentlySelectedInteractable.Select(this, _currentlyHeldItem);

        // Check in case currently selected interactable has changed
        _forceCheck = true;
    }

    public void Pickup(PickableItem p_itemToPick)
    {
        if (_currentlyHeldItem != null)
        {
            _currentlyHeldItem.transform.parent = null;
            _currentlyHeldItem.transform.position = p_itemToPick.transform.position;
            _currentlyHeldItem.transform.rotation = p_itemToPick.transform.rotation;
        }

        _currentlyHeldItem = p_itemToPick.gameObject;
        _currentlyHeldItem.transform.parent = transform;
        _currentlyHeldItem.transform.localPosition = Vector3.zero;
        _currentlyHeldItem.transform.localRotation = Quaternion.identity;

        OnPickUpItem?.Invoke();
    }

    public void RemoveItem()
    {
        if (_currentlyHeldItem == null)
            return;

        _currentlyHeldItem.GetComponent<PickableItem>().Drop();
        _currentlyHeldItem = null;
    }
}
