using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class Player : MonoBehaviour
{
    private Camera m_Camera;

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
        Debug.Log("Start is called!");
    }
}
