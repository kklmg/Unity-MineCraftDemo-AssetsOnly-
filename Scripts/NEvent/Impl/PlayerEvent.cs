using UnityEngine;

namespace Assets.Scripts.NEvent
{
    //Event: Player's Chunk Location has Changed
    class E_Player_LeaveChunk : EventBase<E_Player_LeaveChunk>
    {
        public Vector2Int ChunkInWorld { set; get; }
        public Vector2Int Offset;
        public int playerView { set; get; }

        public E_Player_LeaveChunk(Vector2Int _chunkLoc,int _viewDistance, Vector2Int _offset)
        {
            ChunkInWorld = _chunkLoc;
            playerView = _viewDistance;
            Offset = _offset;
        }
    }
}
