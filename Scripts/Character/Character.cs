using UnityEngine;

//[RequireComponent(typeof(Camera))]
public class Character : MonoBehaviour
{
    [SerializeField]
    [Range(1, 5)]
    private byte m_ViewDistance;

    [SerializeField]
    private Transform m_Camera;
    [SerializeField]
    private Vector3Int m_PlayerSlot;

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


    public float BodyRadius { get { return m_BodyRadius; } }
    public float BodyHeight { get { return m_BodyHeight; } }
    public Transform TCamera { get { return m_Camera; } }
    public float WalkSpeed { get { return m_WalkSpeed; } }
    public float RunSpeed { get { return m_RunSpeed; } }
    public float JumpForce { get { return m_JumpForce; } }
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
        Ray ray = Camera.main.ScreenPointToRay(new Vector2((float)Screen.width / 2f, (float)Screen.height / 2f));
        Debug.DrawRay(transform.position, ray.direction * 50, Color.red);

        //m_PlayerSlot = m_refWorld.CoordToSectionSlot(transform.position);
    }
}
