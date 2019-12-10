using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Assets.Scripts.SMesh;
using Assets.Scripts.Pattern;


namespace Assets.Scripts.World
{
    public enum eDirection { up, down, left, right, forward, backward };

    public abstract class BlockMeshBase : ScriptableObject
    {
        public abstract bool IsSolid(eDirection dir);
        public abstract bool isOverlap(BlockMeshBase mesh);


        byte mask;

        public abstract void ExtractMeshAll(MeshData receiver, int x, int y, int z);
        public abstract void ExtractMesh(eDirection dir, MeshData receiver, int x, int y, int z);
    }


    [CreateAssetMenu(menuName = "blockMesh")]
    class BlockMeshSolidtest : BlockMeshBase
    {
        [SerializeField]
        private Tile[] m_arrTiles;

        [SerializeField]
        MeshData[] m_arrQuad;

        public override bool IsSolid(eDirection dir)
        {
            return true;
        }
        public override bool isOverlap(BlockMeshBase mesh)
        {
            return true;
        }

        public override void ExtractMesh(eDirection dir, MeshData receiver, int x, int y, int z)
        {
            MeshData mesh = m_arrTiles[(int)dir].pyMesh.Clone();
            mesh.Translate(Vector3.right, x);
            mesh.Translate(Vector3.up, y);
            mesh.Translate(Vector3.back, z);

            receiver.add(mesh);
        }


        public override void ExtractMeshAll(MeshData receiver, int x, int y, int z)
        {
            foreach (Tile t in m_arrTiles)
            {
                if (t == null) continue;
                MeshData mesh = t.pyMesh.Clone();
                mesh.Translate(Vector3.right, x);
                mesh.Translate(Vector3.up, y);
                mesh.Translate(Vector3.back, z);


                receiver.add(mesh);
            }
        }
    }

    
    class BlockMeshSolid : BlockMeshBase
    {
        private Tile[] m_arrTiles;
        private MeshData mesh;
        public BlockMeshSolid()
        {
            m_arrTiles = new Tile[6];
            BlockSize  Bsize= MonoSingleton<BlockManager>.Instance.m_BlockSize;
            //MonoSingleton<BlockManager>.Instance
            
            m_arrTiles[(int)eDirection.backward] = new Tile();
            m_arrTiles[(int)eDirection.backward].AddQuad(
                new Vector3(0, 0, 0),
                new Vector3(0, Bsize.HEIGHT, 0),
                new Vector3(Bsize.WIDTH, Bsize.HEIGHT, 0),
                new Vector3(Bsize.WIDTH, 0, 0));

            m_arrTiles[(int)eDirection.forward] = m_arrTiles[(int)eDirection.backward].Clone();
            m_arrTiles[(int)eDirection.forward].pyMesh.Translate(Vector3.forward, 1);
            m_arrTiles[(int)eDirection.forward].pyMesh.reverseFace();

            m_arrTiles[(int)eDirection.left] = m_arrTiles[(int)eDirection.backward].Clone();
            m_arrTiles[(int)eDirection.left].pyMesh.rotate(new Vector3(0, -90, 0));
            m_arrTiles[(int)eDirection.left].pyMesh.reverseFace();

            m_arrTiles[(int)eDirection.right] = m_arrTiles[(int)eDirection.left].Clone();
            m_arrTiles[(int)eDirection.right].pyMesh.Translate(Vector3.right, 1);
            m_arrTiles[(int)eDirection.right].pyMesh.reverseFace();

            m_arrTiles[(int)eDirection.down] = m_arrTiles[(int)eDirection.backward].Clone();
            m_arrTiles[(int)eDirection.down].pyMesh.rotate(-90, Vector3.right);
            m_arrTiles[(int)eDirection.down].pyMesh.reverseFace();

            m_arrTiles[(int)eDirection.up] = m_arrTiles[(int)eDirection.down].Clone();
            m_arrTiles[(int)eDirection.up].pyMesh.Translate(Vector3.up, 1);
            m_arrTiles[(int)eDirection.up].pyMesh.reverseFace();


            //m_arrTiles[1] = m_arrTiles[0].Clone();
            //m_arrTiles[1].pyMesh.rotate(Vector3.right, -90);
            //m_arrTiles[1].pyMesh.reverse();

            //m_arrTiles[2] = m_arrTiles[0].Clone();
            //m_arrTiles[2].pyMesh.rotate(Vector3.up, -90);
            //m_arrTiles[2].pyMesh.reverse();

            //m_arrTiles[3] = m_arrTiles[0].Clone();
            //m_arrTiles[3].pyMesh.Translate(Vector3.back, 1);

            //m_arrTiles[4] = m_arrTiles[1].Clone();
            //m_arrTiles[4].pyMesh.Translate(Vector3.back, 1);

            //m_arrTiles[5] = m_arrTiles[2].Clone();
            //m_arrTiles[5].pyMesh.Translate(Vector3.back, 1);
        }

        public override bool IsSolid(eDirection dir)
        {
            return true;
        }
        public override bool isOverlap(BlockMeshBase mesh)
        {
            return true;
        }

        public override void ExtractMesh(eDirection dir, MeshData receiver, int x, int y, int z)
        {
            MeshData mesh = m_arrTiles[(int)dir].pyMesh.Clone();
            mesh.Translate(Vector3.right, x);
            mesh.Translate(Vector3.up, y);
            mesh.Translate(Vector3.back, z);

            receiver.add(mesh);
        }


        public override void ExtractMeshAll(MeshData receiver, int x, int y, int z)
        {
            foreach (Tile t in m_arrTiles)
            {
                if (t == null) continue;
                MeshData mesh = t.pyMesh.Clone();
                mesh.Translate(Vector3.right, x);
                mesh.Translate(Vector3.up, y);
                mesh.Translate(Vector3.back, z);


                receiver.add(mesh);
            }
        }



    }

}