using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.SMesh;
using Assets.Scripts.NServiceLocator;

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

        //private Chunk m_refChunk;
        private IWorld m_refWorld;

        [SerializeField]
        private SectionInWorld m_SecInWorld;

        [SerializeField]
        private byte[,,] m_arrBlockID= new byte[16, 16, 16];

        //unity component
        private MeshFilter m_MeshFilter;
        private MeshRenderer m_MeshRenderer;
        private MeshCollider m_Collider;

        //Temp Data
        private BlockInSection m_Cache_BlockInSec;

        //property--------------------------------------
        public bool isDirtry { get; set; }

        public SectionInWorld SectionInWorld { get { return m_SecInWorld; } set { m_SecInWorld = value; } }

        //unity function-----------------------------------------
        private void Awake()
        {
            m_refWorld = Locator<IWorld>.GetService();

            //cache unity component
            m_MeshFilter = gameObject.GetComponent<MeshFilter>();
            m_MeshRenderer = gameObject.GetComponent<MeshRenderer>();
            m_Collider = gameObject.GetComponent<MeshCollider>();

            //Set Material 
            m_MeshRenderer.materials = new Material[1];
            m_MeshRenderer.materials[0] = new Material(Shader.Find("Unlit/Texture"));
            if (m_MeshRenderer.materials[0] == null)
            {
                Debug.Log("Can't find Shader");
            }
            m_MeshRenderer.materials[0].mainTexture = m_refWorld.TexSheet.Tex;

            //make instance of mesh data 
            m_MeshData = ScriptableObject.CreateInstance<MeshData>();
            m_MeshData.Reset();
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
                adjSection = GWorldSearcher.GetSection(m_SecInWorld.Offset(Vector3Int.up), m_refWorld);
                if (adjSection != null) adjSection.UpdateMesh();
                //down
                adjSection = GWorldSearcher.GetSection(m_SecInWorld.Offset(Vector3Int.down), m_refWorld);
                if (adjSection != null) adjSection.UpdateMesh();
                //left
                adjSection = GWorldSearcher.GetSection(m_SecInWorld.Offset(Vector3Int.left), m_refWorld);
                if (adjSection != null) adjSection.UpdateMesh();
                //right
                adjSection = GWorldSearcher.GetSection(m_SecInWorld.Offset(Vector3Int.right), m_refWorld);
                if (adjSection != null) adjSection.UpdateMesh();
                //front
                adjSection = GWorldSearcher.GetSection(m_SecInWorld.Offset(new Vector3Int(0, 0, 1)), m_refWorld);
                if (adjSection != null) adjSection.UpdateMesh();
                //back
                adjSection = GWorldSearcher.GetSection(m_SecInWorld.Offset(new Vector3Int(0, 0, -1)), m_refWorld);
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
        public void GenerateBlankSection()
        {
            ushort width = m_refWorld.Section_Width;
            ushort height = m_refWorld.Section_Height;
            ushort depth = m_refWorld.Section_Depth;

            int x, y, z;
            for (x = 0; x < width; x++)
            {
                for (z = 0; z < depth; z++)
                {
                    for (y = 0; y < height; y++)
                    {
                        m_arrBlockID[x, y, z] = 0;
                    }
                }
            }
        }
        public void GenerateSection_ByLayer(LayerData LayerData, int[,] HeightMap,int abs_y)
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
                        Curblk = GetBlock(new BlockInSection(x,y,z,m_refWorld));
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
            Debug.Log("vertex "+ m_MeshData._Vertices.Count);

        }
        private void _GetNonDuplicateMesh(Block Curblk,int x,int y,int z,byte dir)
        {
            Vector3Int vt3dir = Direction.DirToVectorInt(dir);
            m_Cache_BlockInSec = new BlockInSection(x, y, z, m_refWorld);

            //Debug.Log("block in sec first:" + m_Cache_BlockInSec.Value);
            Vector3Int SectionOffset;
            m_Cache_BlockInSec.Move(vt3dir, m_refWorld, out SectionOffset);

            Debug.Log("sec offset :" + SectionOffset);
            //Debug.Log("block in sec then :" + m_Cache_BlockInSec.Value);
            //Block is located in This Section
            if (SectionOffset == Vector3Int.zero)
            {
                Block adj = GetBlock(m_Cache_BlockInSec);

                if (adj == null || !adj.IsSolid(Direction.Opposite(dir)))
                {
                    Debug.Log("extract this");
                    Curblk.ExtractMesh(dir, m_MeshData, x, y, z, m_refWorld.TexSheet);
                }
            }
            //Block is not located in This Section
            else
            {            
                //Search the section which this block located
                Section section = GWorldSearcher.GetSection(m_SecInWorld.Offset(SectionOffset), m_refWorld);
                if (section == null) return;

                Block adj = GetBlock(m_Cache_BlockInSec);

                if (adj == null || !adj.IsSolid(Direction.Opposite(dir)))
                {
                    Debug.Log("extract other");
                    Curblk.ExtractMesh(dir, m_MeshData, x, y, z, m_refWorld.TexSheet);
                }
            }
        }

        public Block GetBlock(BlockInSection blkInSec)
        {
            return m_refWorld.BlockTypes[m_arrBlockID[blkInSec.x, blkInSec.y, blkInSec.z]];
        }
        public byte GetBlockID(BlockInSection blkInSec)
        {
            return m_arrBlockID[blkInSec.x, blkInSec.y, blkInSec.z];
        }
        public void SetBlock(BlockInSection blkInSec, byte blkID)
        {
            m_arrBlockID[blkInSec.x, blkInSec.y, blkInSec.z] = blkID;
        }
    }

}
