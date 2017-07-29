using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine<T> : Object where T : MonoBehaviour
{
    T m_AttachedGameObject;

    State<T> m_State = null;

    public State<T> currentState
    {
        get
        {
            return m_State;
        }
    }

    public StateMachine(T obj)
    {
        m_AttachedGameObject = obj;
    }

    public void Update()
    {
        if (m_State != null)
        {
            m_State.Execute(m_AttachedGameObject);
        }
    }

	public void FixedUpdate()
	{
		if (m_State != null)
		{
			m_State.FixedExecute (m_AttachedGameObject);
		}
	}

    public void ChangeState(State<T> state)
    {
        if(m_State != null)
            m_State.Exit(m_AttachedGameObject);

        m_State = state;
        m_State.Enter(m_AttachedGameObject);
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