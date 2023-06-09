using System.Collections;
using UnityEngine;
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
    }

    public void NextLevel()
    {
        PlayfabManager.Instance.SendLeaderboard(1);
        StartCoroutine(NextLevelSlowly());
    }

    IEnumerator NextLevelSlowly()
    {
        

        Debug.Log("currentLevelONE: " + currentLevel);
        // TODO Panel
        yield return new WaitForSecondsRealtime(.4f);
        UIManager.Instance.LevelEndPanel(true, true, CurrentLevelData.levelID);
        
        Debug.Log("currentLevelTWO: " + currentLevel);
        //LoadCurrentLevelValues();
    }

    public void NextLevelButton()
    {
        StartCoroutine(NextLevelButtonSlowly());
    }

    IEnumerator NextLevelButtonSlowly()
    {
        // TODO Panel
        yield return new WaitForSecondsRealtime(.4f);
        LoadCurrentLevelValues();
    }

    public void LoadCurrentLevelValues()
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
            GameObject createdTile = ObjectPool.Instance.GetObjectFromPool();

            createdTile.transform.SetParent(tilesParent);
            createdTile.transform.localPosition = new Vector3(CurrentLevelData.TileValuesList[i].tilePosition.x, CurrentLevelData.TileValuesList[i].tilePosition.y, 0);
            createdTile.name = CurrentLevelData.TileValuesList[i].type.ToString() + " " + gameManager.allTiles.Count;
            createdTile.GetComponent<Tile>().SetupTile(CurrentLevelData.TileValuesList[i].type);
            createdTile.GetComponent<Tile>().isFinal = false;

            gameManager.allTiles.Add(createdTile.GetComponent<Tile>());
            gameManager.levelTiles++;

            if(i == CurrentLevelData.TileValuesList.Count-1)
            {
                gameManager.CheckTilesAreUnder();
            }
        }
    }

    public void BoosterRTile(Tile tile)
    {
        tile.transform.SetParent(tilesParent);

        int index = tile.name.IndexOf(" ") + 1;
        string tileID = tile.name.Substring(index);

        int value = int.Parse(tileID);

        tile.transform.SetParent(tilesParent);
        tile.transform.localPosition = new Vector3(CurrentLevelData.TileValuesList[value].tilePosition.x, CurrentLevelData.TileValuesList[value].tilePosition.y, 0);
        tile.GetComponent<Tile>().isFinal = false;

        gameManager.allTiles.Add(tile);
        gameManager.levelTiles++;

        gameManager.tileList.Remove(tile);

        gameManager.CheckTilesAreUnder();
    }

    public void BoosterHighlight(Tile tile, int value)
    {
        if(gameManager.allTiles.Count >= value)
        {
            tile.transform.SetAsLastSibling();

            gameManager.allTiles.Remove(tile);
            gameManager.allTiles.Add(tile);
            gameManager.CheckTilesAreUnder();
        }
    }
}

