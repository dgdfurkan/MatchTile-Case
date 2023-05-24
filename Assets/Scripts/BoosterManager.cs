using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Tile;

public class BoosterManager : MonoBehaviour
{
    public enum BoosterType {Magnet, R, Highlight}

    public GameManager gameManager;

    public void UseMagnet()
    {
        if (gameManager.tileList.Count == 0) return;

        // TODO if amount and fail text

        PlayerPrefs.SetInt("BoosterMagnet", PlayerPrefs.GetInt("BoosterMagnet", 0) - 1);

        UIManager.Instance.UpdatingBooster(BoosterType.Magnet, -1);

        //int random = Random.Range(0,gameManager.tileList.Count);

        //Tile.ItemType itemType = gameManager.tileList[random].itemType;

        //for (int i = 0; i < gameManager.allTiles.Count; i++)
        //{
        //    if (gameManager.allTiles[i].itemType == itemType)
        //    {
        //        gameManager.CheckTile(gameManager.allTiles[i]);
        //        break;
        //    }
        //}

        StartCoroutine(SlowlyMagnet());
    }

    IEnumerator SlowlyMagnet()
    {
        yield return new WaitForSeconds(.3f);

        int random = Random.Range(0, gameManager.tileList.Count);

        Tile.ItemType itemType = gameManager.tileList[random].itemType;

        for (int i = 0; i < gameManager.allTiles.Count; i++)
        {
            if (gameManager.allTiles[i].itemType == itemType)
            {
                gameManager.CheckTile(gameManager.allTiles[i]);
                break;
            }
        }
    }

    public void UseR()
    {

    }

    public void UseHighlight()
    {
        if(gameManager.allTiles.Count >= 3)
        {

        }
    }
}
