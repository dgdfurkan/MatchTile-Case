using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Level2", menuName = "Scriptable Objects/Level2")]
public class Level2 : ScriptableObject
{
    [SerializeField]
    public List<LevelValue2> LevelValues2;

    [Serializable]
    public class LevelValue2
    {
        [Header("Level Informations")]
        [SerializeField]
        public int levelID;
        public string levelName;
        public int typeTileCount;

        public List<GameObject> TileValuesList2 = new List<GameObject>();
    }
}
