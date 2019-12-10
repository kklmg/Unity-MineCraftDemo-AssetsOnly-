using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Assets.Scripts.Interface;
using Assets.Scripts.SMesh;

namespace Assets.Scripts.World
{
    class Tile : IClone<Tile>
    {
        //Mesh m_Mesh = new Mesh();
        MeshData m_Mesh; 
        public MeshData pyMesh { get { return m_Mesh; } }

        private bool m_bSolid = true;
       // public Vector2Int  texture uv;
        

        public Tile()
        {
            m_Mesh = ScriptableObject.CreateInstance<MeshData>();
            Debug.Log("instance created");
        }
        public Tile(Tile proto)
        {
            m_Mesh = proto.pyMesh.Clone();
            m_bSolid = proto.IsSolid();
        }
        public Tile Clone()
        {
            return new Tile(this);

        }


        public void AddQuad(Vector3 v1, Vector3 v2, Vector3 v3, Vector3 v4)
        {
            m_Mesh.AddQuad(v1, v2, v3, v4);
        }
        public void ExtractMesh(MeshFilter Filter)
        {
            m_Mesh.ToMeshFilter(Filter);
        }
        public bool IsSolid()
        {
            return m_bSolid;
        }

        //xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx
        Tile GetNonOverlapTile(Tile other)
        {
            return null;
        }

    }

}
