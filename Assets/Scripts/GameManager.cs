using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Objects")]
    [SerializeField] private GameObject[] targetArea = new GameObject[7];
    //[HideInInspector]
    public List<Tile> tileList;
    //[HideInInspector]
    public List<Tile> allTiles = new List<Tile>();

    [Header("Properties")]
    private const int tileLenght = 7;
    [HideInInspector] public int levelTiles;

    private int firstValue;

    private int score = 0;
    private const int amount = 300;

    private void Awake()
    {
        Instance = this;

        score = PlayerPrefs.GetInt("Score", 0);
    }

    public void SetupGame()
    {
        
    }

    public void CheckTile(Tile tile)
    {
        if(isPlaceFull())
        {
            print("<color=red> Lose </color>");
            // Lose effects (Panel, sounds, effects..)
            //UIManager.Instance.LevelEndPanel(true, );
        }
        else
        {
            int availableNumber = AvailablePlace(tile.gameObject.GetComponent<Tile>());


            if(availableNumber != -1)
            {
                if(availableNumber < tileList.Count)
                {
                    FixTilesPlace(availableNumber);
                }
                TileToTilesPlace(tile, targetArea[availableNumber]);
                levelTiles--;
            
                tileList.Insert(availableNumber, tile);

                CheckMatch(tile);

                tile.CheckItem(true);

                allTiles.Remove(tile);

                CheckTilesAreUnder();
            }
        }
        WinorLose();
    }

    public void CheckTilesAreUnder()
    {
        Debug.Log("allTiles.Count: " + allTiles.Count);
        firstValue = -1;

        for (int i = 0; i < allTiles.Count; i++)
        {
            for (int j = i + 1; j < allTiles.Count; j++)
            {
                if (CheckCollision(allTiles[i], allTiles[j]))
                {
                    Debug.Log("IfChecking: " + allTiles[i].name + " ve " + allTiles[j].name);
                    if (!allTiles[i].isFinal && !allTiles[j].isFinal)
                    {                       
                        if (j > i)
                        {
                            Debug.Log("Image1BU " + allTiles[i].name + " ve Image " + allTiles[j].name + " birbirine deðiyor!");
                            allTiles[i].UnControlItem(true);
                            firstValue = i;
                            Debug.Log("firstValue: "  + firstValue);
                            allTiles[j].UnControlItem(false);
                        }
                    }
                }
                else
                {
                    Debug.Log("Image3BU " + allTiles[i].name + " ve Image " + allTiles[j].name);
                    Debug.Log("firstValue: " + firstValue + " : " + i);
                    if (firstValue != i)
                    {
                        Debug.Log("Girdi");
                        allTiles[i].UnControlItem(false);
                        allTiles[j].UnControlItem(false);
                    }
                }
            }
        }

        if(allTiles.Count == 1)
        {
            allTiles[0].UnControlItem(false);
        }
    }

    private bool CheckCollision(Tile tile1, Tile tile2)
    {
        RectTransform rectTransform1 = tile1.GetComponent<Image>().rectTransform;
        RectTransform rectTransform2 = tile2.GetComponent<Image>().rectTransform;

        Vector3[] corners1 = new Vector3[4];
        Vector3[] corners2 = new Vector3[4];

        rectTransform1.GetWorldCorners(corners1);
        rectTransform2.GetWorldCorners(corners2);

        if (corners1[2].x >= corners2[0].x && corners1[0].x <= corners2[2].x &&
            corners1[2].y >= corners2[0].y && corners1[0].y <= corners2[2].y)
        {
            return true;
        }

        return false;
    }

    private bool isPlaceFull()
    {
        return (tileList.Count == tileLenght) ? true : false;
    }

    private void WinorLose()
    {
        if(levelTiles == 0 && tileList.Count == 0)
        {
            print("<color=green> Win </color>");
            LevelManager.Instance.NextLevel();
        }
        else if (tileList.Count == tileLenght)
        {
            print("<color=red> Lose </color>");
            UIManager.Instance.LevelEndPanel(true, false, LevelManager.Instance.CurrentLevelData.levelID);
        }
    }
    private int AvailablePlace(Tile tile)
    {
        int count = 0, tilePos = 0;
        for (int i = 0; i < tileList.Count; i++)
        {
            if (tile.itemType == tileList[i].itemType )
            {
                count++;
                tilePos = i;
            }
        }
        if (count != 0)
        {
            return tilePos + 1;
        }
        else
            return tileList.Count;
    }

    private void TileToTilesPlace(Tile tile, GameObject gameObject)
    {
        tile.transform.position = new Vector2(gameObject.transform.position.x, gameObject.transform.position.y);
    }

    private void FixTilesPlace(int index)
    {
        if (index == 0) return;

        for (int i = tileList.Count; i > index; i--)
        {
            TileToTilesPlace(tileList[i - 1], targetArea[i]);
        }
    }

    public void CheckMatch(Tile tile)
    {
        if (tileList.Count < 3) return;

        int count = 0;

        for (int i = 0; i < tileList.Count; i++)
        {
            if (tileList[i].itemType == tile.itemType)
            {
                count++;
                if(count == 3)
                {
                    StartCoroutine(DeactivateObject(tileList[i].gameObject));
                    StartCoroutine(DeactivateObject(tileList[i-1].gameObject));
                    StartCoroutine(DeactivateObject(tileList[i-2].gameObject));

                    tileList.RemoveAll(t => t.itemType == tile.itemType);

                    StartCoroutine(EditAfterMatching(.5f));

                    score += amount;
                    PlayerPrefs.SetInt("Score", score);
                    UIManager.Instance.UpdatingScore(amount, score);
                }
            }
        }
    }

    IEnumerator DeactivateObject (GameObject go)
    {
        go.gameObject.GetComponent<Tile>().isFinal = true;
        yield return new WaitForSeconds(.4f);
        ObjectPool.Instance.ReturnObjectToPool(go);

    }

    IEnumerator EditAfterMatching(float value)
    {
        yield return new WaitForSeconds(value);
        EditAfterMatch();
    }

    private void EditAfterMatch()
    {
        for (int i = 0; i < tileList.Count; i++)
        {
            tileList[i].transform.position = new Vector2(targetArea[i].transform.position.x, targetArea[i].transform.position.y);
        }
    }
}
