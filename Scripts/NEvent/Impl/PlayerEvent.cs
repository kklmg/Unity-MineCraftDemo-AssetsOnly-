using UnityEngine;

namespace Assets.Scripts.NEvent
{
    //Event: Player's Chunk Location has Changed
    public class E_Player_Spawned : EventBase<E_Player_Spawned>
    {
        public Vector3 SpawnPos { set; get; }
        public Character Player { set; get; }

        public E_Player_Spawned(Vector3 _SpawnPos,Character _player)
        {
            SetPriority(0);

            SpawnPos = _SpawnPos;
            Player = _player;
        }
    }

    class E_Player_LeaveChunk : EventBase<E_Player_LeaveChunk>
    {
        public Vector2Int Offset;

        public E_Player_LeaveChunk(Vector2Int _offset)
        {
            SetPriority(0);

            Offset = _offset;
        }
    }
}
