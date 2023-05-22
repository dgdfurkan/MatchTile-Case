using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Level", menuName = "Scriptable Objects/Level")]
public class Level : ScriptableObject
{
    [SerializeField]
    public List<LevelValue> LevelValues;

    [Serializable]
    public class LevelValue
    {
        [Header("Level Informations")]
        [SerializeField]
        public int levelID;
        public string levelName;
        public int typeTileCount;

        public List<TileValue> TileValuesList = new List<TileValue>();
    }

    [Serializable]
    public class TileValue
    {
        [Header("Tile Informations")]
        public Tile.ItemType type;
        public Vector2 tilePosition;
    }
}
