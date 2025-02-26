using UnityEngine;

public class StateMachine 
{
    public Istate currentState;

    public void SetState(Istate newState)
    {
        if (currentState != null)
            currentState.Exit();

        currentState = newState;
        currentState.Enter();
    }

    public void Update(Vector3 playerVector)
    {
        if (currentState != null)
            currentState.Execute(playerVector);
    }
}