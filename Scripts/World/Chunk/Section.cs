using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.SMesh;


namespace Assets.Scripts.NWorld
{
    [RequireComponent(typeof(MeshFilter))]
    [RequireComponent(typeof(MeshRenderer))]
    [RequireComponent(typeof(MeshCollider))]

    public class Section : MonoBehaviour
    {
        //Field----------------------------------------
        [SerializeField]
        private MeshData m_MeshData;

        private Chunk m_refChunk;
        private World m_refWorld;
        private List<Block> m_refBlocks;
        private TextureSheet m_refTexs;

        [SerializeField]
        private Vector3Int m_SecionSlot;

        [SerializeField]
        private byte[,,] m_arrBlockID;

        //unity component
        private MeshFilter m_MeshFilter;
        private MeshRenderer m_MeshRenderer;
        private MeshCollider m_Collider;
       
        //property--------------------------------------
        public bool isDirtry { get; set; }

        public Vector3Int SectionSlot { get { return m_SecionSlot; } set { m_SecionSlot = value; } }

        //unity function-----------------------------------------
        private void Awake()
        {
            //Debug.Log("Section_start");

            //cache unity component
            m_MeshFilter = gameObject.GetComponent<MeshFilter>();
            m_MeshRenderer = gameObject.GetComponent<MeshRenderer>();
            m_Collider = gameObject.GetComponent<MeshCollider>();

            //cache World reference
            m_refChunk = GetComponentInParent<Chunk>();
            m_refWorld = m_refChunk.WorldReference;
            m_refBlocks = m_refChunk.WorldReference.BlockList;

            m_refTexs = m_refChunk.WorldReference.TexSheet;

            m_MeshRenderer.materials = new Material[1];
            m_MeshRenderer.materials[0] = new Material(Shader.Find("Unlit/Texture"));
            if (m_MeshRenderer.materials[0] == null)
            {
                Debug.Log("Can't find Shader");
            }
            m_MeshRenderer.materials[0].mainTexture = m_refTexs.TexSheet;

            //make instance of mesh data 
            m_MeshData = ScriptableObject.CreateInstance<MeshData>();
            m_MeshData.Reset();

            //generate world
            //


        }
        private void Start()
        {
            //Debug.Log("mesh update");
        }
        private void Update()
        {
            if (isDirtry == true)
            {
                UpdateMesh();

                //update adjacent Sections
                Section adjSection;
                //up
                adjSection = m_refWorld.GetSection(m_SecionSlot + Vector3Int.up);
                if (adjSection != null) adjSection.UpdateMesh();
                //down
                adjSection = m_refWorld.GetSection(m_SecionSlot + Vector3Int.down);
                if (adjSection != null) adjSection.UpdateMesh();
                //left
                adjSection = m_refWorld.GetSection(m_SecionSlot + Vector3Int.left);
                if (adjSection != null) adjSection.UpdateMesh();
                //right
                adjSection = m_refWorld.GetSection(m_SecionSlot + Vector3Int.right);
                if (adjSection != null) adjSection.UpdateMesh();
                //front
                adjSection = m_refWorld.GetSection(m_SecionSlot + new Vector3Int(0, 0, 1));
                if (adjSection != null) adjSection.UpdateMesh();
                //back
                adjSection = m_refWorld.GetSection(m_SecionSlot + new Vector3Int(0, 0, -1));
                if (adjSection != null) adjSection.UpdateMesh();

                isDirtry = false;
            }
        }
        private void OnBecameInvisible()
        {
            //gameObject.SetActive(false);
        }
        private void OnBecameVisible()
        {
            //gameObject.SetActive(true);
        }

        //function---------------------------------------------------
        public void GenerateSection(LayerData LayerData, int[,] HeightMap,int abs_y)
        {
            ushort width = m_refWorld.Section_Width;
            ushort height = m_refWorld.Section_Height;
            ushort depth = m_refWorld.Section_Depth;

            //Init Section space
            m_arrBlockID = new byte[width, height, depth];

            //init blockType 
            int x, y, z;
            for (x = 0; x < width; x++)
            {
                for (z = 0; z < depth; z++)
                {
                    for (y = 0; y < height; y++)
                    {
                        m_arrBlockID[x, y, z] = LayerData.GetBlockID(y+abs_y, HeightMap[x, z]);
                    }
                }
            }
            isDirtry = true;
        }

