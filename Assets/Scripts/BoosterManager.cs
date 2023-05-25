using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoosterManager : MonoBehaviour
{
    public enum BoosterType {Magnet, R, Highlight}

    public GameManager gameManager;

    public void UseMagnet()
    {
        if (gameManager.tileList.Count == 0)
        {
            VibrationManager.Instance.Vibration(HapticTypes.Failure);
            return;
        } 

        // TODO if amount and fail text

        if (PlayerPrefs.GetInt("BoosterMagnet") == 0)
        {
            VibrationManager.Instance.Vibration(HapticTypes.LightImpact);
            return;
        }

        UIManager.Instance.UpdatingBooster(BoosterType.Magnet, -1);
        
        PlayerPrefs.SetInt("BoosterMagnet", PlayerPrefs.GetInt("BoosterMagnet") - 1);

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
        if (gameManager.tileList.Count == 0)
        {
            VibrationManager.Instance.Vibration(HapticTypes.Failure);
            return;
        }

        // TODO if amount and fail text

        if (PlayerPrefs.GetInt("BoosterR") == 0)
        {
            VibrationManager.Instance.Vibration(HapticTypes.LightImpact);
            return;
        }

        UIManager.Instance.UpdatingBooster(BoosterType.R, -1);

        PlayerPrefs.SetInt("BoosterR", PlayerPrefs.GetInt("BoosterR") - 1);

        LevelManager.Instance.BoosterRTile(gameManager.tileList[gameManager.tileList.Count - 1]);
    }

    public void UseHighlight()
    {
        if (gameManager.allTiles.Count <= 1) 
        {
            VibrationManager.Instance.Vibration(HapticTypes.Failure);
            return; 
        }

        // TODO if amount and fail text
        // TODO isUnder amount check

        List<int> undertilesValue = new List<int>();

        for (int i = 0; i < gameManager.allTiles.Count; i++)
        {
            if (gameManager.allTiles[i].isUnder) undertilesValue.Add(i);
        }

        if(undertilesValue.Count <= 0)
        {
            VibrationManager.Instance.Vibration(HapticTypes.Failure);
            return;
        }

        if (PlayerPrefs.GetInt("BoosterHighlight") == 0)
        {
            VibrationManager.Instance.Vibration(HapticTypes.LightImpact);
            return;
        }

        UIManager.Instance.UpdatingBooster(BoosterType.Highlight, -1);
        
        PlayerPrefs.SetInt("BoosterHighlight", PlayerPrefs.GetInt("BoosterHighlight") - 1);

        int randomValue = Random.Range(0, undertilesValue.Count);

        LevelManager.Instance.BoosterHighlight(gameManager.allTiles[undertilesValue[randomValue]], undertilesValue[randomValue]);
    }

    public void InformationPanel(int value)
    {
        UIManager.Instance.InformationPanel(value);
    }

    public void BuyBooster(int boosterType)
    {
        switch (boosterType)
        {
            case 0:
                if (gameManager.score < 1500)
                {
                    VibrationManager.Instance.Vibration(HapticTypes.Failure);
                    return;
                }
                PlayerPrefs.SetInt("BoosterMagnet", PlayerPrefs.GetInt("BoosterMagnet") + 1);
                UIManager.Instance.UpdatingBooster(BoosterType.Magnet, 0);
                gameManager.ScoreAmountDecrease(-1500);
                break;
            case 1:
                if (gameManager.score < 1800)
                {
                    VibrationManager.Instance.Vibration(HapticTypes.Failure);
                    return;
                }
                PlayerPrefs.SetInt("BoosterR", PlayerPrefs.GetInt("BoosterR") + 1);
                UIManager.Instance.UpdatingBooster(BoosterType.R, 0);
                gameManager.ScoreAmountDecrease(-1800);
                break;
            case 2:
                if (gameManager.score < 2000)
                {
                    VibrationManager.Instance.Vibration(HapticTypes.Failure);
                    return;
                }
                PlayerPrefs.SetInt("BoosterHighlight", PlayerPrefs.GetInt("BoosterHighlight") + 1);
                UIManager.Instance.UpdatingBooster(BoosterType.Highlight, 0);
                gameManager.ScoreAmountDecrease(-2000);
                break;
        }
    }
}
