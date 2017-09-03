using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

    static public GameManager gm;

    public Text scoreText;				//Text组件，用于显示当前游戏得分的文本信息
    public Text timeText;				//Text组件，用于显示当前游戏时间的文本信息

    public Text finalScore;               //最终结算界面
    public Text finishCnt;
    public Text bestScore;
    public Text hintScore;

    private int TaskNum = 0;
    private int tip = 0;
    private int failNum = 0;
    private int currentScore;			//当前得分

    public float restrictTime;			//本关限时
    private float startTime;
    private float pauseTime;
    private float currentTime;

    public GameObject star1, star2, star3;
    public int threeStars;             //得分星级
    public int twoStars;
    public int oneStar;

    public GameObject GameOverUI;
    public GameObject GamePauseUI;

    public enum GameState 				//游戏状态枚举
    { Playing, Pause, GameOver };
    public GameState gameState;			//游戏状态

    public GameObject player;			//游戏主角

    private bool isGameOver = false;

	// Use this for initialization
	void Start () {
        Cursor.visible = true;	//禁用鼠标光标
        if (gm == null)			//静态游戏管理器初始化
            gm = GetComponent<GameManager>();
        if (player == null)		//获取场景中的游戏主角
            player = GameObject.FindGameObjectWithTag("Player");
        gm.gameState = GameState.Playing;	//游戏状态设置为游戏进行中
        currentScore = 0;					//当前得分初始化为0
        startTime = Time.time;
        Time.timeScale = 1;
	}

    public void GamePause() {
        Cursor.visible = true;
        Object[] objects = GameObject.FindGameObjectsWithTag("Player");
        foreach (GameObject go in objects)
             go.SendMessage("PauseGame");
        Time.timeScale = 0;
        GamePauseUI.SetActive(true);
        gameState = GameState.Pause;
        GameManager.gm.SendMessage("PauseGame");
    }

    public void GameResume() {
        Cursor.visible = true;
        Object[] objects = GameObject.FindGameObjectsWithTag("Player");
        foreach (GameObject go in objects)
            go.SendMessage("ResumeGame");
        GamePauseUI.SetActive(false);
        Time.timeScale = 1;
        gameState = GameState.Playing;
        GameManager.gm.SendMessage("ResumeGame");
    }

    public void AddScore(int tp) {
        ++TaskNum;
        tip += tp;
        currentScore += 20;
        currentScore += tp;
    }

    public void MinusScore() {
        ++failNum;
        currentScore -= 10;
    }

	void Update () {
        switch (gameState) {
            case GameState.Playing:
                if (Input.GetKeyDown(KeyCode.Escape))
                    GamePause();
                currentTime = restrictTime - (Time.time - startTime);
                int minute = (int)currentTime / 60;
                int second = (int)(currentTime - minute*60);
                timeText.text = string.Format("{0:D2}:{1:D2}", minute, second);
                scoreText.text = currentScore.ToString();
                if (currentTime <= 0)
                    gameState = GameState.GameOver;
                break;
            case GameState.Pause:
                if (Input.GetKeyDown(KeyCode.Escape))
                    GameResume();
                break;
            case GameState.GameOver:
                Cursor.visible = true;
                GameManager.gm.SendMessage("PauseGame");
                this.SendMessage("GameOver");
                GameOverUI.SetActive(true);     //结束界面显示
                string fc = TaskNum.ToString() + " x 20 = " + (TaskNum*20).ToString() +'\n' 
                             + tip.ToString() +'\n'
                             + failNum.ToString() + " x 10 = " + (failNum * 10).ToString();
                finishCnt.text = fc;
                finalScore.text = currentScore.ToString();
                int bs = PlayerPrefs.GetInt("level1_best");
                if (bs < currentScore) {
                    PlayerPrefs.SetInt("level1_best",currentScore);
                    bs = currentScore;
                }
                if (currentScore >= threeStars) {
                    star1.GetComponent<Image>().color = Color.white;
                    star2.GetComponent<Image>().color = Color.white;
                    star3.GetComponent<Image>().color = Color.white;
                    bestScore.text = bs.ToString() + '\n';
                    hintScore.text = "最好成绩\n";
                }
                else if (currentScore >= twoStars) {
                    star1.GetComponent<Image>().color = Color.white;
                    star2.GetComponent<Image>().color = Color.white;
                    star3.GetComponent<Image>().color = Color.black;
                    bestScore.text = bs.ToString() + '\n' + threeStars.ToString();
                }
                else if (currentScore >= oneStar) {
                    star1.GetComponent<Image>().color = Color.white;
                    star2.GetComponent<Image>().color = Color.black;
                    star3.GetComponent<Image>().color = Color.black;
                    bestScore.text = bs.ToString() + '\n' + twoStars.ToString();
                }
                else {
                    star1.GetComponent<Image>().color = Color.black;
                    star2.GetComponent<Image>().color = Color.black;
                    star3.GetComponent<Image>().color = Color.black;
                    bestScore.text = bs.ToString() + '\n' + oneStar.ToString();
                }
                break;
        }
	}
}
