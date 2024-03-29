using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : BaseSingelton<InputManager>
{
    [SerializeField] InputActionReference movementControll;
    [SerializeField] InputActionReference jumpControll;
    [SerializeField] InputActionReference shootControll;
    [SerializeField] InputActionReference interActionControll;
    [SerializeField] InputActionReference dropActionControll;
    [SerializeField] InputActionReference pauseActionControll;
    [SerializeField] InputActionReference chatActionControll;

    private void Start()
    {
        OnEnable();
    }
    private void OnEnable()
    {
        movementControll.action.Enable();
        jumpControll.action.Enable();
        shootControll.action.Enable();
        interActionControll.action.Enable();
        dropActionControll.action.Enable();
        pauseActionControll.action.Enable();
        chatActionControll.action.Enable();

    }
    private void OnDisable()
    {
        movementControll.action.Disable();
        jumpControll.action.Disable();
        shootControll.action.Disable();
        interActionControll.action.Disable();
        dropActionControll.action.Disable();
        pauseActionControll.action.Disable();
        chatActionControll.action.Disable();
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

    public bool Interact()
    {
        return interActionControll.action.triggered;
    }

    public bool Drop()
    {
        return dropActionControll.action.triggered;
    }

    public bool Pause()
    {
        return pauseActionControll.action.triggered;
    }

    public bool Chat()
    {
        return chatActionControll.action.triggered;
    }

}
