using UnityEngine;

//[RequireComponent(typeof(Camera))]
public class Character : MonoBehaviour
{
    //Field
    //-----------------------------------------------------------------------
    [SerializeField]
    [Range(0, 5)]
    private byte m_ViewDistance = 1;

    [SerializeField]
    private Transform m_Camera;

    [SerializeField]
    private float m_BodyRadius = 1;
    [SerializeField]
    private float m_BodyHeight = 2;

    [SerializeField]
    private float m_WalkSpeed = 1;
    [SerializeField]
    private float m_RunSpeed = 2;
    [SerializeField]
    private float m_JumpForce = 0.5f;

    //Properties
    //-----------------------------------------------------------------------
    public float BodyRadius { get { return m_BodyRadius; } }
    public float BodyHeight { get { return m_BodyHeight; } }
    public Transform TCamera { get { return m_Camera; } }
    public float WalkSpeed { get { return m_WalkSpeed; } }
    public float RunSpeed { get { return m_RunSpeed; } }
    public float JumpForce { get { return m_JumpForce; } }
    public byte ViewDistance { get { return m_ViewDistance; } }
    public Vector3 Movement { get; set; }

}
