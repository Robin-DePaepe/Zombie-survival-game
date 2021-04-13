using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementBehaviour : MonoBehaviour
{
    //variables
    [SerializeField]
    protected float m_MovementSpeed = 10.0f;

    protected Rigidbody m_RigidBody;

    protected Vector3 m_DesiredMovementDirection = Vector3.zero;

    protected Vector2 m_Rotation = Vector2.zero;

    protected GameObject m_Target = null;

    private bool m_Sprinting = false;
    //functions

       
    public Vector3 DesiredMovementDirection
    {
        get { return m_DesiredMovementDirection; }
        set { m_DesiredMovementDirection = value; }
    }
    public Vector2 DesiredLookatPoint
    {
        get { return m_Rotation; }
        set { m_Rotation = value; }
    }
    public GameObject Target
    {
        get { return m_Target; }
        set { m_Target = value; }
    }
    protected virtual void Awake()
    {
        m_RigidBody = GetComponent<Rigidbody>();
    }
    protected virtual void Update()
    {
        HandleRotation();
    }

    protected virtual void FixedUpdate()
    {
        HandleMovement();
    }

    protected virtual void HandleMovement()
    {
        float sprintAcceleration = 1.5f;

        if (m_Sprinting && m_DesiredMovementDirection.y == 0)
        {
            m_RigidBody.MovePosition(m_RigidBody.position + m_DesiredMovementDirection.normalized * Time.deltaTime * m_MovementSpeed * sprintAcceleration);
        }
        else if(m_DesiredMovementDirection.y > 0)
        {
            m_RigidBody.MovePosition(m_RigidBody.position + m_DesiredMovementDirection.normalized * Time.deltaTime * m_MovementSpeed);
        }
        else
        {
            m_RigidBody.MovePosition(m_RigidBody.position + m_DesiredMovementDirection.normalized * Time.deltaTime * m_MovementSpeed);
        }
        m_Sprinting = false;
    }

    protected virtual void HandleRotation()
    {
        transform.localRotation = Quaternion.AngleAxis(m_Rotation.x, Vector3.up);
        transform.localRotation *= Quaternion.AngleAxis(-m_Rotation.y, Vector3.right);
    }

    public void Sprint()
    {
        m_Sprinting = true;
    }
    public void Jump()
    {
        float jumpHeight = 8f;
        m_RigidBody.velocity = Vector3.up * jumpHeight;
    }
}

