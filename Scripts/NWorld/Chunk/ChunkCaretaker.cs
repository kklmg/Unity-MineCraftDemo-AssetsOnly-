using UnityEngine;
using System.Collections.Generic;

namespace Assets.Scripts.NWorld
{
    class ChunkCaretaker : MonoBehaviour
    {
        [SerializeField]
        private Dictionary<Vector2Int, ChunkMemo> m_Memo = new Dictionary<Vector2Int, ChunkMemo>();

        void Write(Vector2Int a,ChunkMemo memo)
        {


        }



    }
}
