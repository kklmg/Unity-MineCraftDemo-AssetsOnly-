using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Assets.Scripts.Interface;

namespace Assets.Scripts.World
{
    public abstract class FactoryBase<T> where T : IClone<T>
    {
        Dictionary<string, BlockMeshBase> m_HashMap;
        BlockMeshBase GetReference(string name)
        {
            if (m_HashMap.ContainsKey(name))
                return m_HashMap[name];
            else return null;
        }
        //abstract void InitData()
        //{



        //}

    }



    class BlockMeshFactory
    {
        private List<BlockMeshBase> m_BlockRef;
        private TileFactory m_TileFac;


        Dictionary<string, BlockMeshBase> m_HashMap;
        public BlockMeshFactory()
        {
            m_TileFac = new TileFactory();
            m_BlockRef = new List<BlockMeshBase>(1);

            //m_BlockRef[0] = new BlockMesh();
            //m_BlockRef[0][0] = m_TileFac.CreateTile(0, Vector3.left, Vector3.up, 90);
            //m_BlockRef[0][0] = m_TileFac.CreateTile(0, Vector3.right, Vector3.up, -90);
        }

        // public GetClone

        public BlockMeshBase CreateNormal()
        {
            return null;
            //return m_BlockRef[0];
        }
        public BlockMeshBase CreateMesh(int type, int m)
        {
            return null;
        }
    }
}
