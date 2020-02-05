using UnityEngine;
using Assets.Scripts.NMesh;

namespace Assets.Scripts.NWorld
{
    [CreateAssetMenu(menuName = "block")]
    public class Block : ScriptableObject
    {
        //Field------------------------------------------
        [SerializeField]
        private Tile[] m_Tiles;

        //public Function----------------------------------
        public MeshDataScriptable GetMesh(byte dir)
        {
            return m_Tiles[dir].Mesh;
        }
       
       
        public  bool IsSolid(byte dir)
        {
            return m_Tiles[dir].IsSolid;
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
        public void ExtractMesh(byte dir, MeshDataDynamic receiver,ref BlockInSection blkInSec,TextureSheet texRef)
        {
            if (m_Tiles[dir].Mesh == null) return;

            //make clone
            MeshDataDynamic mesh = m_Tiles[dir].Mesh.Data.Clone();
            
            //set postion
            mesh.Translate(Vector3.right, blkInSec.x);
            mesh.Translate(Vector3.up, blkInSec.y);
            mesh.Translate(Vector3.forward, blkInSec.z);

            //set texture
            Frame fr = texRef.GetCoord(m_Tiles[dir].TexID);
            mesh.SetUV_quad(fr);

            //save mesh data
            receiver.Add(mesh);
        }

        public void ExtractMeshAll(MeshDataDynamic receiver, int x, int y, int z, TextureSheet texRef)
        {
            foreach (Tile t in m_Tiles)
            {
                if (t.Mesh == null) continue;

                //make clone
                MeshDataDynamic mesh = t.Mesh.Data.Clone();

                //set postion
                mesh.Translate(Vector3.right, x);
                mesh.Translate(Vector3.up, y);
                mesh.Translate(Vector3.back, z);

                //set texture
                Frame fr = texRef.GetCoord(t.TexID);
                mesh.SetUV_quad(fr);

                receiver.Add(mesh);
            }
        }

    }
}
