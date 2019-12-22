using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.InputHandler;
using Assets.Scripts.World;
using Assets.Scripts.Pattern;


//[RequireComponent(typeof(Camera))]
public class Player : MonoBehaviour
{
    [SerializeField][Range(1,5)]
    private byte m_ViewDistance;

    private Camera m_Camera;


    [SerializeField]
    private Vector3Int m_PlayerSlot;

    //[SerializeField]
    //private MySubject<Vector3Int> m_WorldSlot;

    public World m_RefWorld;

    
    public Vector3Int PlayerSlot { get { return m_PlayerSlot; } }
    //public MySubject<Vector3Int> SubjectWorldSlot { get { return m_WorldSlot; } }
    public byte ViewDistance { get { return m_ViewDistance; } }


    private void Awake()
    {
        Debug.Log("Awake is called!");
        m_Camera = GetComponent<Camera>();
    }

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Start is called!");
    }


    // Update is called once per frame
    void Update()
    {
        m_PlayerSlot = m_RefWorld.CoordToSectionSlot(transform.position);
    }
}
