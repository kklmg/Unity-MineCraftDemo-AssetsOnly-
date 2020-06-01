using UnityEngine;
using System.Collections;
using System.Threading;

using Assets.Scripts.NData;
using Assets.Scripts.NMesh;
using Assets.Scripts.NGlobal.ServiceLocator;

namespace Assets.Scripts.NWorld
{
    [RequireComponent(typeof(MeshFilter))]
    [RequireComponent(typeof(MeshRenderer))]

    public class Section : MonoBehaviour
    {
        //Field----------------------------------------
        [SerializeField]
        private DynamicMesh m_DynamicMesh;
        private Mesh m_UnityMesh;

        //private Chunk m_refChunk;
        private IWorld m_refWorld;

        //Location Data
        [SerializeField]
        private SectionInWorld m_SecInWorld;

        private int m_Width, m_Height, m_Depth;
        private BlockPalette m_Palette;

        [SerializeField]
        private byte[,,] m_arrBlockID = new byte[16, 16, 16];

        private object m_LockDynamicMesh;

        //unity component
        private MeshFilter m_MeshFilter;
        private MeshRenderer m_MeshRenderer;
        private MeshCollider m_Collider;

        //Temp Data
        private BlockInSection m_Cache_BlockInSec;

        Chunk m_Chunk; //the chunk this section belong to

        //property--------------------------------------
        public bool HasDMeshUpdated { private set; get; }

        public SectionInWorld SectionInWorld { get { return m_SecInWorld; } set { m_SecInWorld = value; } }

        //unity function-----------------------------------------
        private void Awake()
        {
            m_LockDynamicMesh = new object();

            m_refWorld = Locator<IWorld>.GetService();

            //cache unity component
            m_MeshFilter = gameObject.GetComponent<MeshFilter>();
            m_MeshRenderer = gameObject.GetComponent<MeshRenderer>();
            m_Collider = gameObject.GetComponent<MeshCollider>();

            m_Width = m_refWorld.Section_Width;
            m_Height = m_refWorld.Section_Height;
            m_Depth = m_refWorld.Section_Depth;
            m_Palette = m_refWorld.BlkPalette;

            //make instance of mesh data 
            m_DynamicMesh = new DynamicMesh();
            m_UnityMesh = new Mesh();
            m_DynamicMesh.Reset();
        }

        private void Update()
        {
            if (HasDMeshUpdated)
            {
                lock (m_LockDynamicMesh)
                {
                    //Generate Unity Mesh
                    m_MeshFilter.mesh = m_DynamicMesh.GenerateUnityMesh();
                    HasDMeshUpdated = false;
                }
            }
        }

        //Public function
        //------------------------------------------------------------------------------------------
        public void Init(SectionInWorld sectionInWorld)
        {
            m_Chunk = GetComponentInParent<Chunk>();

            //Init Location
            m_SecInWorld = sectionInWorld;    
            transform.localPosition = new Vector3(0, m_SecInWorld.Value.y * m_refWorld.Chunk_Height, 0);

            //set name
            transform.name = "Section" + '[' + m_SecInWorld.Value.y + ']';

            //Init Mesh
            m_DynamicMesh.Clear();
        }

        public void GenerateBlankSection()
        {
            int x, y, z;
            for (x = 0; x < m_Width; x++)
            {
                for (z = 0; z < m_Depth; z++)
                {
                    for (y = 0; y < m_Height; y++)
                    {
                        m_arrBlockID[x, y, z] = 0;
                    }
                }
            }

        }

        public void GenerateBlocks()
        {
            ChunkHeightMap heightMap = m_Chunk.GetHeightMap();
            LayerData layerData = m_Chunk.GetBiome().Layer;

            int OffsetHeight = m_SecInWorld.ToSectionInChunk().Value * m_refWorld.Section_Height;

            //init blockType 
            int x, y, z;
            Block tempBlk;

            for (x = 0; x < m_Width; x++)
            {
                for (y = 0; y < m_Height; y++)
                {
                    for (z = 0; z < m_Depth; z++)
                    {
                        tempBlk = layerData.GetBlock(y + OffsetHeight, heightMap[x, z]);
                        m_arrBlockID[x, y, z] = (tempBlk == null) ? byte.MinValue : tempBlk.ID;
                    }
                }
            }
        }

