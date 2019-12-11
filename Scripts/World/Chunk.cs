//using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Assets.Scripts.SMesh;

namespace Assets.Scripts.World
{
    [RequireComponent(typeof(MeshFilter))]
    [RequireComponent(typeof(MeshRenderer))]
    [RequireComponent(typeof(MeshCollider))]

    class Chunk : MonoBehaviour
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
        private void Start()
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

            //Get chunk size
            int width = m_refWorld.C_WIDTH;
            int height = m_refWorld.C_HEIGHT;
            int depth = m_refWorld.C_DEPTH;



            //Init chunk space
            m_arrBlockID = new byte[width, height, depth];

            int x, y, z;
            for (x = 0; x < width; x++)
            {
                for (y = 0; y < height; y++)
                {
                    for (z = 0; z < depth; z++)
                    {
                        //BlockMeshBase mm = new BlockMeshSolid();
                        //m_arrBlocks[x, y, z] = ScriptableObject.CreateInstance<Block>();
                        //m_arrBlockID[x, y, z] = 0;
                        m_arrBlockID[x, y, z] = (byte) Random.Range (0, m_refBlocks.Count);


                        //m_arrBlocks[x, y, z].GetMesh().ExtractMeshAll(m_MeshData, x, y, z);
                        //m_arrBlocks[x, y, z].GetMesh().ExtractMesh(eDirection.forward,m_MeshData,x,y,z);
                        //m_arrBlocks[x, y, z].GetMesh().ExtractMesh((eDirection)1, m_MeshData, x, y, z);
                        //m_arrBlocks[x, y, z].GetMesh().ExtractMesh((eDirection)2, m_MeshData, x, y, z);
                        //m_arrBlocks[x, y, z].GetMesh().ExtractMesh((eDirection)3, m_MeshData, x, y, z);
                        //m_arrBlocks[x, y, z].GetMesh().ExtractMesh((eDirection)4, m_MeshData, x, y, z);
                        //m_arrBlocks[x, y, z].GetMesh().ExtractMesh((eDirection)5, m_MeshData, x, y, z);
                    }
                }
            }
            UpdateChunk();


            m_MeshData.ToMeshFilter(m_MeshFilter);
            //Debug.Log(m_MeshFilter.mesh.);
            // UpdateChunk();
        }
        private void Update()
        {
            //transform.Rotate(Vector3.up, 45 * Time.deltaTime);
            //transform.Rotate(Vector3.right, 45 * Time.deltaTime);
            //transform.Rotate(Vector3.forward, 45 * Time.deltaTime);
        }


        //function---------------------------------------------------
        void UpdateChunk()
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
