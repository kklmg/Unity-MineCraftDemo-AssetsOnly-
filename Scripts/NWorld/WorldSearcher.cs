using UnityEngine;

namespace Assets.Scripts.NWorld
{
    static class GWorldSearcher
    {
        //Search Chunk 
        //-----------------------------------------------------------------------
        static public Chunk GetChunk(Vector3 Coord, IWorld _World)
        {
            return _World.Pool.GetChunk(new ChunkInWorld(Coord, _World));
        }
        static public Chunk GetChunk(ChunkInWorld chunkinworld, IWorld _World)
        {
            return _World.Pool.GetChunk(chunkinworld);
        }

        //Search Section 
        //-----------------------------------------------------------------------
        static public Section GetSection(Vector3 Coord, IWorld _World)
        {
            //Try get chunk
            Chunk _chunk = GetChunk(Coord, _World);
            if (_chunk == null) return null;

            //Get Section
            return _chunk.GetSection(new SectionInChunk(Coord, _World));
        }
        static public Section GetSection(SectionInWorld Slot, IWorld _World)
        {
            //Try get chunk
            Chunk _chunk = GetChunk(Slot.ToChunkInWorld(_World), _World);
            if (_chunk == null) return null;

            //get Section
            return _chunk.GetSection(Slot.ToSectionInChunk());
        }

        //Search Block 
        //-----------------------------------------------------------------------
        static public Block GetBlock(Vector3 Coord, IWorld _World)
        {
            //Try get section
            Section section = GetSection(Coord, _World);
            if (section == null) return null;

            BlockInSection BlockSlot = new BlockInSection(Coord, _World);
            return section.GetBlock(BlockSlot);
        }

        static public float GetGroundHeight(Vector3 Coord, IWorld _World)
        {
            Chunk chk = GetChunk(new ChunkInWorld(), _World);
            if (chk == null || Coord.y < 0) return float.MinValue;

            BlockInSection BlkInSec = new BlockInSection(Coord, _World);

            float res;
            if (chk.GetGroundHeight(BlkInSec.x, BlkInSec.z, (int)Coord.y, out res))
                return res;
            else return float.MinValue;
        }
    }
}
