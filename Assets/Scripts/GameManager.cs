using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

    public GameObject bigTree;
    public GameObject tree;
    public GameObject smallTree;
    public GameObject player;
    public GameObject Loader;
    public GameObject finishLine;
    public GameObject GamePanel;
    public GameObject GameOverPanel;
    public GameObject NextLevelPanel;
    public GameObject AboutPanel;
    public GameObject CommingSoonPanel;

    public GameObject VibrateButton;
    public GameObject HighScoreButton;
    public GameObject MusicButton;
    public GameObject AboutUsButton;
    public Text levelText;

    //Play Pause Button
    public Sprite playSprite;
    public Sprite pauseSprite;
    public Button pauseButton;
    public Button settingsButton;

    public AudioSource HitMusicSource;
    public AudioSource BackGroudSource;

    private Vector2 playerLastVelocity;
    private bool gameOver = false;
    private bool pause = false;
    private Camera cam;
    private int level;

    private int spawnUnits;             // level * 7 + 33
    private float spwningUpperRange;     // screen.width related world cordinate
    private float spwningLowerRange;     // -screen.width related world cordinate
    private float treeDistance;          // 0.9f
    private float jungleLenth;

    private bool settingsButtonActive;
    private bool commingSoonPanelActive;

    public static bool isVibrationOn;
    public static bool isMusicOn;

    private void Awake() {
        cam = GetComponent<Camera>();
        SetColorsOnGameObject();
        ShowAboutPanel();
        InstantiateParameters(); 
        MakeTheJungle();
        MakeTheFinishLine();
    }

    void Start() {
        UpdateGameLevelTag();
        PlayBackgroundMusic();
    }

    void Update() {
        if (player.transform.position.y < -52f) {
            if (!Player.win) {
                Player.win = true;
                StartNextLevel();
            }
        }
        if (player.transform.position.x >= spwningUpperRange || player.transform.position.x <= spwningLowerRange) {
            if (!Player.gameOver) {
                GameOver();
                Player.gameOver = true;
            }
        }

        HandleInputs();
    }

    public void PlayBackgroundMusic() {
        if (GameManager.isMusicOn) {
            Debug.Log("Now Playing Background Music");
            BackGroudSource.Play();
        }
    }

    private void MakeTheJungle() {
        bool reached = false;
        for (int  i = 0; i < spawnUnits && !reached; i++) {
            if (i*treeDistance > 50) {
                reached = true;
            }
            GenATree(i, treeDistance);
        }

        // rest of trees
        if (spawnUnits > 50) {
            float restTreeDistance = jungleLenth / (spawnUnits - 50);
            for (int i = 0; i < spawnUnits - 50; i++) {
                GenATree(i, restTreeDistance);
            }
        }
    }

    private void HandleInputs() {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            Application.Quit();
        }
    }

    private void MakeTheFinishLine() {
        finishLine.transform.position = new Vector3(0, -jungleLenth, 0);
    }

    private void InstantiateParameters() {
        if (!PlayerPrefs.HasKey("Level")) {
            PlayerPrefs.SetInt("Level", 1);
            level = 1;
        } else {
            level = PlayerPrefs.GetInt("Level");
        }

        if (!PlayerPrefs.HasKey("Vib")) {
            PlayerPrefs.SetInt("Vib", 1);
            isVibrationOn = true;
        } else if (PlayerPrefs.GetInt("Vib") == 1) {
            isVibrationOn = true;
        } else {
            VibrateButton.GetComponent<Image>().color = new Color(.7f, .7f, .7f, 1);
            isVibrationOn = false;
        }
        
        if (!PlayerPrefs.HasKey("Music")) {
            PlayerPrefs.SetInt("Music", 1);
            isMusicOn = true;
        } else if (PlayerPrefs.GetInt("Music") == 1) {
            isMusicOn = true;
        } else {
            MusicButton.GetComponent<Image>().color = new Color(.7f, .7f, .7f, 1);
            isMusicOn = false;
        }

        settingsButtonActive = false;
        commingSoonPanelActive = false;
        spwningUpperRange = cam.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height)).x; // right side from center
        spwningLowerRange = -spwningUpperRange; // left side from center
        spawnUnits = (level*5) + 40; // y = mx + c // m = 7 // c = 33 // last used values
        jungleLenth = 50f; // is a Constant
        treeDistance = 0.9f; // is a constance
    }

    private void GenATree(int i, float treeDistance) {
        Vector3 pos = bigTree.transform.position + new Vector3(Random.Range(spwningLowerRange, spwningUpperRange), -treeDistance * i, 0);
        if (i % 3 == 1) {
            Instantiate(smallTree, pos, Quaternion.identity);
        } else if (i%3 == 0) {
            Instantiate(tree, pos, Quaternion.identity);
        } else {
            Instantiate(bigTree, pos, Quaternion.identity);
        }
    }

    private void SetColorsOnGameObject() {
        ColorStyle style = ColorStyleHolder.GetAColorSet();
        cam.backgroundColor = style.BackGroudColor;
        Color colorq = style.TreeColor;
        bigTree.transform.GetComponent<SpriteRenderer>().material.color = colorq;
        tree.transform.GetComponent<SpriteRenderer>().material.color = colorq;
        smallTree.transform.GetComponent<SpriteRenderer>().material.color = colorq;
    }

    private void GameOver() {
        if (GameManager.isVibrationOn) {
            Vibration.Vibrate(350);
        }
        if (GameManager.isMusicOn) {
            Debug.Log("Playing");
            HitMusicSource.Play();
        }
        Destroy(player.gameObject.GetComponent<SpriteRenderer>());
        player.gameObject.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        if (!AboutPanel.activeSelf) {
            GameOverPanel.SetActive(true);
        }
    }

    private void RestartGame() {
        SceneManager.LoadScene("Game");
    }

    private void StartNextLevel() {
        level += 1;
        PlayerPrefs.SetInt("Level", level);
        //PlayerPrefs.SetInt(level.ToString(), player.score);
        Player.gameOver = true;
        Destroy(player.gameObject.GetComponent<SpriteRenderer>());
        player.gameObject.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        NextLevelPanel.SetActive(true);
    }

    private void UpdateGameLevelTag() {
        levelText.text = level.ToString();
    }

    private void ShowAboutPanel() {
        if (!PlayerPrefs.HasKey("OldPlayer")) {
            GamePanel.SetActive(false);
            AboutPanel.SetActive(true);
            PlayerPrefs.SetInt("OldPlayer", 1);
            player.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        }
    }

    // Button Trigger
    public void PauseGameButtonTrigger() {
        if (!pause) {
            Time.timeScale = 0;
            pause = true;
            pauseButton.image.sprite = playSprite;
            settingsButton.interactable = false;
        } else {
            Time.timeScale = 1;
            pause = false;
            pauseButton.image.sprite = pauseSprite;
            settingsButton.interactable = true;
        }
    }

    // Button Trigger
    public void SettingsButtonTrigger() {
        if (!pause) {
            Time.timeScale = 0;
            pause = true;
            pauseButton.image.sprite = playSprite;
        } else {
            Time.timeScale = 1;
            pause = false;
            pauseButton.image.sprite = pauseSprite;
        }
        if (!settingsButtonActive) {
            pauseButton.interactable = false;
            HighScoreButton.SetActive(true);
            VibrateButton.SetActive(true);
            MusicButton.SetActive(true);
            AboutUsButton.SetActive(true);
            settingsButtonActive = true;
        } else {
            pauseButton.interactable = true;
            HighScoreButton.SetActive(false);
            VibrateButton.SetActive(false);
            MusicButton.SetActive(false);
            AboutUsButton.SetActive(false);
            settingsButtonActive = false;
        }
        
        //ShowOtherButtons();
    }

    // Button Trigger Vibrate On Off
    public void VibrateButtonTrigger() {
        if (isVibrationOn) {
            isVibrationOn = false;
            PlayerPrefs.SetInt("Vib", 0);
            VibrateButton.GetComponent<Image>().color = new Color(.7f, .7f, .7f, 1);
        } else {
            isVibrationOn = true;
            PlayerPrefs.SetInt("Vib", 1);
            Vibration.Vibrate(500);
            VibrateButton.GetComponent<Image>().color = new Color(1f, 1f, 1f, 1);
        }
    }

    // Button Trigger Music On Off
    public void MusicButtonTrigger() {
        if (isMusicOn) {
            isMusicOn = false;
            PlayerPrefs.SetInt("Music", 0);
            MusicButton.GetComponent<Image>().color = new Color(.7f, .7f, .7f, 1);
            if (BackGroudSource.isPlaying) {
                BackGroudSource.Stop();
            }
        } else {
            isMusicOn = true;
            PlayerPrefs.SetInt("Music", 1);
            MusicButton.GetComponent<Image>().color = new Color(1f, 1f, 1f, 1);
            BackGroudSource.Play();
        }
    }

    // Button Trigger
    public void AboutUsButtonTrigger() {
        CommingSoon();
    }
    
    // Button Trigger
    public void HighScoreButtonTrigger() {
        CommingSoon();
    }
    
    // Button Trigger
    public void ComminSoonButtonTrigger() {
        if(commingSoonPanelActive) {
            CommingSoonPanel.SetActive(false);
            commingSoonPanelActive = false;
        }
    }

    private void CommingSoon() {
        CommingSoonPanel.SetActive(true);
        commingSoonPanelActive = true;
    }
    
}
