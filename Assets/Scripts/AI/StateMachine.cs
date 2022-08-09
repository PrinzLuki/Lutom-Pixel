using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine<T>
{
    public T owner;
    public State<T> currentState;

    public StateMachine(T owner)
    {
        this.owner = owner;
    }

    public void Update()
    {
        if (currentState != null)
        {
            currentState.Update(owner);
        }
    }

    public void ChangeState(State<T> newState)
    {
        if (currentState == null)
        {
            newState.Enter(owner);
        }
        else if (currentState != null && newState != currentState)
        {
            currentState.Exit(owner);
            newState.Enter(owner);
        }
        currentState = newState;
    }
}

public abstract class State<T>
{
    public abstract void Enter(T owner);
    public abstract void Update(T owner);
    public abstract void Exit(T owner);
}