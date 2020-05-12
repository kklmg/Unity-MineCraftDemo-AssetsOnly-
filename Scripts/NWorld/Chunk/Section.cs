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
        public bool isMeshDirty { get; set; }
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

                    Debug.Log("late build"+m_DynamicMesh._Vertices.Count);

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
            ClearMesh();
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
            Block tempblk;

            for (x = 0; x < m_Width; x++)
            {
                for (y = 0; y < m_Height; y++)
                {
                    for (z = 0; z < m_Depth; z++)
                    {
                        tempblk = layerData.GetBlock(y + OffsetHeight, heightMap[x, z]);
                        m_arrBlockID[x, y, z] = m_Palette.GetBlockID(tempblk);
                    }
                }
            }
        }

        //build mesh 
        public void BuildMesh()
        {
            Debug.Log(m_Palette.Count);
            lock (m_LockDynamicMesh)
            {
                GSW.RestartTimer();
                GSW.ShowElapsedTime("ready");

                //Clear Current Mesh 
                m_DynamicMesh.Clear();

                TextureSheet TexSheet = m_refWorld.TexSheet;

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
                                m_DynamicMesh.Add(CurBlk.GetAllMesh(TexSheet, x, y, z));
                            }
                            else
                            {
                                //Check Up side
                                AdjBlk = _GetBlock(x, y + 1, z);
                                if (AdjBlk == null || !AdjBlk.IsDownMeshExist)
                                {
                                    m_DynamicMesh.Add(CurBlk.GetUpMesh(TexSheet, x, y, z));
                                }

                                //Check Down side
                                AdjBlk = _GetBlock(x, y - 1, z);
                                if (AdjBlk == null || !AdjBlk.IsUpMeshExist)
                                {
                                    m_DynamicMesh.Add(CurBlk.GetDownMesh(TexSheet, x, y, z));
                                }

                                //Check Left side
                                AdjBlk = _GetBlock(x - 1, y, z);
                                if (AdjBlk == null || !AdjBlk.IsRigthMeshExist)
                                {
                                    m_DynamicMesh.Add(CurBlk.GetLeftMesh(TexSheet, x, y, z));
                                }

                                //Check Right side
                                AdjBlk = _GetBlock(x + 1, y, z);
                                if (AdjBlk == null || !AdjBlk.IsLeftMeshExist)
                                {
                                    m_DynamicMesh.Add(CurBlk.GetRightMesh(TexSheet, x, y, z));
                                }

                                //Check Front side
                                AdjBlk = _GetBlock(x, y, z + 1);
                                if (AdjBlk == null || !AdjBlk.IsDownMeshExist)
                                {
                                    m_DynamicMesh.Add(CurBlk.GetFrontMesh(TexSheet, x, y, z));
                                }

                                //Check Back side
                                AdjBlk = _GetBlock(x, y, z - 1);
                                if (AdjBlk == null || !AdjBlk.IsDownMeshExist)
                                {
                                    m_DynamicMesh.Add(CurBlk.GetBackMesh(TexSheet, x, y, z));
                                }
                            }
                        }//z
                    }//y
                }//x

                HasDMeshUpdated = true;
            }//lock
        }
   
        public void ClearMesh()
        {
            m_MeshFilter.mesh.Clear();
            m_DynamicMesh.Clear();
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
    }
}



////Back Up Function 
//using UnityEngine;
//using System.Collections;
//using System.Threading;

//using Assets.Scripts.NData;
//using Assets.Scripts.NMesh;
//using Assets.Scripts.NGlobal.ServiceLocator;

//namespace Assets.Scripts.NWorld
//{
//    [RequireComponent(typeof(MeshFilter))]
//    [RequireComponent(typeof(MeshRenderer))]

//    public class Section : MonoBehaviour, IMeshObj
//    {
//        //Field----------------------------------------
//        [SerializeField]
//        private DynamicMesh m_MeshDynamic;
//        private Mesh m_UnityMesh;

//        //private Chunk m_refChunk;
//        private IWorld m_refWorld;

//        //Location Data
//        [SerializeField]
//        private SectionInWorld m_SecInWorld;

//        int m_Width, m_Height, m_Depth;

//        [SerializeField]
//        private byte[,,] m_arrBlockID = new byte[16, 16, 16];

//        private object m_LockDynamicMesh;

//        //unity component
//        private MeshFilter m_MeshFilter;
//        private MeshRenderer m_MeshRenderer;
//        private MeshCollider m_Collider;

//        //Temp Data
//        private BlockInSection m_Cache_BlockInSec;

//        Chunk m_Chunk; //the chunk this section belong to

//        //property--------------------------------------
//        public bool isMeshDirty { get; set; }
//        public bool HasDMeshUpdated { private set; get; }

