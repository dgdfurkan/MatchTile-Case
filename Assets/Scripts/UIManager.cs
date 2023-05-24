using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Newtonsoft.Json.Linq;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [SerializeField] private TextMeshProUGUI levelID;
    [SerializeField] private TextMeshProUGUI levelName;
    [SerializeField] private TextMeshProUGUI score;

    [Header("LevelEndPanel")]
    [SerializeField] private GameObject levelEndPanel;
    [SerializeField] private TextMeshProUGUI levelCompleted;
    [SerializeField] private Button nextLevelButton;
    [SerializeField] private GameObject[] stars;

    [Header("Boosters")]
    [SerializeField] private TextMeshProUGUI boosterMagnet;
    [SerializeField] private TextMeshProUGUI boosterR;
    [SerializeField] private TextMeshProUGUI boosterHighlight;

    void Awake()
    {
        Instance = this;

        LevelEndPanel(false, false, 0);

        UpdateScore();
    }

    public void GetLevelValues(int value, string name)
    {
        levelID.text = "Level " + value;
        levelName.text = name;
    }

    public void UpdateScore()
    {
        int scoreValue = PlayerPrefs.GetInt("Score", 0);
        score.text = "Score: " + scoreValue;
    }

    public void UpdatingScore(int amount, int total)
    {
        StartCoroutine(IncrementScoreIE(amount, total));
    }

    IEnumerator IncrementScoreIE(int amount, int total)
    {
        int currentScore = total - amount;
        int incrementAmount = 20;
        int incrementsRemaining = amount / incrementAmount;

        while (incrementsRemaining > 0)
        {
            currentScore += incrementAmount;

            score.text = "Score: " + currentScore;

            incrementsRemaining--;
            yield return new WaitForSeconds(0.03f);

            if (incrementsRemaining == 0)
            {
                //
            }
        }
    }

    public void UpdatingBooster(BoosterManager.BoosterType boosterType, int amount)
    {
        switch (boosterType)
        {
            case BoosterManager.BoosterType.Magnet:
                boosterMagnet.text = (PlayerPrefs.GetInt("BoosterMagnet",0) + amount).ToString();
                break;
            case BoosterManager.BoosterType.R:
                boosterR.text = (PlayerPrefs.GetInt("BoosterR", 0) + amount).ToString().ToString();
                break;
            case BoosterManager.BoosterType.Highlight:
                boosterHighlight.text = (PlayerPrefs.GetInt("BoosterHighlight", 0) + amount).ToString();
                break;
        }
    }

    public void LevelEndPanel(bool open, bool winlose, int levelID)
    {
        levelEndPanel.SetActive(open);

        if (open)
        {
            string information = winlose ? "completed" : "failed";

            levelCompleted.text = $"Level {levelID} {information}";
            nextLevelButton.interactable = winlose;

            foreach (GameObject item in stars)
            {
                item.SetActive(winlose);
            }
        }
    }

    public void NextLevelButton()
    {
        LevelEndPanel(false, true, LevelManager.Instance.currentLevel);
        LevelManager.Instance.NextLevelButton();
    }

    public void MenuButton()
    {
        LevelEndPanel(false, false, LevelManager.Instance.currentLevel);
        // TODO Menu Panel
    }
}
