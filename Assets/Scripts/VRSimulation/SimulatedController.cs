using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SimulatedController : MonoBehaviour
{
    [SerializeField]
    InputActionReference moveAction;

    [SerializeField]
    InputActionReference lookAction;

    Vector3 _movement = Vector3.zero;

    bool _hasControl = false;

    public bool Control
    {
        get => _hasControl;
        set => _hasControl = value;
    }

    Vector2 _rotationAngles = Vector2.zero;

    [SerializeField]
    float _angularSpeed = 5f;

    void Start()
    {
        moveAction.action.Enable();
        lookAction.action.Enable();

        moveAction.action.performed += MoveController;
        moveAction.action.canceled += MoveController;
        lookAction.action.performed += RotateController;
    }

    private void Update()
    {
        if (!_hasControl)
            return;

        if (_movement != Vector3.zero)
            transform.position += transform.rotation * _movement * Time.deltaTime;
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
    }
}
