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
        //Field------------------------------------------
        [SerializeField]
        private Tile[] m_Tiles;

        //public Function----------------------------------
        public MeshData GetMesh(eDirection dir)
        {
            return m_Tiles[(int)dir].Mesh;
        }
       
       
        public  bool IsSolid(eDirection dir)
        {
            return m_Tiles[(int)dir].IsSolid;
        }
        public  bool isOverlap(Block blk)
        {
            return true;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="dir"></param>
        /// <param name="receiver"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="z"></param>
        /// <param name="texRef"></param>
        public void ExtractMesh(eDirection dir, MeshData receiver, int x, int y, int z,TextureSheet texRef)
        {
            if (m_Tiles[(int)dir].Mesh == null) return;

            //make clone
            MeshData mesh = m_Tiles[(int)dir].Mesh.Clone();
            
            //set postion
            mesh.Translate(Vector3.right, x);
            mesh.Translate(Vector3.up, y);
            mesh.Translate(Vector3.forward, z);

            //set texture
            Frame fr = texRef.GetCoord(m_Tiles[(int)dir].TexID);
            mesh.SetUV_quad(fr);

            //save mesh data
            receiver.add(mesh);
        }


        public void ExtractMeshAll(MeshData receiver, int x, int y, int z, TextureSheet texRef)
        {
            foreach (Tile t in m_Tiles)
            {
                if (t.Mesh == null) continue;

                //make clone
                MeshData mesh = t.Mesh.Clone();

                //set postion
                mesh.Translate(Vector3.right, x);
                mesh.Translate(Vector3.up, y);
                mesh.Translate(Vector3.back, z);

                //set texture
                Frame fr = texRef.GetCoord(t.TexID);
                mesh.SetUV_quad(fr);

                receiver.add(mesh);
            }
        }

    }
}
