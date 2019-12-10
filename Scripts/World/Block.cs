using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Assets.Scripts.SMesh;

namespace Assets.Scripts.World
{
    [CreateAssetMenu(menuName = "block")]
    public class Block : ScriptableObject
    {
        public uint blockID;
        public eDirection edir;

        public bool[] m_IsSolid;
        public Vector2[] M_TextureRef;

        [SerializeField]
        private MeshData[] m_MeshDatas;

        public MeshData GetMesh(int dir)
        {
            return m_MeshDatas[dir];
        }
        private void Awake()
        {
            m_IsSolid = new bool[6] { true, true, true, true, true, true };
        }
       
        public  bool IsSolid(eDirection dir)
        {
            return m_IsSolid[(int)dir];
        }
        public  bool isOverlap(Block blk)
        {
            return true;
        }

        public void ExtractMesh(eDirection dir, MeshData receiver, int x, int y, int z)
        {
            //if ((int)dir > m_MeshDatas.) return;


            MeshData mesh = m_MeshDatas[(int)dir].Clone();
            mesh.Translate(Vector3.right, x);
            mesh.Translate(Vector3.up, y);
            mesh.Translate(Vector3.back, z);

            Frame fr = new Frame();
            fr.left = 0.2f;
            fr.right = 0.8f;
            fr.top = 0.2f;
            fr.bottom = 0.8f;

            mesh.SetUV_quad(fr);

            receiver.add(mesh);
        }


        public void ExtractMeshAll(MeshData receiver, int x, int y, int z)
        {
            foreach (MeshData m in m_MeshDatas)
            {
                if (m == null) continue;
                MeshData mesh = m.Clone();
                mesh.Translate(Vector3.right, x);
                mesh.Translate(Vector3.up, y);
                mesh.Translate(Vector3.back, z);


                receiver.add(mesh);
            }
        }




    }
}
