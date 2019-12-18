//using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Assets.Scripts.SMesh;
using MyNoise.Perlin;

namespace Assets.Scripts.World
{
    [RequireComponent(typeof(MeshFilter))]
    [RequireComponent(typeof(MeshRenderer))]
    [RequireComponent(typeof(MeshCollider))]

    public class Chunk : MonoBehaviour
    {
        //Field----------------------------------------
        private MeshData m_MeshData;

        private World m_refWorld;
        private List<Block> m_refBlocks;
        private TextureSheet m_refTexs;

        [SerializeField]
        private Vector3Int m_PosWorld;

        [SerializeField]
        private byte[,,] m_arrBlockID;

        //unity component
        MeshFilter m_MeshFilter;
        MeshRenderer m_MeshRenderer;
        MeshCollider m_Collider;
       
        //property--------------------------------------
        public bool isDirtry { get; set; }

        public Vector3Int WorldPos { get { return m_PosWorld; } set { m_PosWorld = value; } }

        //unity function-----------------------------------------
        private void Awake()
        {
            //Debug.Log("Chunk_start");

            //get unity component
            m_MeshFilter = gameObject.GetComponent<MeshFilter>();
            m_MeshRenderer = gameObject.GetComponent<MeshRenderer>();
            m_Collider = gameObject.GetComponent<MeshCollider>();

            //save World reference
            m_refWorld = GetComponentInParent<World>();
            m_refBlocks = m_refWorld.BlockList;

            //Debug.Log("----------------------------------------------");
            //Debug.Log("block list size : " + m_refBlocks.Count);
            //Debug.Log("----------------------------------------------");

            m_refTexs = m_refWorld.TexSheet;

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
            GenerateWorld();


        }
        private void Start()
        {
            UpdateMesh();
            m_MeshData.ToMeshFilter(m_MeshFilter);
        }
        private void Update()
        {
            //transform.Rotate(Vector3.up, 45 * Time.deltaTime);
            //transform.Rotate(Vector3.right, 45 * Time.deltaTime);
            //transform.Rotate(Vector3.forward, 45 * Time.deltaTime);
        }


        //function---------------------------------------------------
        void GenerateWorld()
        {

            //Init chunk space
            m_arrBlockID = new byte[m_refWorld.C_WIDTH, m_refWorld.C_HEIGHT, m_refWorld.C_DEPTH];

            //chunk's absolute coordinate range
            int abs_x_min = WorldPos.x * m_refWorld.C_WIDTH;
            int abs_y_min = WorldPos.y * m_refWorld.C_HEIGHT;
            int abs_z_min = WorldPos.z * m_refWorld.C_DEPTH;

            int abs_x_max = abs_x_min + m_refWorld.C_WIDTH;
            int abs_y_max = abs_y_min + m_refWorld.C_HEIGHT;
            int abs_z_max = abs_z_min + m_refWorld.C_DEPTH;

            //World's Total size
            uint t_width = m_refWorld.TOTAL_WIDTH;
            uint t_depth = m_refWorld.TOTAL_DEPTH;
            uint t_height = m_refWorld.TOTAL_HEIGHT;

            PerlinNoiseMaker NoiseMaker = new PerlinNoiseMaker();
            

            int x, y, z;
            for (x = abs_x_min; x < abs_x_max; x++)
            {   
                //for (y = 0; y < height; y++)
                //{
                for (z = abs_z_min; z < abs_z_max; z++)
                {
                    int maxheight = (int)(t_height * Mathf.PerlinNoise((float)(x) /13.0f, (float)(z) /13.0f));
                    //int maxheight = (int)(t_height * NoiseMaker.GetNoise_2D_abs(new Vector2((float)x/13, (float)z/13)));
                    //int maxheight = (int)(t_height * NoiseMaker.GetOctaveNoise_2D(new Vector2((float)(x) / t_width, (float)(z) / t_depth), 1.0f / t_width, 128.0f, 8));

                    //Debug.Log("x : " + x / width);
                    //Debug.Log("z : " + z / depth);
                    //Debug.Log("y : " + maxheight);

                    for (y = abs_y_min; y < abs_y_max; y++)
                    {
                        if(y < maxheight)
                        m_arrBlockID[x% m_refWorld.C_WIDTH, y% m_refWorld.C_HEIGHT, z% m_refWorld.C_DEPTH] = 1;


                        //BlockMeshBase mm = new BlockMeshSolid();
                        //m_arrBlocks[x, y, z] = ScriptableObject.CreateInstance<Block>();
                        //m_arrBlockID[x, y, z] = 1;
                        //m_arrBlockID[x, y, z] = (byte)Random.Range(0, m_refBlocks.Count);
                    }
                }
                //}
            }
           
        }

