using System;

using UnityEngine;

using Assets.Scripts.NMesh;

namespace Assets.Scripts.NWorld
{
    [System.Serializable]
    public class Tile
    {
        [SerializeField]
        private DynamicMeshScObj m_Mesh;

        [SerializeField]
        private int m_TexID;

        private Frame m_TexFrame;

        public DynamicMesh GetClonedMesh(int x, int y, int z,TextureSheet TexSheet)
        {
            if (m_Mesh == null) return null;
            DynamicMesh TempMesh = m_Mesh.GetClonedMesh(x,y,z);

            //Compute UV
            TempMesh.SetUV_quad(TexSheet.GetCoord(m_TexID));

            return TempMesh;
        }

        public DynamicMesh GetClonedMesh(int x, int y, int z)
        {
            DynamicMesh TempMesh = m_Mesh.GetClonedMesh(x, y, z);

            //Compute UV
            TempMesh.SetUV_quad(m_TexFrame);

            return TempMesh;
        }
    }

}
