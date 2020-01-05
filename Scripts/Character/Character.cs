using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.InputHandler;
using Assets.Scripts.WorldComponent;
using Assets.Scripts.Pattern;


//[RequireComponent(typeof(Camera))]
public class Character : MonoBehaviour
{
    [SerializeField][Range(1,5)]
    private byte m_ViewDistance;

    [SerializeField]
    private Transform m_Camera;
    [SerializeField]
    private Vector3Int m_PlayerSlot;
    [SerializeField]
    private World m_refWorld;
    [SerializeField]
    private float m_WalkSpeed = 1;
    [SerializeField]
    private float m_RunSpeed = 2;
    [SerializeField]
    private float m_JumpForce = 0.5f;


    public Transform Camera { get { return m_Camera; } }
    public float WalkSpeed { get { return m_WalkSpeed; } }
    public float RunSpeed { get { return m_RunSpeed; } }
    public float JumpForce { get { return m_JumpForce; } }
    public World refWorld { get { return m_refWorld; } }  
    public Vector3Int PlayerSlot { get { return m_PlayerSlot; } }
    //public MySubject<Vector3Int> SubjectWorldSlot { get { return m_WorldSlot; } }
    public byte ViewDistance { get { return m_ViewDistance; } }
    public Vector3 Movement { get; set; }

    private void Awake()
    {
        Debug.Log("Awake is called!");     
    }

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Start is called!");
    }


    // Update is called once per frame
    void Update()
    {
        //m_PlayerSlot = m_refWorld.CoordToSectionSlot(transform.position);
    }
}
