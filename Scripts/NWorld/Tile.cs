using System;
using UnityEngine;
using UnityEditor;

using Assets.Scripts.NMesh;

namespace Assets.Scripts.NWorld
{
    [Serializable]
    struct Tile
    {
        public MeshData Mesh;
        public bool IsSolid;
        public int TexID;
    }

}
