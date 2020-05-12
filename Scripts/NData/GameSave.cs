using System;

using UnityEngine;

namespace Assets.Scripts.NData
{
    [System.Serializable]
    public class GameSave
    {
        public string WorldSeed;
        public string WorldName;

        public GameTime LastPlayed;      
        public Vector3 PlayerPos;
        public Quaternion playerRot;

        public bool HasPlayerSpawned = false;

        public WorldChange WorldModfication;

        public void Init(string _WorldName,string _WorldSeed)
        {
            WorldName = _WorldName;
            WorldSeed = _WorldSeed;
            LastPlayed.Assign(System.DateTime.Now);
            WorldModfication = new WorldChange();
        }
    }
}
