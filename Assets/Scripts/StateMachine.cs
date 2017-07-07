using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine<T>
{
    T m_AttachedGameObject;

    State<T> m_State = null;

    public StateMachine(T obj)
    {
        m_AttachedGameObject = obj;
    }

    public State<T> GetNowState()
    {
        return m_State;
    }

    public void Update()
    {
        if (m_State != null)
        {
            m_State.Execute(m_AttachedGameObject);
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

public abstract class State<T>
{
    public virtual void Enter(T obj)
    {

    }

    public virtual void Execute(T obj)
    {

    }

    public virtual void Exit(T obj)
    {

    }
}