//        public SectionInWorld SectionInWorld { get { return m_SecInWorld; } set { m_SecInWorld = value; } }

//        //unity function-----------------------------------------
//        private void Awake()
//        {
//            m_LockDynamicMesh = new object();

//            m_refWorld = Locator<IWorld>.GetService();

//            //cache unity component
//            m_MeshFilter = gameObject.GetComponent<MeshFilter>();
//            m_MeshRenderer = gameObject.GetComponent<MeshRenderer>();
//            m_Collider = gameObject.GetComponent<MeshCollider>();

//            m_Width = m_refWorld.Section_Width;
//            m_Height = m_refWorld.Section_Height;
//            m_Depth = m_refWorld.Section_Depth;

//            //make instance of mesh data 
//            m_MeshDynamic = new DynamicMesh();
//            m_UnityMesh = new Mesh();
//            m_MeshDynamic.Reset();
//        }

//        private void Update()
//        {
//            if (HasDMeshUpdated)
//            {
//                lock (m_LockDynamicMesh)
//                {
//                    //Generate Unity Mesh
//                    m_MeshFilter.mesh = m_MeshDynamic.GenerateUnityMesh();
//                    HasDMeshUpdated = false;
//                }
//            }
//        }

//        //Public function
//        //------------------------------------------------------------------------------------------
//        public void Init(SectionInWorld sectionInWorld)
//        {
//            m_Chunk = GetComponentInParent<Chunk>();

//            //Init Location
//            m_SecInWorld = sectionInWorld;
//            transform.localPosition = new Vector3(0, m_SecInWorld.Value.y * m_refWorld.Chunk_Height, 0);

//            //set name
//            transform.name = "Section" + '[' + m_SecInWorld.Value.y + ']';

//            //Init Mesh
//            ClearMesh();
//        }

//        public void GenerateBlankSection()
//        {
//            int x, y, z;
//            for (x = 0; x < m_Width; x++)
//            {
//                for (z = 0; z < m_Depth; z++)
//                {
//                    for (y = 0; y < m_Height; y++)
//                    {
//                        m_arrBlockID[x, y, z] = 0;
//                    }
//                }
//            }

//        }

//        public void GenerateBlocks()
//        {
//            ChunkHeightMap heightMap = m_Chunk.GetHeightMap();
//            LayerData layerData = m_Chunk.GetBiome().Layer;

//            int OffsetHeight = m_SecInWorld.ToSectionInChunk().Value * m_refWorld.Section_Height;

//            //init blockType 
//            int x, y, z;
//            Block tempblk;
//            BlockPalette plaette = Locator<IWorld>.GetService().BlkPalette;

//            int i = 0;
//            for (x = 0; x < m_Width; x++)
//            {
//                for (y = 0; y < m_Height; y++)
//                {
//                    for (z = 0; z < m_Depth; z++)
//                    {
//                        tempblk = layerData.GetBlock(y + OffsetHeight, heightMap[x, z]);
//                        m_arrBlockID[x, y, z] = plaette.GetBlockID(tempblk);
//                    }
//                }
//            }
//        }

//        //build mesh 
//        public void BuildMeshInstantly()
//        {
//            lock (m_LockDynamicMesh)
//            {
//                GSW.RestartTimer();
//                GSW.ShowElapsedTime("ready");

//                //Clear Current Mesh 
//                m_MeshDynamic.Clear();

//                int width = m_refWorld.Section_Width;
//                int height = m_refWorld.Section_Height;
//                int depth = m_refWorld.Section_Depth;

//                int x, y, z;
//                for (x = 0; x < width; x++)
//                {
//                    for (y = 0; y < height; y++)
//                    {
//                        for (z = 0; z < depth; z++)
//                        {
//                            _GetMesh(x, y, z);
//                        }
//                    }
//                }
//                HasDMeshUpdated = true;

//                GSW.ShowElapsedTime("mesh completed");
//            }


//        }

//        public void BuildMeshInBackground()
//        {
//            //GTimer.RestartTimer();
//            ////instantiate new thread
//            //m_TGenerateDMesh = new Thread(new ThreadStart(Thrread_GenerateDynamicMesh));
//            //m_TGenerateDMesh.Priority = System.Threading.ThreadPriority.Lowest;
//            //m_TGenerateDMesh.IsBackground = true;


//            //GTimer.ShowElapsedTimeAndRestart("instantiate thread");

//            //m_TGenerateDMesh.Start();

//            //GTimer.ShowElapsedTimeAndRestart(m_TGenerateDMesh.ManagedThreadId + "start thread "+ Time.deltaTime);

//            //Debug.Log("Thread ID " + m_TGenerateDMesh.ManagedThreadId + " start");
//            //Debug.Log("Frame " + Time.deltaTime);
//        }