        public void UpdateMesh()
        {
            //if (isDirtry == false) return;
            m_MeshData.clear();
            m_MeshFilter.mesh.Clear();
            Block Curblk;

            int width = m_refWorld.Section_Width;
            int height = m_refWorld.Section_Height;
            int depth = m_refWorld.Section_Depth;

            int x, y, z;
            for (x = 0; x < width; x++)
            {
                for (y = 0; y < height; y++)
                {
                    for (z = 0; z < depth; z++)
                    {
                        Curblk = GetBlock(x, y, z);
                        if (Curblk == null) continue;
                        //check top block
                        _GetNonDuplicateMesh(Curblk, x, y, z, Direction.UP);
                        ////check bottom block
                        _GetNonDuplicateMesh(Curblk, x, y, z, Direction.DOWN);
                        ////check left block
                        _GetNonDuplicateMesh(Curblk, x, y, z, Direction.LEFT);
                        ////check right block
                        _GetNonDuplicateMesh(Curblk, x, y, z, Direction.RIGHT);
                        ////check forward block
                        _GetNonDuplicateMesh(Curblk, x, y, z, Direction.FORWARD);
                        ////check backward block
                        _GetNonDuplicateMesh(Curblk, x, y, z, Direction.BACKWARD);
                    }
                }
            }
            m_MeshData.ToMeshFilter(m_MeshFilter);
        }

        private void _GetNonDuplicateMesh(Block Curblk,int x,int y,int z,byte dir)
        {
            Vector3Int vt3dir = Direction.DirToVectorInt(dir);
            Block adj = GetBlock(x + vt3dir.x, y + vt3dir.y, z + vt3dir.z);
            if (adj == null || !adj.IsSolid(Direction.Opposite(dir)))
            {
                Curblk.ExtractMesh(dir, m_MeshData, x, y, z, m_refTexs);
            }
        }


        bool IsBlockInThisSection(int x, int y, int z,out Vector3Int offset)
        {
            if (x < 0)
            {
                offset = Vector3Int.left;
                return false;
            }
            if (x >= m_refWorld.Section_Width)
            {
                offset = Vector3Int.right;
                return false;
            }
            if (y < 0)
            {
                offset = Vector3Int.down;
                return false;
            }
            if (y >= m_refWorld.Section_Height)
            {
                offset = Vector3Int.up;
                return false;
            }
            if (z < 0)
            {
                offset = new Vector3Int(0, 0, -1);
                return false;
            }
            if (z >= m_refWorld.Section_Depth)
            {
                offset = new Vector3Int(0, 0, 1);
                return false;
            }

            offset = Vector3Int.zero;
            return true;
        }
        public Block GetBlock(int x, int y, int z)
        {
            Vector3Int offset;

            //Case: Target block in this Section
            if (IsBlockInThisSection(x, y, z, out offset))
            {               
                return m_refBlocks[m_arrBlockID[x, y, z]];
            }
            //Case: Target block out of this Section
            else
            {
                //Get adjacent Section
                Section adjSection = m_refWorld.GetSection(m_SecionSlot + offset);

                if (adjSection == null) return null;

                int relative_x = (m_refWorld.Section_Width + offset.x + x) % m_refWorld.Section_Width;
                int relative_y = (m_refWorld.Section_Height + offset.y + y) % m_refWorld.Section_Height;
                int relative_z = (m_refWorld.Section_Depth + offset.z + z) % m_refWorld.Section_Depth;

                return adjSection.GetBlock(relative_x, relative_y, relative_z);
            }
        }

        public byte GetBlockID(Vector3Int pos)
        {
            return m_arrBlockID[pos.x, pos.y, pos.z];
        }
        public void SetBlock(Vector3Int pos, byte blkID)
        {
            m_arrBlockID[pos.x,pos.y,pos.z] = blkID;
            isDirtry = true;
        }

    }

}
