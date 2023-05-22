using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Objects")]
    [SerializeField] private GameObject[] targetArea = new GameObject[7];
    [SerializeField] private List<Tile> tileList;
    public List<Tile> allTiles = new List<Tile>();

    [Header("Properties")]
    private const int tileLenght = 7;
    [HideInInspector] public int levelTiles;

    private int firstValue, secondValue;

    private void Awake()
    {
        Instance = this;

    }

    public void SetupGame()
    {
        
    }

    public void CountAllTiles()
    {
        
    }

    public void CheckTile(Tile tile)
    {
        if(isPlaceFull())
        {
            print("<color=red> Lose </color>");
            // Lose effects (Panel, sounds, effects..)
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

                CheckTilesAreUnder();
            }
        }
        WinorLose();
    }

    public void CheckTilesAreUnder()
    {
        if (allTiles.Count <= 1) return;

        for (int i = 0; i < allTiles.Count; i++)
        {
            for (int j = i + 1; j < allTiles.Count; j++)
            {
                if (CheckCollision(allTiles[i], allTiles[j]))
                {
                    if (!allTiles[i].isFinal && !allTiles[j].isFinal)
                    {
                        //Debug.Log("Image " + allTiles[i].name + " ve Image " + allTiles[j].name + " birbirine deðiyor!");
                        
                        if (j > i)
                        {
                            Debug.Log("Image1BU " + allTiles[i].name + " ve Image " + allTiles[j].name + " birbirine deðiyor!");
                            allTiles[i].DisableItem();
                            firstValue = i;
                            allTiles[j].EnableItem();
                        }
                    }
                }
                else
                {
                    Debug.Log("Image3BU " + allTiles[i].name + " ve Image " + allTiles[j].name);
                    if (firstValue != i)
                    {
                        allTiles[i].EnableItem();
                        allTiles[j].EnableItem();
                    }
                }
            }
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
        }
        else if (tileList.Count == tileLenght)
        {
            print("<color=red> Lose </color>");
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
        Debug.Log("SetParent: " + tile.name);
    }

    private void FixTilesPlace(int index)
    {
        if (index == 0) return;

        for (int i = tileList.Count; i > index; i--)
        {
            TileToTilesPlace(tileList[i - 1], targetArea[i]);
            Debug.Log("TileToTilesPlace: " + tileList[i-1].name);
        }
    }

    public void CheckMatch(Tile tile)
    {
        if (tileList.Count < 3) return;

        int count = 0;

        Debug.Log("tiles.Count: " + tileList.Count);

        for (int i = 0; i < tileList.Count; i++)
        {
            if (tileList[i].itemType == tile.itemType)
            {
                count++;
                Debug.Log("Counted: " + count);
                if(count == 3)
                {
                    Debug.Log("Counted3 olmali: " + count);
                    Debug.Log("i: " + i);
                    Debug.Log("tiles[i]: " + tileList[i].name);
                    Debug.Log("tiles[i-1]: " + tileList[i-1].name);
                    Debug.Log("tiles[i-2]: " + tileList[i-2].name);

                    //Destroy(tileList[i].gameObject, .4f);
                    //Destroy(tileList[i-1].gameObject, .4f);
                    //Destroy(tileList[i-2].gameObject, .4f);

                    StartCoroutine(DeactivateObject(tileList[i].gameObject));
                    StartCoroutine(DeactivateObject(tileList[i-1].gameObject));
                    StartCoroutine(DeactivateObject(tileList[i-2].gameObject));

                    tileList.RemoveAll(t => t.itemType == tile.itemType);

                    StartCoroutine(EditAfterMatching(.5f));
                }
            }
        }
    }

    IEnumerator DeactivateObject (GameObject go)
    {
        go.gameObject.GetComponent<Tile>().isFinal = true;
        yield return new WaitForSeconds(.4f);
        ObjectPool.Instance.ReturnObjectToPool(go);

        yield return new WaitForSeconds(4f);
        GameObject saa = ObjectPool.Instance.GetObjectFromPool();

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
