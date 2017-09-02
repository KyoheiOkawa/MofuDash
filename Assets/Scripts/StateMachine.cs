using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine<T> : Object where T : MonoBehaviour
{
    T attachedGameObject;

    State<T> stage = null;

    public State<T> CurrentState
    {
        get
        {
            return stage;
        }
    }

    public StateMachine(T obj)
    {
        attachedGameObject = obj;
    }

    public void Update()
    {
        if (stage != null)
        {
            stage.Execute(attachedGameObject);
        }
    }

	public void FixedUpdate()
	{
		if (stage != null)
		{
			stage.FixedExecute (attachedGameObject);
		}
	}

    public void ChangeState(State<T> state)
    {
        if(stage != null)
            stage.Exit(attachedGameObject);

        stage = state;
        stage.Enter(attachedGameObject);
    }
}

public abstract class State<T> : ScriptableObject where T : MonoBehaviour
{
    public virtual void Enter(T obj)
    {

    }

    public virtual void Execute(T obj)
    {

    }

	public virtual void FixedExecute(T obj)
	{

	}

    public virtual void Exit(T obj)
    {

    }
}