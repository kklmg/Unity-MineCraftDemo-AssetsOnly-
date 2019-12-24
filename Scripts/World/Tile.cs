using System;
using UnityEngine;
using UnityEditor;

using Assets.Scripts.SMesh;

namespace Assets.Scripts.WorldComponent
{
    [Serializable]
    struct Tile
    {
        public MeshData Mesh;
        public bool IsSolid;
        public int TexID;
    }

}
