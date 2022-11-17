using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class VRSimulator : MonoBehaviour
{
    [SerializeField]
    InputActionReference moveAction;

    [SerializeField]
    InputActionReference lookAction;

    [SerializeField]
    InputActionReference selectAction;

    [SerializeField]
    InputActionReference switchAction;

    [SerializeField]
    SimulatedController leftController;

    [SerializeField]
    SimulatedController rightController;

    [SerializeField]
    Camera cam;

    [SerializeField]
    float cameraMaxY = 75f;

    [SerializeField]
    float cameraMinY = -75f;

    Vector2 _cameraAngles = Vector2.zero;

    [SerializeField]
    float cameraAngularSpeed = 5f;

    // Time you can keep the warp line without moving it until it disappears
    [SerializeField]
    float timeWarpLineIsAvailable = 3f;

    float _timeLeftWithWarpLine = 0f;

    [SerializeField]
    LineRenderer warpLine;

    [SerializeField]
    float timeWarpIsActive;

    [SerializeField]
    Vector3 initialWarpDirection;

    Vector3 warpDirection;

    Vector3 _currentWarpPos;

    Vector2 warpDirectionCurrentDelta;

    [SerializeField]
    float warpLineMovementSpeed = 2f;

    bool _hasControl = true;

    void Start()
    {
        moveAction.action.Enable();
        lookAction.action.Enable();
        selectAction.action.Enable();
        switchAction.action.Enable();

        moveAction.action.performed += MoveWarpCursor;
        moveAction.action.canceled += MoveWarpCursor;
        lookAction.action.performed += LookAround;
        selectAction.action.performed += Warp;
        switchAction.action.performed += SwitchControl;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        warpLine.enabled = false;

        initialWarpDirection = initialWarpDirection.normalized;
    }

    private void Update()
    {
        if (!_hasControl)
            return;

        if (warpLine.enabled)
        {
            warpDirection.x += warpDirectionCurrentDelta.x;
            warpDirection.z += warpDirectionCurrentDelta.y;
            UpdateWarpLine();

            _timeLeftWithWarpLine -= Time.deltaTime;
            if (_timeLeftWithWarpLine <= 0f)
                warpLine.enabled = false;
        }
    }

    /// <summary>
    /// Move line indicating the position to where the user can warp
    /// </summary>
    /// <param name="p_ctx"></param>
    void MoveWarpCursor(InputAction.CallbackContext p_ctx)
    {
        if (!_hasControl)
            return;

        _timeLeftWithWarpLine = timeWarpLineIsAvailable;

        if (!warpLine.enabled)
        {
            warpDirection = initialWarpDirection;
            warpLine.enabled = true;
            warpDirectionCurrentDelta = p_ctx.ReadValue<Vector2>() * warpLineMovementSpeed * Time.deltaTime;
            UpdateWarpLine();
        }
        else
        {
            warpDirectionCurrentDelta = p_ctx.ReadValue<Vector2>() * warpLineMovementSpeed * Time.deltaTime;
            UpdateWarpLine();
        }
    }

    /// <summary>
    /// Update Line renderer used to display where the user can warp
    /// </summary>
    void UpdateWarpLine()
    {
        if (Physics.Raycast(warpLine.gameObject.transform.position, transform.rotation * warpDirection, out RaycastHit hit, 100f))
        {
            warpLine.SetPosition(0, warpLine.gameObject.transform.position);
            warpLine.SetPosition(1, hit.point);
            _currentWarpPos = hit.point;
        }
    }

    /// <summary>
    /// Warp player to position based on warp line. Called by player input
    /// </summary>
    /// <param name="p_ctx"></param>
    void Warp(InputAction.CallbackContext p_ctx)
    {
        if (!_hasControl || !warpLine.enabled)
            return;

        transform.position = _currentWarpPos;
        warpLine.enabled = false;
    }

    /// <summary>
    /// Rotate camera based on player input
    /// </summary>
    /// <param name="p_ctx"></param>
    void LookAround(InputAction.CallbackContext p_ctx)
    {
        if (!_hasControl)
            return;

        Vector2 lookDelta = p_ctx.ReadValue<Vector2>();
        float timeSpeed = Time.deltaTime * cameraAngularSpeed;
        _cameraAngles.x += lookDelta.x * timeSpeed;
        _cameraAngles.x = _cameraAngles.x % 360f;
        if (_cameraAngles.x < 0f)
            _cameraAngles.x = 360f - _cameraAngles.x;

        _cameraAngles.y = Mathf.Clamp(_cameraAngles.y - lookDelta.y * timeSpeed, cameraMinY, cameraMaxY);

        // cameraAngles.x is the rotation to the sides (so rotation on the Y axis)
        // cameraAngles.y is the rotation up and down (so rotation on the X axis)
        cam.transform.localRotation = Quaternion.Euler(_cameraAngles.y, _cameraAngles.x, 0f);
    }

    /// <summary>
    /// Switch control between the simulator (camera) and the two controllers
    /// </summary>
    /// <param name="p_ctx"></param>
    void SwitchControl(InputAction.CallbackContext p_ctx)
    {
        if (_hasControl)
        {
            _hasControl = false;
            leftController.Control = true;
        }
        else if (leftController.Control)
        {
            leftController.Control = false;
            rightController.Control = true;
        }
        else
        {
            rightController.Control = false;
            _hasControl = true;
        }
    }
}
