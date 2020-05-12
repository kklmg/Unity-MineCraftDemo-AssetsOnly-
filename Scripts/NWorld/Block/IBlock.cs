using UnityEngine;

using Assets.Scripts.NMesh;
using Assets.Scripts.NData;

namespace Assets.Scripts.NWorld
{
    public interface IBlock
    {
        Sprite Icon { get; }

        bool IsSpecial { get; }
        bool IsOpaque { get; }
        bool IsObstacle { get;}

        bool IsLeftMeshExist { get; }
        bool IsRigthMeshExist { get; }
        bool IsUpMeshExist { get; }
        bool IsDownMeshExist { get; }
        bool IsFrontMeshExist { get; }
        bool IsBackMeshExist { get; }

        DynamicMesh GetLeftMesh(TextureSheet tex = null, int x = 0, int y = 0, int z = 0);
        DynamicMesh GetRightMesh(TextureSheet tex = null, int x = 0, int y = 0, int z = 0);
        DynamicMesh GetUpMesh(TextureSheet tex = null, int x = 0, int y = 0, int z = 0);
        DynamicMesh GetDownMesh(TextureSheet tex = null, int x = 0, int y = 0, int z = 0);
        DynamicMesh GetFrontMesh(TextureSheet tex = null, int x = 0, int y = 0, int z = 0);
        DynamicMesh GetBackMesh(TextureSheet tex = null, int x = 0, int y = 0, int z = 0);

        //ushort GetLeftTextureID();
        //ushort GetRightTextureID();
        //ushort GetUpTextureID();
        //ushort GetDownTextureID();
        //ushort GetFrontTextureID();
        //ushort GetBackTextureID();

        DynamicMesh GetAllMesh(TextureSheet tex, int x, int y, int z);
    }
}
