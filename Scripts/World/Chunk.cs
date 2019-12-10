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
       
        public ushort WIDTH = 16;
        public ushort HEIGHT = 16;
        public ushort DEPTH = 16;

        MeshFilter m_MeshFilter;
        MeshCollider m_Collider;
        //World m_World;

        MeshData m_MeshData;
        public bool isDirtry { get; set; }

        [SerializeField]
        public Block blk;

        public List<Block> M_refBlocks;
        public byte[,,] m_arrBlockID;

        public int a;
        Chunk() { }

        private void Awake()
        {
            m_MeshFilter = gameObject.GetComponent<MeshFilter>();
            m_Collider = gameObject.GetComponent<MeshCollider>();
            M_refBlocks = GetComponentInParent<World>().BlockList;

            m_arrBlockID = new byte[WIDTH, HEIGHT, DEPTH];
            m_MeshData = ScriptableObject.CreateInstance<MeshData>();


            
            int x, y, z;
            for (x = 0; x < WIDTH; x++)
            {
                for (y = 0; y < HEIGHT; y++)
                {
                    for (z = 0; z < DEPTH; z++)
                    {
                        //BlockMeshBase mm = new BlockMeshSolid();
                        //m_arrBlocks[x, y, z] = ScriptableObject.CreateInstance<Block>();
                        m_arrBlockID[x, y, z] = (byte) Random.Range (0, M_refBlocks.Count);


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


        void UpdateChunk()
        {
            //if (isDirtry == false) return;

            //ChunkMesh NewMesh = new ChunkMesh();

            Block cur, adj;
            int x, y, z;
            for (x = 0; x < WIDTH; x++)
            {
                for (y = 0; y < HEIGHT; y++)
                {
                    for (z = 0; z < DEPTH; z++)
                    {
                        cur = GetBlock(x, y, z);

                        if (cur == null) continue;

                        //check above block
                        adj = GetBlock(x, y + 1, z);

                        if (adj == null || adj.IsSolid(eDirection.down) == false)
                        {
                            cur.ExtractMesh(eDirection.up, m_MeshData, x, y, z);
                        }

                        //check below block
                        adj = GetBlock(x, y - 1, z);
                        if (adj == null || adj.IsSolid(eDirection.up) == false)
                        {
                            cur.ExtractMesh(eDirection.down, m_MeshData, x, y, z);
                        }

                        //check left block
                        adj = GetBlock(x - 1, y, z);
                        if (adj == null || adj.IsSolid(eDirection.right) == false)
                        {
                            cur.ExtractMesh(eDirection.left, m_MeshData, x, y, z);
                        }

                        //check right block
                        adj = GetBlock(x + 1, y, z);
                        if (adj == null || adj.IsSolid(eDirection.left) == false)
                        {
                            cur.ExtractMesh(eDirection.right, m_MeshData, x, y, z);
                        }

                        //check forward block
                        adj = GetBlock(x, y, z + 1);
                        if (adj == null || adj.IsSolid(eDirection.backward) == false)
                        {
                            cur.ExtractMesh(eDirection.forward, m_MeshData, x, y, z);
                        }

                        //check backward block
                        adj = GetBlock(x, y, z - 1);
                        if (adj == null || adj.IsSolid(eDirection.forward) == false)
                        {
                            cur.ExtractMesh(eDirection.backward, m_MeshData, x, y, z);
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

        bool BlockInThisChunk(int x, int y, int z)
        {
            return (0 <= x && x < WIDTH && 0 <= y && y < HEIGHT && 0 <= z && z < DEPTH);
        }



        public Block GetBlock(int x, int y, int z)
        {
            if (BlockInThisChunk(x, y, z))
            {
                return M_refBlocks[m_arrBlockID[x, y, z]];
            }
            //else if (searchnearby)

            else return null;
        }


        void updateOneBlock(int x, int y, int z)
        {

        }

    }

}