        //build mesh 
        public void BuildMesh()
        {
            lock (m_LockDynamicMesh)
            {
                //Clear Current Mesh 
                m_DynamicMesh.Clear();
                GSW.RestartTimer();

                IBlock CurBlk, AdjBlk;
                int x, y, z;
                for (x = 0; x < m_Width; x++)
                {
                    for (y = 0; y < m_Height; y++)
                    {
                        for (z = 0; z < m_Depth; z++)
                        {
                            //get block type
                            CurBlk = m_Palette[m_arrBlockID[x, y, z]];
                            if (CurBlk == null) continue;

                            //case: Block is not opaque
                            if (!CurBlk.IsOpaque)
                            {
                                m_DynamicMesh.Add(CurBlk.GetAllMesh(x, y, z));
                            }
                            else
                            {
                                //Check Up side
                                AdjBlk = _GetBlock(x, y + 1, z);
                                if (AdjBlk == null || !AdjBlk.IsOpaque || !AdjBlk.IsDownMeshExist)
                                {
                                    m_DynamicMesh.Add(CurBlk.GetUpMesh(x, y, z));
                                }

                                //Check Down side
                                AdjBlk = _GetBlock(x, y - 1, z);
                                if (AdjBlk == null || !AdjBlk.IsOpaque || !AdjBlk.IsUpMeshExist)
                                {
                                    m_DynamicMesh.Add(CurBlk.GetDownMesh(x, y, z));
                                }

                                //Check Left side
                                AdjBlk = _GetBlock(x - 1, y, z);
                                if (AdjBlk == null || !AdjBlk.IsOpaque || !AdjBlk.IsRigthMeshExist)
                                {
                                    m_DynamicMesh.Add(CurBlk.GetLeftMesh(x, y, z));
                                }

                                //Check Right side
                                AdjBlk = _GetBlock(x + 1, y, z);
                                if (AdjBlk == null || !AdjBlk.IsOpaque || !AdjBlk.IsLeftMeshExist)
                                {
                                    m_DynamicMesh.Add(CurBlk.GetRightMesh(x, y, z));
                                }

                                //Check Front side
                                AdjBlk = _GetBlock(x, y, z + 1);
                                if (AdjBlk == null || !AdjBlk.IsOpaque || !AdjBlk.IsDownMeshExist)
                                {
                                    m_DynamicMesh.Add(CurBlk.GetFrontMesh(x, y, z));
                                }

                                //Check Back side
                                AdjBlk = _GetBlock(x, y, z - 1);
                                if (AdjBlk == null || !AdjBlk.IsOpaque || !AdjBlk.IsDownMeshExist)
                                {
                                    m_DynamicMesh.Add(CurBlk.GetBackMesh(x, y, z));
                                }
                            }
                        }//z
                    }//y
                }//x

                HasDMeshUpdated = true;
                GSW.ShowElapsedTime("Section Mesh Building");
            }//lock
        }
  


        private IBlock _GetBlock(int x, int y, int z)
        {
            if (x < 0 || y < 0 || z < 0 ||
                x >= m_Width || y >= m_Height || z >= m_Depth) return null;
            else return m_Palette[m_arrBlockID[x, y, z]];
        }

        public IBlock GetBlock(BlockInSection blkInSec)
        {
            return m_Palette[m_arrBlockID[blkInSec.x, blkInSec.y, blkInSec.z]];
        }
        public byte GetBlockID(BlockInSection blkInSec)
        {
            return m_arrBlockID[blkInSec.x, blkInSec.y, blkInSec.z];
        }

        public void SetBlock(BlockInSection blkInSec, byte blkID, bool Refresh = true)
        {
            m_arrBlockID[blkInSec.x, blkInSec.y, blkInSec.z] = blkID;
            if (Refresh == true) BuildMesh();
        }

        public void ClearUnityMesh()
        {
            m_MeshFilter.mesh.Clear();
            m_MeshFilter.mesh = null;
        }
    }
}