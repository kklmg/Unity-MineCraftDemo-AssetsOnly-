using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.InputHandler;
using Assets.Scripts.World;



[RequireComponent(typeof(Camera))]
public class Player : MonoBehaviour
{
    private Camera m_Camera;
    public Vector3Int m_worldSlot;
    public World m_World;

    private void Awake()
    {
        m_worldSlot = m_World.CoordToSlot(transform.position);

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
        Vector3Int slot = m_World.CoordToSlot(transform.position);

        if (slot != m_worldSlot)
        {
            m_worldSlot = slot;
            m_World.CreateChunk(slot.x, slot.z);
        }
    }
}
