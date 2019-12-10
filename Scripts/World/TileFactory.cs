using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.World
{
    class TileFactory
    {
        private List<Tile> m_TileProto;   //Tile prototype

        public TileFactory()
        {
            //m_TileProto = new List<Tile>(4);

            ////Normal Tile
            //m_TileProto[0] = new Tile();
            //m_TileProto[0].AddQuad(
            //    new Vector3(0, 0, 0),
            //    new Vector3(0, BlockSetting.BLOCK_HEIGHT, 0),
            //    new Vector3(BlockSetting.BLOCK_WIDTH, BlockSetting.BLOCK_HEIGHT, 0),
            //    new Vector3(BlockSetting.BLOCK_WIDTH, 0, 0));

            ////1/4 Tile
            //m_TileProto[1] = new Tile();
            //m_TileProto[1].AddQuad(
            //    new Vector3(0, 0, 0),
            //    new Vector3(0, BlockSetting.BLOCK_HEIGHT_HALF, 0),
            //    new Vector3(BlockSetting.BLOCK_WIDTH_HALF, BlockSetting.BLOCK_HEIGHT_HALF, 0),
            //    new Vector3(BlockSetting.BLOCK_WIDTH_HALF, 0, 0));

            //// 2/4 Tile
            //m_TileProto[2] = new Tile();
            //m_TileProto[2].AddQuad(
            //    new Vector3(0, 0, 0),
            //    new Vector3(0, BlockSetting.BLOCK_HEIGHT_HALF, 0),
            //    new Vector3(BlockSetting.BLOCK_WIDTH, BlockSetting.BLOCK_HEIGHT_HALF, 0),
            //    new Vector3(BlockSetting.BLOCK_WIDTH, 0, 0));

            //// 3/4 Tile
            //m_TileProto[3] = new Tile();
            //m_TileProto[3].AddQuad(
            //    new Vector3(0, 0, 0),
            //    new Vector3(0, BlockSetting.BLOCK_HEIGHT_HALF, 0),
            //    new Vector3(BlockSetting.BLOCK_WIDTH, BlockSetting.BLOCK_HEIGHT_HALF, 0),
            //    new Vector3(BlockSetting.BLOCK_WIDTH, 0, 0));
            //m_TileProto[3].AddQuad(
            //   new Vector3(0, BlockSetting.BLOCK_HEIGHT_HALF, 0),
            //   new Vector3(0, BlockSetting.BLOCK_HEIGHT, 0),
            //   new Vector3(BlockSetting.BLOCK_WIDTH_HALF, BlockSetting.BLOCK_HEIGHT, 0),
            //   new Vector3(BlockSetting.BLOCK_WIDTH_HALF, 0, 0));
        }

        public Tile CreateTile(int TileType)
        {
            return new Tile(m_TileProto[TileType]);
        }

        public Tile CreateTile(int TileType, Vector3 move, Vector3 axis, float angle)
        {
            Tile newTile = new Tile(m_TileProto[TileType]);

            return new Tile(m_TileProto[TileType]);
        }
    }
}