        void GenerateWorld(Biome _biome,int x,int y,int z,int[,] HeightMap,IEnumerator<BlockLayer> iter)
        {      
            for (x = 0; x < abs_x_max; x++)
            {
                for (z = abs_z_min; z < abs_z_max; z++)
                {
                    for (y = abs_y_min; y < abs_y_max; y++)
                    {
                       
                        m_arrBlockID[x % m_refWorld.C_WIDTH, y % m_refWorld.C_HEIGHT, z % m_refWorld.C_DEPTH] = 1;


     
                    }
                }
                //}
            }




            //Init chunk space
            m_arrBlockID = new byte[m_refWorld.C_WIDTH, m_refWorld.C_HEIGHT, m_refWorld.C_DEPTH];

            //chunk's absolute coordinate range
            int abs_x_min = WorldPos.x * m_refWorld.C_WIDTH;
            int abs_y_min = WorldPos.y * m_refWorld.C_HEIGHT;
            int abs_z_min = WorldPos.z * m_refWorld.C_DEPTH;

            int abs_x_max = abs_x_min + m_refWorld.C_WIDTH;
            int abs_y_max = abs_y_min + m_refWorld.C_HEIGHT;
            int abs_z_max = abs_z_min + m_refWorld.C_DEPTH;

            //World's Total size
            uint t_width = m_refWorld.TOTAL_WIDTH;
            uint t_depth = m_refWorld.TOTAL_DEPTH;
            uint t_height = m_refWorld.TOTAL_HEIGHT;

            PerlinNoiseMaker NoiseMaker = new PerlinNoiseMaker();


          

        }

        void UpdateMesh()
        {
            //if (isDirtry == false) return;
            
            Block cur, adj;

            int width = m_refWorld.C_WIDTH;
            int height = m_refWorld.C_HEIGHT;
            int depth = m_refWorld.C_DEPTH;

            int x, y, z;
            for (x = 0; x < width; x++)
            {
                for (y = 0; y < height; y++)
                {
                    for (z = 0; z < depth; z++)
                    {
                        cur = GetBlock(x, y, z);

                        if (cur == null) continue;

                        //check above block
                        adj = GetBlock(x, y + 1, z);

                        if (adj == null || adj.IsSolid(eDirection.down) == false)
                        {
                            cur.ExtractMesh(eDirection.up, m_MeshData, x, y, z, m_refTexs);
                        }

                        //check below block
                        adj = GetBlock(x, y - 1, z);
                        if (adj == null || adj.IsSolid(eDirection.up) == false)
                        {
                            cur.ExtractMesh(eDirection.down, m_MeshData, x, y, z, m_refTexs);
                        }

                        //check left block
                        adj = GetBlock(x - 1, y, z);
                        if (adj == null || adj.IsSolid(eDirection.right) == false)
                        {
                            cur.ExtractMesh(eDirection.left, m_MeshData, x, y, z, m_refTexs);
                        }

                        //check right block
                        adj = GetBlock(x + 1, y, z);
                        if (adj == null || adj.IsSolid(eDirection.left) == false)
                        {
                            cur.ExtractMesh(eDirection.right, m_MeshData, x, y, z, m_refTexs);
                        }

                        //check forward block
                        adj = GetBlock(x, y, z + 1);
                        if (adj == null || adj.IsSolid(eDirection.backward) == false)
                        {
                            cur.ExtractMesh(eDirection.forward, m_MeshData, x, y, z, m_refTexs);
                        }

                        //check backward block
                        adj = GetBlock(x, y, z - 1);
                        if (adj == null || adj.IsSolid(eDirection.forward) == false)
                        {
                            cur.ExtractMesh(eDirection.backward, m_MeshData, x, y, z, m_refTexs);
                        }
                    }
                }
            }
        }


        void GetMesh(int x, int y, int z)
        {
            //Block adj = GetBlock(x, y - 1, z);
            ////check above block
            //adj = 

            //if (adj == null) { }
            //if (adj != null && adj.GetMesh().IsSolid(eDirection.down) == false)
            //{
            //    cur.GetMesh().ExtractMesh(eDirection.up, m_MeshData, x, y, z);
            //}
        }

        bool BlockInThisChunk(int x, int y, int z,out Vector3Int offset)
        {
            if (x < 0)
            {
                offset = Vector3Int.left;
                return false;
            }
            if (x >= m_refWorld.C_WIDTH)
            {
                offset = Vector3Int.right;
                return false;
            }
            if (y < 0)
            {
                offset = Vector3Int.down;
                return false;
            }
            if (y >= m_refWorld.C_HEIGHT)
            {
                offset = Vector3Int.up;
                return false;
            }
            if (z < 0)
            {
                offset = new Vector3Int(0, 0, -1);
                return false;
            }
            if (z >= m_refWorld.C_DEPTH)
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

            //Case: Target block in this chunk
            if (BlockInThisChunk(x, y, z, out offset))
            {
                return m_refBlocks[m_arrBlockID[x, y, z]];
            }
            //Case: Target block out of this chunk
            else
            {
                //Get adjacent chunk
                Chunk adjChunk = m_refWorld.GetChunk(m_PosWorld + offset);

                if (adjChunk == null) return null;

                int relative_x = (m_refWorld.C_WIDTH + offset.x) % m_refWorld.C_WIDTH;
                int relative_y = (m_refWorld.C_HEIGHT + offset.y) % m_refWorld.C_HEIGHT;
                int relative_z = (m_refWorld.C_DEPTH + offset.z) % m_refWorld.C_DEPTH;

                return GetBlock(relative_x, relative_y, relative_z);
            }
        }

        void updateOneBlock(int x, int y, int z)
        {

        }

    }

}
