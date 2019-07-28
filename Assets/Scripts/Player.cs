using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour {
    public float speedX;
    public float speedY;
    public float treeDistance;
    public float time;
    public float currunt;
    private bool left = false;
    private bool timePass = true;
    private float changeTime;
    private float velocity;
    private Rigidbody2D body;
    private float mouseTime;
    private bool mousePressed;
    private bool longPass;

    public GameObject GameOverPanel;
    public GameObject NextLevelPanel;
    public GameObject EncPrefab;
    public Text ScoreText;

    public AudioSource HitSound;

    public static bool gameOver = false;
    public static bool win = false;
    private int score;
    private int highScore;
    public static int level;
    private float timeHolder;

    void Start() {
        score = 0;
        if (!PlayerPrefs.HasKey("Level")) {
            level = 1;
        } else {
            level = PlayerPrefs.GetInt("Level");
        }
        if (PlayerPrefs.HasKey(level.ToString())) {
            highScore = PlayerPrefs.GetInt(level.ToString());
        } else {
            highScore = 0;
        }
        gameOver = false;
        win = false;
        treeDistance = 0.6f;
        mouseTime = 0;
        time = .5f;
        body = GetComponent<Rigidbody2D>();
        body.velocity = new Vector2(-1f, -1f);
        speedY = -1.8f;
        speedX = body.velocity.x;
    }

    void Update() {
        if (timeHolder > 1) {
            timeHolder = 0;
            ScoreAndNotify(level);
        } else {
            timeHolder += Time.deltaTime;
        }

        RaycastHit2D hit = Physics2D.Raycast(this.gameObject.transform.position + new Vector3(0.24f, 0.24f, 0f), Vector2.down);
        RaycastHit2D hit2 = Physics2D.Raycast(this.gameObject.transform.position + new Vector3(-0.24f, 0.24f, 0f), Vector2.down);

        if (hit.collider != null) {
            if (hit.distance <= treeDistance) {
                if (hit.collider.tag == "Tree") {
                    hit.collider.gameObject.GetComponent<Tree>().PlayAnim();
                    ScoreAndNotify(level * 2);
                }
            }
        }

        if (hit2.collider != null) {
            if (hit2.distance <= treeDistance) {
                if (hit2.collider.tag == "Tree") {
                    hit2.collider.gameObject.GetComponent<Tree>().PlayAnim();
                    ScoreAndNotify(level * 2);
                }
            }
        }

        if (Input.GetMouseButtonDown(0) && timePass) {
            mousePressed = true;
            if (left) {
                left = false;
            } else {
                left = true;
            }
            timePass = false;
        } else if (Input.GetMouseButtonUp(0)) {
            mousePressed = false;
            mouseTime = 0;
        }

        if (mousePressed) {
            mouseTime += Time.deltaTime;
            if (mouseTime >= 0.3) {
                longPass = true;
            } else {
                longPass = false;
            }
        }

        // curve motion
        if (!timePass) {
            if (!gameOver) {
                if (longPass) {
                    body.velocity = LongTimePass(left, Time.deltaTime);
                } else {
                    body.velocity = TimePass(left, Time.deltaTime);
                }
            } else {
                body.velocity = Vector2.zero;
            }
        }
    }

    private Vector2 TimePass(bool left, float delta) {
        currunt += delta;
        velocity = ((currunt % time) * 6) - 2;
        if (currunt >= time) {
            timePass = true;
            currunt = 0;
            if (left) {
                return new Vector2(1.9f, speedY);
            } else {
                return new Vector2(-1.9f, speedY);
            }
        }
        if (left) {
            return new Vector2(velocity, speedY);
        } else {
            return new Vector2(-velocity, speedY);
        }
    }

    private Vector2 LongTimePass(bool left, float delta) {
        currunt += delta;
        velocity = ((currunt % time) * 9) - 3;
        if (currunt >= time) {
            timePass = true;
            currunt = 0;
            if (left) {
                return new Vector2(3, speedY);
            } else {
                return new Vector2(-3, speedY);
            }
        }
        if (left) {
            return new Vector2(velocity, speedY);
        } else {
            return new Vector2(-velocity, speedY);
        }
    }


    private void OnCollisionEnter2D(Collision2D collision) {
        GameOverAndShowRestartPanel();
    }

    public void GameOverAndNextLevel() {
        gameOver = true;
        SaveScore();
        Destroy(this.gameObject.GetComponent<SpriteRenderer>());
        this.gameObject.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        NextLevelPanel.SetActive(true);
    }

    public void GameOverAndShowRestartPanel() {
        if (GameManager.isMusicOn) {
            HitSound.Play();
        }
        gameOver = true;
        if (GameManager.isVibrationOn) { 
            Vibration.Vibrate(350);
        }
        
        SaveScore();
        Destroy(this.gameObject.GetComponent<SpriteRenderer>());
        this.gameObject.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        GameOverPanel.SetActive(true);
    }

    private void ScoreAndNotify(int i) {
        score += i;
        ScoreText.text = score.ToString();
    }

    private void SaveScore() {
        if (highScore < score) {
            PlayerPrefs.SetInt(level.ToString(), score);
        }
    }

}
