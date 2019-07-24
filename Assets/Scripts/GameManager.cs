using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

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
    public Text levelText;

    //Play Pause Button
    public Sprite playSprite;
    public Sprite pauseSprite;
    public Button pauseButton;
    private Vector2 playerLastVelocity;

    private bool gameOver = false;
    private bool pause = false;

    //for debuggin
    private GameObject[] GameObjects;

    private Camera cam;
    private int level;

    private int spawndUnits;             // level * 7 + 33
    private float spwningUpperRange;     // screen.width related world cordinate
    private float spwningLowerRange;     // -screen.width related world cordinate
    private float treeDistance;          // 0.9f
    private float jungleLenth;

    private void Awake() {
        cam = GetComponent<Camera>();

        // Color settings on Game Objects
        SetColorsOnGameObject();
        ShowAboutPanel();
        InstantiateParameters();
        GameObjects = new GameObject[spawndUnits]; //  for debugging 
        MakeTheJungle();
        finishLine.transform.position = new Vector3(0, -jungleLenth, 0);
    }

    void Start() {
        UpdateGameLevelTag();
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
    }

    private void MakeTheJungle() {
        bool reached = false;
        for (int  i = 0; i < spawndUnits && !reached; i++) {
            if (i*treeDistance > 50) {
                reached = true;
            }
            GenATree(i, treeDistance);
        }

        // rest of trees
        if (spawndUnits > 50) {
            float restTreeDistance = jungleLenth / (spawndUnits - 50);
            for (int i = 0; i < spawndUnits - 50; i++) {
                GenATree(i, restTreeDistance);
            }
        }
    }

    private void InstantiateParameters() {
        if (!PlayerPrefs.HasKey("Level")) {
            PlayerPrefs.SetInt("Level", 1);
            level = 1;
        } else {
            level = PlayerPrefs.GetInt("Level");
        }
        //PlayerPrefs.SetInt("Level", 1);
        spwningUpperRange = cam.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height)).x; // right side from center
        spwningLowerRange = -spwningUpperRange; // left side from center
        spawndUnits = (level*7) + 33; // y = mx + c // m = 7 // c = 33
        jungleLenth = 50f; // is a Constant
        treeDistance = 0.9f; // is a constance
    }

    private void GenATree(int i, float treeDistance) {
        Vector3 pos = bigTree.transform.position + new Vector3(Random.Range(spwningLowerRange, spwningUpperRange), -treeDistance * i, 0);
        GameObject duplicate;
        if (i % 3 == 1) {
            duplicate = Instantiate(smallTree, pos, Quaternion.identity) as GameObject;
        } else if (i%3 == 0) {
            duplicate = Instantiate(tree, pos, Quaternion.identity) as GameObject;
        } else {
            duplicate = Instantiate(bigTree, pos, Quaternion.identity) as GameObject;
        }
        GameObjects[i] = duplicate;
    }


    public void SetColorsOnGameObject() {
        ColorStyle style = ColorStyleHolder.GetAColorSet();
        cam.backgroundColor = style.BackGroudColor;
        Color colorq = style.TreeColor;
        bigTree.transform.GetComponent<SpriteRenderer>().material.color = colorq;
        tree.transform.GetComponent<SpriteRenderer>().material.color = colorq;
        smallTree.transform.GetComponent<SpriteRenderer>().material.color = colorq;
    }

    public void GameOver() {
        Debug.Log("Game Over !!!");
        Destroy(player.gameObject.GetComponent<SpriteRenderer>());
        player.gameObject.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        if (!AboutPanel.activeSelf) {
            GameOverPanel.SetActive(true);
        }
    }

    public void RestartGame() {
        Debug.Log("Trying to Restart currunt Level");
        SceneManager.LoadScene("Game");
    }

    public void StartNextLevel() {
        level += 1;
        PlayerPrefs.SetInt("Level", level);
        Player.gameOver = true;
        Debug.Log("Game Over !!! and Win!!");
        Destroy(player.gameObject.GetComponent<SpriteRenderer>());
        player.gameObject.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        NextLevelPanel.SetActive(true);
    }

    public void UpdateGameLevelTag() {
        levelText.text = level.ToString();
    }

    public void ShowAboutPanel() {
        if (!PlayerPrefs.HasKey("OldPlayer")) {
            GamePanel.SetActive(false);
            AboutPanel.SetActive(true);
            PlayerPrefs.SetInt("OldPlayer", 1);
            player.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        }
    }

    //Button // Have a bug
    public void PauseGame() {
        if (!pause) {
            playerLastVelocity = player.GetComponent<Rigidbody2D>().velocity;
            Time.timeScale = 0;
            pause = true;
            pauseButton.image.sprite = playSprite;
        } else {
            Time.timeScale = 1;
            pause = false;
            pauseButton.image.sprite = pauseSprite;
            player.GetComponent<Rigidbody2D>().velocity = playerLastVelocity;
        }
    }
    
}
