using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : BaseSingelton<InputManager>
{
    [SerializeField] InputActionReference movementControll;
    [SerializeField] InputActionReference jumpControll;
    [SerializeField] InputActionReference shootControll;

    private void OnEnable()
    {
        movementControll.action.Enable();
        jumpControll.action.Enable();
        shootControll.action.Enable();

    }
    private void OnDisable()
    {
        movementControll.action.Disable();
        jumpControll.action.Disable();
        shootControll.action.Disable();
    }

    public float CurrentPosition()
    {
        return movementControll.action.ReadValue<float>();
    }

    public bool IsJumping()
    {
        return jumpControll.action.triggered;
    }

    public bool IsShooting()
    {
        return shootControll.action.triggered;
    }

}