//        public void Thrread_GenerateDynamicMesh()
//        {
//            lock (m_LockDynamicMesh)
//            {
//                m_MeshDynamic.Clear();

//                int width = m_refWorld.Section_Width;
//                int height = m_refWorld.Section_Height;
//                int depth = m_refWorld.Section_Depth;

//                int x, y, z;
//                for (x = 0; x < width; x++)
//                {
//                    for (y = 0; y < height; y++)
//                    {
//                        for (z = 0; z < depth; z++)
//                        {
//                            _GetMesh(x, y, z);
//                        }
//                    }
//                }
//            }
//            HasDMeshUpdated = true;

//            //  Debug.Log("Thread ID " + m_TGenerateDMesh.ManagedThreadId + " completed");
//        }

//        public void RunBuildMeshCoro()
//        {
//            StopAllCoroutines();
//            IEnumerator Coro = BuildMesh_Coro();
//            StartCoroutine(Coro);
//        }

//        public IEnumerator BuildMesh_Coro()
//        {
//            m_MeshRenderer.enabled = false;
//            //Clear Current Mesh 
//            m_MeshFilter.mesh.Clear();
//            m_MeshDynamic.Clear();

//            yield return null;

//            int width = m_refWorld.Section_Width;
//            int height = m_refWorld.Section_Height;
//            int depth = m_refWorld.Section_Depth;

//            int x, y, z;
//            for (x = 0; x < width; x++)
//            {
//                for (y = 0; y < height; y++)
//                {
//                    for (z = 0; z < depth; z++)
//                    {
//                        _GetMesh(x, y, z);
//                    }
//                }
//            }
//            yield return null;


//            yield return m_MeshDynamic.SaveToUnityMesh_Coro(m_MeshFilter.mesh);

//            m_MeshRenderer.enabled = true;
//            yield return null;
//        }

//        public void ClearMesh()
//        {
//            m_MeshFilter.mesh.Clear();
//            m_MeshDynamic.Clear();
//        }

//        //public Getter
//        private void _GetMesh(int x, int y, int z)
//        {
//            m_Cache_BlockInSec = new BlockInSection(x, y, z, m_refWorld);
//            Block Curblk = GetBlock(m_Cache_BlockInSec);

//            if (Curblk == null) return;

//            //check top block
//            _GetMesh_ByDir(ref m_Cache_BlockInSec, Curblk, Direction.UP);
//            //check bottom block
//            _GetMesh_ByDir(ref m_Cache_BlockInSec, Curblk, Direction.DOWN);
//            //check left block
//            _GetMesh_ByDir(ref m_Cache_BlockInSec, Curblk, Direction.LEFT);
//            //check right block
//            _GetMesh_ByDir(ref m_Cache_BlockInSec, Curblk, Direction.RIGHT);
//            //check forward block
//            _GetMesh_ByDir(ref m_Cache_BlockInSec, Curblk, Direction.FORWARD);
//            //check backward block
//            _GetMesh_ByDir(ref m_Cache_BlockInSec, Curblk, Direction.BACKWARD);

//        }
//        private void _GetMesh_ByDir(ref BlockInSection Curlocation, Block blkType, byte dir)
//        {
//            Vector3Int SectionOffset;
//            BlockInSection AdjLocation = Curlocation.Offset(Direction.DirToVectorInt(dir), m_refWorld, out SectionOffset);

//            if (SectionOffset == Vector3Int.zero)
//            {
//                Block adj = GetBlock(AdjLocation);
//                byte opdir = Direction.Opposite(dir);

//                if (adj == null || !adj.IsSolid(opdir) || adj.IsTransparent(opdir))
//                {
//                    blkType.ExtractMesh(dir, m_MeshDynamic, ref Curlocation, m_refWorld.TexSheet);
//                }
//            }
//            //Block is not located in This Section
//            else
//            {
//                blkType.ExtractMesh(dir, m_MeshDynamic, ref Curlocation, m_refWorld.TexSheet);
//            }
//        }

//        public Block GetBlock(BlockInSection blkInSec)
//        {
//            return m_refWorld.BlkPalette[m_arrBlockID[blkInSec.x, blkInSec.y, blkInSec.z]];
//        }
//        public byte GetBlockID(BlockInSection blkInSec)
//        {
//            return m_arrBlockID[blkInSec.x, blkInSec.y, blkInSec.z];
//        }

//        public void SetBlock(BlockInSection blkInSec, byte blkID, bool Refresh = true)
//        {
//            m_arrBlockID[blkInSec.x, blkInSec.y, blkInSec.z] = blkID;
//            if (Refresh == true) BuildMeshInstantly();
//        }

//    }
//}



////Back Up Function 


