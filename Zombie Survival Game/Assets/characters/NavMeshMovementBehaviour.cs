using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class NavMeshMovementBehaviour : MovementBehaviour
{
    //variables 

    static private bool m_SlowMotion;
    private bool m_InSmoke;

    private NavMeshAgent m_NavMeshAgent;

    private Vector3 m_PreviousTargetPosition = Vector3.zero;
    private bool m_InRange = false;

    const float MOVEMENT_EPSILON = .25f;

    //functions
    static public bool SlowMotion
    {
        set { m_SlowMotion = value; }
    }

    public bool InRange
    {
        set { m_InRange = value; }
    }
    public void SetInSmokeDuration(float duration)
    {
        if (!m_InSmoke)
        {
            m_InSmoke = true;
            Invoke("UnSetInSmoke", duration);
        }
    }

    private void UnSetInSmoke()
    {
        m_InSmoke = false;
    }

    protected override void Awake()
    {
        base.Awake();

        m_NavMeshAgent = GetComponent<NavMeshAgent>();

        m_PreviousTargetPosition = transform.position;
    }

    protected override void HandleMovement()
    {
        if (m_Target == null)
        {
            return;
        }

        if (m_InSmoke)//frozen in smoke
        {
            m_NavMeshAgent.speed = 0f;
        }
        else if (!m_SlowMotion) //Regular speed
        {
            m_NavMeshAgent.speed = m_MovementSpeed;
        }
        else
        {
            m_NavMeshAgent.speed = m_MovementSpeed / 2f; //slowmotion so half the speed
        }

        if (m_InRange)
        {
            m_NavMeshAgent.isStopped = true;
            m_NavMeshAgent.velocity = Vector3.zero;
        }
        else
        {
            m_NavMeshAgent.isStopped = false;
        }
        //should the target move we should recalculate our path
        if ((m_Target.transform.position - m_PreviousTargetPosition).sqrMagnitude > MOVEMENT_EPSILON)
        {
            m_NavMeshAgent.SetDestination(m_Target.transform.position);
            m_NavMeshAgent.isStopped = false;
            m_PreviousTargetPosition = m_Target.transform.position;
        }
    }
}
