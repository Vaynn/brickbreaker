using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public int currentLevel = 1;
    private int score = 0;
    private int coins = 0;

    private int life = 3;
    private int bricksRemaining = 0;
    public TextMeshProUGUI coinsText;
    public TextMeshProUGUI pointsText;
    public TextMeshProUGUI lifeText;
    public TextMeshProUGUI titleText;
    public TextMeshProUGUI pointsWinnerPanel;
    public TextMeshProUGUI coinsWinnerPanel;
    [SerializeField] private TextMeshProUGUI lastScoreReplay;
    [SerializeField] private TextMeshProUGUI newScoreReplay;
    [SerializeField] private TextMeshProUGUI savedScoreReplay;
    [SerializeField] private TextMeshProUGUI coinsReplay;
    [SerializeField] private Button ok;
    public GameObject GameOverPanel;
    public GameObject WinnerPanel;
    public GameObject PausePanel;
    public GameObject WinnerReplayPanel;
    public GameObject paddle;
    public GameObject ball;
    [SerializeField] private Button pauseButton;
    [SerializeField] private PlayerMove paddleControl;
    [SerializeField] public Button startButton;
    private Dictionary<BonusType, bool> bonusStates = new();
    public List<GameObject> bonusPrefabs;

    private void Awake(){
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();  
        if(Instance == null){
            Instance = this;
            bonusStates = new Dictionary<BonusType, bool>(){
                {BonusType.ExpandPaddle, false},
                {BonusType.ShrinkPaddle, false},
                {BonusType.BallSpeedUp, false },
                {BonusType.ExtraLife, false}
            };
        } 
        else Destroy(gameObject);
    }

    private void Start(){
        ShowStartButton();
        if (GameContext.Instance.CurrentMode == GameMode.Adventure)
            currentLevel = UserManager.Instance.Profile.highestLevel;
        else if (GameContext.Instance.CurrentMode == GameMode.Retry)
        {
            currentLevel = GameContext.Instance.LevelToLoad;
            lastScoreReplay.text = GameContext.Instance.LastScoreLevelToLoad.ToString("D4");
        }
        else if (GameContext.Instance.CurrentMode == GameMode.Generator)
        {
            currentLevel = GameContext.Instance.LevelToLoad;
        }
        Sprite loadedBall = Resources.Load<Sprite>("Sprites/DarkTheme/Items/" + UserManager.Instance.Profile.equippedBall);
        Sprite loadedPaddle = Resources.Load<Sprite>("Sprites/DarkTheme/Items/" + UserManager.Instance.Profile.equippedPaddle);
        ball.GetComponent<SpriteRenderer>().sprite = loadedBall;
        paddle.GetComponent<SpriteRenderer>().sprite = loadedPaddle;
        paddleControl.enabled = false;
        UpdateLevelTitle();
    }

    public async void LoadNextLevel(bool goToNextStage){
        await UserManager.Instance.SubmitBestScore("level " + currentLevel, score);
        await UserManager.Instance.UpdateTotalScore(score);
        UserManager.Instance.AddCoins(coins);
        currentLevel++;
        UserManager.Instance.UpdateHighestLevel(currentLevel);
        string nextLevelFile = "level" + currentLevel + ".json";
        Time.timeScale = 1f;
        if(goToNextStage){
            GameContext.Instance.SetLevelToLoad(currentLevel);
            GameContext.Instance.SetMode(GameMode.Adventure);
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
        else{
            SceneManager.LoadScene("Menu");
            SoundManager.PlaySound(SoundType.BACK);
        }
    }

    public async void ValidNewVictory(){
        if (score > GameContext.Instance.LastScoreLevelToLoad){
            await UserManager.Instance.SubmitBestScore("level " + currentLevel, score);
            await UserManager.Instance.UpdateTotalScore(score += score - GameContext.Instance.LastScoreLevelToLoad);   
        }
        UserManager.Instance.AddCoins(coins);
        HideWinnerReplayPanel();
        Time.timeScale = 1f;
        SceneManager.LoadScene("Stages");
    }


    public void UpdateLevelTitle(){
        if (titleText != null){
            titleText.text = "STAGE " + currentLevel;
        }
    }

    public void AddPoints(int amount){
        score += amount;
        pointsText.text = score.ToString("D4");
        pointsWinnerPanel.text = pointsText.text;
        newScoreReplay.text = pointsText.text;
        if (score > GameContext.Instance.LastScoreLevelToLoad)
            savedScoreReplay.text = score.ToString("D4");
        else
            savedScoreReplay.text = GameContext.Instance.LastScoreLevelToLoad.ToString("D4");
        Debug.Log("last scoooore" + GameContext.Instance.LastScoreLevelToLoad.ToString());
        Debug.Log("Score: " + score);
    }

    public void AddCoins(int amount){
        coins += amount;
        coinsText.text = coins.ToString("D4");
        coinsWinnerPanel.text = coinsText.text;
        coinsReplay.text = coinsText.text;
        Debug.Log("Coins: " + coins);
    }

    public void RegisterBricks(){
        bricksRemaining++;
    }

    public void BrickDestroyed(){
        bricksRemaining--;
        if(bricksRemaining <= 0){
            Debug.Log("Niveau réussi !");
            GameObject ball = GameObject.FindWithTag("Ball");
            Time.timeScale = 0f;
            if (ball != null)
                Destroy(ball);
            if (GameContext.Instance.CurrentMode == GameMode.Adventure)
                ShowWinnerPanel();
            else
                ShowWinnerReplayPanel();
        }
    }

    public void LooseLife(){
        life--;
        lifeText.text = life.ToString();
        if (life <= 0){
            Debug.Log("Game Over");
            GameObject ball = GameObject.FindWithTag("Ball");
            Time.timeScale = 0f;
            if (ball != null)
                Destroy(ball);
            if (GameContext.Instance.CurrentMode == GameMode.Adventure)
                ShowGameOverPanel();
            else
                SceneManager.LoadScene("Stages");
        } else {
            ShowStartButton();
            Vector3 pos = paddle.transform.position;
            pos.x = 0f;
            paddle.transform.position = pos;
            paddleControl.enabled = false;
        }
    }

    public void ShowGameOverPanel(){
        GameOverPanel.SetActive(true);
        pauseButton.interactable = false;
        paddleControl.enabled = false;
        SoundManager.PlaySound(SoundType.LOOSE_LIFE);
    }

    public void HideGameOverPanel(){
        GameOverPanel.SetActive(false);
        pauseButton.interactable = true;
        paddleControl.enabled = true;
    }

    public void ShowWinnerPanel(){
        WinnerPanel.SetActive(true);
        pauseButton.interactable = false;
        paddleControl.enabled = false;
        SoundManager.PlaySound(SoundType.WIN);
    }

    public void HideWinnerPanel(){
        WinnerPanel.SetActive(false);
        pauseButton.interactable = true;
        paddleControl.enabled = true;
    }

    public void ShowWinnerReplayPanel(){
        WinnerReplayPanel.SetActive(true);
        pauseButton.interactable = false;
        paddleControl.enabled = false;
        SoundManager.PlaySound(SoundType.WIN);
    }

    public void HideWinnerReplayPanel(){
        WinnerReplayPanel.SetActive(false);
        pauseButton.interactable = true;
        paddleControl.enabled = true;
        SoundManager.PlaySound(SoundType.SELECT);
    }

    public void ShowPausePanel(){
        Time.timeScale = 0;
        PausePanel.SetActive(true);
        pauseButton.interactable = false;
        paddleControl.enabled = false;
        SoundManager.PlaySound(SoundType.SELECT);
    }

    public void HidePausePanel(){
        PausePanel.SetActive(false);
        pauseButton.interactable = true;
        paddleControl.enabled = true;
        SoundManager.PlaySound(SoundType.BACK);
    }

    public void ApplyBonus(BonusType bonus){
        if (bonus == BonusType.ExtraLife){
            life++;
            lifeText.text = life.ToString();
            return;
        }
        if(bonusStates.ContainsKey(bonus) && bonusStates[bonus])
            return;
        bonusStates[bonus] = true;
        switch(bonus){
            case BonusType.ExpandPaddle:
                StartCoroutine(ExpandPaddleRoutine());
                break;
            case BonusType.ShrinkPaddle:
                StartCoroutine(ShrinkPaddleRoutine());
                break;
            case BonusType.BallSpeedUp:
                StartCoroutine(SpeedUpBallRoutine());
                break;
        }
    }

    public bool CanSpawnBonus(BonusType type)
    {
        if (type == BonusType.ExtraLife)
            return true;
        return !bonusStates.ContainsKey(type) || !bonusStates[type];
    }

    IEnumerator ExpandPaddleRoutine(){
        Transform t = paddle.transform;
        t.localScale += new Vector3(0.2f, 0, 0);

        yield return new WaitForSeconds(15f);

        t.localScale -= new Vector3(0.2f, 0, 0);
        bonusStates[BonusType.ExpandPaddle] = false;
    }

    IEnumerator ShrinkPaddleRoutine(){
    Transform t = paddle.transform;
    Vector3 originalScale = t.localScale;
    // shrink de 30 %
    t.localScale = new Vector3(originalScale.x * 0.7f,
                               originalScale.y,
                               originalScale.z);
    yield return new WaitForSeconds(15f);
    t.localScale = originalScale;
    bonusStates[BonusType.ShrinkPaddle] = false;
}

    IEnumerator SpeedUpBallRoutine(){
        Rigidbody2D rb = ball.GetComponent<Rigidbody2D>();
        rb.linearVelocity *= 1.5f;

        yield return new WaitForSeconds(15f);

        rb.linearVelocity *= 0.666f; // annule l'effet
        bonusStates[BonusType.BallSpeedUp] = false;
    }

    public void HideStartButton(){
        Time.timeScale = 1f;
        startButton.interactable = false;
        startButton.gameObject.SetActive(false);
        paddleControl.enabled = true;
    }

    private void ShowStartButton(){
        Time.timeScale = 0f;
        Vector2 p = paddle.transform.position;
        p.x = 0;
        paddle.transform.position = p;
        startButton.interactable = true;
        startButton.gameObject.SetActive(true);
    }

}
