using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using static Level;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;

    public GameManager gameManager;
    public Level level => Resources.Load<Level>("Level/Level");

    public int levelCount => level.LevelValues.Count;

    public int currentLevel => PlayerPrefs.GetInt("CurrentLevel", 0);
    public LevelValue CurrentLevelData => level.LevelValues[currentLevel];

    public int allTilesCount => CurrentLevelData.TileValuesList.Count;

    int totalTielType = 8;

    [Header("Objects")]
    public GameObject tileObject;
    [SerializeField] private Transform tilesParent;

    private void Awake()
    {
        Instance = this;

        LoadCurrentLevelValues();
    }

    public void NextLevel()
    {
        StartCoroutine(NextLevelSlowly());
    }

    IEnumerator NextLevelSlowly()
    {
        PlayerPrefs.SetInt("CurrentLevel", currentLevel + 1);
        DestroyOldObjects();
        yield return new WaitForSecondsRealtime(.4f);
        LoadCurrentLevelValues();
    }

    private void DestroyOldObjects()
    {
        //foreach (var item in GameManager.Instance.CreatedBirds) Destroy(item);


    }

    void LoadCurrentLevelValues()
    {
        CreateGameValues();
        CreateTiles();
    }

    void CreateGameValues()
    {
        UIManager.Instance.GetLevelValues(CurrentLevelData.levelID, CurrentLevelData.levelName);
    }

    void CreateTiles()
    {
        for (int i = 0; i < CurrentLevelData.TileValuesList.Count; i++)
        {
            //GameObject createdTile = Instantiate(tileObject, (tilesParent.position + new Vector3(CurrentLevelData.TileValuesList[i].tilePosition.x, CurrentLevelData.TileValuesList[i].tilePosition.y, 0)), Quaternion.identity);

            GameObject createdTile = ObjectPool.Instance.GetObjectFromPool();

            createdTile.transform.position = tilesParent.position + new Vector3(CurrentLevelData.TileValuesList[i].tilePosition.x, CurrentLevelData.TileValuesList[i].tilePosition.y, 0);

            createdTile.transform.SetParent(tilesParent);
            createdTile.GetComponent<Tile>().SetupTile(CurrentLevelData.TileValuesList[i].type);
            gameManager.allTiles.Add(createdTile.GetComponent<Tile>());
            gameManager.levelTiles++;

            if(i == CurrentLevelData.TileValuesList.Count)
            {
                gameManager.CheckTilesAreUnder();
            }
        }
    }
}

