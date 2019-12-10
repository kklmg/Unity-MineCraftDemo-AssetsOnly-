using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Assets.Scripts.Pattern;


namespace Assets.Scripts.World
{
    [Serializable]
    public class BlockSize
    {
        //block Size setting
        [SerializeField]
        private float m_fWidth = 1.0f;
        [SerializeField]
        private float m_fHeight = 1.0f;
        [SerializeField]
        private float m_fDepth = 1.0f;

        public float WIDTH { get { return m_fWidth; } }
        public float HEIGHT { get { return m_fHeight; } }
        public float DEPTH { get { return m_fDepth; } }

        public Texture2D aa;
        //public Texture bbs;
        public Texture2DArray ada;

        private readonly float WIDTH_HALF;
        private readonly float HEIGHT_HALF;
        private readonly float DEPTH_HALF;
        public BlockSize()
        {
            WIDTH_HALF = WIDTH / 2;
            HEIGHT_HALF = HEIGHT / 2;
            DEPTH_HALF = DEPTH / 2;
        }

        public Material ss;

        [SerializeField]
        private List<Block> m_listBlock;

        //public Block seasd = new Block();

    }



    [Serializable]
    public class BlockManager : MonoSingleton<BlockManager>
    {
        public BlockSize m_BlockSize = new BlockSize();
    }
}
