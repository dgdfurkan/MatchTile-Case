using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    public GameManager gameManager;

    [SerializeField] private TextMeshProUGUI levelID;
    [SerializeField] private TextMeshProUGUI levelName;
    [SerializeField] private TextMeshProUGUI score;
    [SerializeField] private TextMeshProUGUI menuScore;

    [Header("Menu and Game Panels")]
    [SerializeField ] private GameObject gameUIParent;
    [SerializeField ] private GameObject targetArea;
    [SerializeField ] private GameObject menuPanel;

    [Header("LevelEndPanel")]
    [SerializeField] private GameObject levelEndPanel;
    [SerializeField] private TextMeshProUGUI levelCompleted;
    [SerializeField] private Button nextLevelButton;
    [SerializeField] private GameObject[] stars;

    [Header("Boosters")]
    [SerializeField] private TextMeshProUGUI boosterMagnet;
    [SerializeField] private TextMeshProUGUI boosterR;
    [SerializeField] private TextMeshProUGUI boosterHighlight;
    [SerializeField] private GameObject[] informationPanels;
    [SerializeField] private GameObject[] infoButtons;

    [Header("Leaderboard")]
    [SerializeField] private GameObject leaderbardPanel;
    [SerializeField] private GameObject rankPrefab;
    [SerializeField] private Transform rankContent;

    void Awake()
    {
        Instance = this;

        LevelEndPanel(false, false, 0);

        UpdateScore();

        GameState(false);
    }

    public void GameState(bool value)
    {
        menuPanel.SetActive(!value);
        gameUIParent.SetActive(value);
        targetArea.SetActive(value);
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
        menuScore.text = "Score: " + scoreValue;

        int boosterMagnetValue = PlayerPrefs.GetInt("BoosterMagnet", 0);
        int boosterRValue = PlayerPrefs.GetInt("BoosterR", 0);
        int boosterHighlightValue = PlayerPrefs.GetInt("BoosterHighlight", 0);

        boosterMagnet.text = boosterMagnetValue.ToString();
        boosterR.text = boosterRValue.ToString();
        boosterHighlight.text = boosterHighlightValue.ToString();
    }

    public void UpdatingScore(int amount, int total)
    {
        StartCoroutine(IncrementScoreIE(amount, total));
    }

    IEnumerator IncrementScoreIE(int amount, int total)
    {
        int currentScore = total - amount;
        int incrementAmount = 20;
        int incrementsRemaining = Mathf.Abs(amount) / incrementAmount;

        while (incrementsRemaining > 0)
        {
            currentScore += incrementAmount;

            score.text = "Score: " + currentScore;
            menuScore.text = "Score: " + currentScore;

            incrementsRemaining--;
            yield return new WaitForSeconds(0.02f);

            if (incrementsRemaining == 0)
            {
                //
            }
        }
    }
    public void DecreaseScore(int amount, int total)
    {
        StartCoroutine(DecreaseScoreIE(amount, total));
    }

    IEnumerator DecreaseScoreIE(int amount, int total)
    {
        int currentScore = total - amount;
        int incrementAmount = 100;
        int incrementsRemaining = Mathf.Abs(amount) / incrementAmount;

        while (incrementsRemaining > 0)
        {
            currentScore -= incrementAmount;

            score.text = "Score: " + currentScore;
            menuScore.text = "Score: " + currentScore;

            incrementsRemaining--;
            yield return new WaitForSeconds(0.02f);

            if (incrementsRemaining == 0)
            {
                //
            }
        }
    }

    public void LeaderboardButton()
    {
        if(rankContent.childCount > 0)
        {
            for (int i = 0; i < rankContent.childCount; i++)
            {
                Destroy(rankContent.GetChild(i).gameObject);
            }
        }
        
        PlayfabManager.Instance.GetLeaderboard();
    }

    public void LeaderboardRank(int rank, string name, int value)
    {
        GameObject user = Instantiate(rankPrefab, rankContent);
        user.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "#" + rank.ToString();
        user.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = name;
        user.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = value.ToString();
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

    public void InformationPanel(int value)
    {
        StartCoroutine(ClosePanelAuto(value));
    }

    IEnumerator ClosePanelAuto(int value)
    {
        informationPanels[value].SetActive(true);
        infoButtons[value].GetComponent<Button>().interactable = false;
        yield return new WaitForSeconds(3f);
        informationPanels[value].SetActive(false);
        infoButtons[value].GetComponent<Button>().interactable = true;
    }

    public void InformationButton(int value)
    {
        infoButtons[value].transform.localScale = Vector3.one;
    }

    //IEnumerator ScaleGO(int value)
    //{
    //    infoButtons[value].transform.localScale = Vector3.one;

    //    bool scaling = false;

    //    while (!scaling)
    //    {
    //        infoButtons[value].transform.localScale = Vector3.Lerp()
    //    }
    //}

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

            if(winlose) PlayerPrefs.SetInt("CurrentLevel", PlayerPrefs.GetInt("CurrentLevel") + 1);

            if (levelID == 5 && winlose)
            {
                levelCompleted.text = "You completed all levels, play again";
                PlayerPrefs.SetInt("CurrentLevel", 0);
                nextLevelButton.interactable = false;
            }
        }
    }

    public void NextLevelButton()
    {
        LevelEndPanel(false, true, LevelManager.Instance.currentLevel);
        LevelManager.Instance.NextLevelButton();
        VibrationManager.Instance.Vibration(HapticTypes.MediumImpact);
    }

    public void MenuButton()
    {
        LevelEndPanel(false, false, LevelManager.Instance.currentLevel);

        GameState(false);

        VibrationManager.Instance.Vibration(HapticTypes.MediumImpact);
    }
}
