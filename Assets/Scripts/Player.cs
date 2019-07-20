﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour {
    public float speedX;
    public float speedY;
    public float enemyDistance;
    private bool left = false;
    private bool timePass = true;
    public float time;
    public float currunt;
    private float changeTime;
    float velocity;
    private Rigidbody2D body;
    private ParticleSystem particleSystem;
    private float mouseTime;
    private bool mousePressed;
    private bool longPass;
    ParticleSystem.EmissionModule emmition;

    private float prev;

    // for debug
    private Vector3 duration;
    public bool stop = false;

    public GameObject GameOverPanel;
    public GameObject NextLevelPanel;
    public static bool gameOver = false;
    public static bool win = false;

    void Start() {
        // debug parm
        duration = new Vector3(.4f, -.4f, 0);
        gameOver = false;
        win = false;
        // non debug parm
        enemyDistance = 0.5f;
        mouseTime = 0;
        time = .5f;
        body = GetComponent<Rigidbody2D>();
        body.velocity = new Vector2(0f, 0f);
        speedY = -1.8f;
        speedX = body.velocity.x;
        particleSystem = GetComponentInChildren<ParticleSystem>();
        emmition = particleSystem.emission;
    }

    void Update() {
        RaycastHit2D hit = Physics2D.Raycast(this.gameObject.transform.position + new Vector3(0.24f, 0.24f, 0f), Vector2.down);
        RaycastHit2D hit2 = Physics2D.Raycast(this.gameObject.transform.position + new Vector3(-0.24f, 0.24f, 0f), Vector2.down);

        if (hit.collider != null) {
            if (hit.distance <= enemyDistance) {
                if (hit.collider.tag == "Tree") {
                    Tree tree = hit.collider.gameObject.GetComponent<Tree>();
                    tree.PlayAnim();
                }
            }
        }

        if (hit2.collider != null) {
            if (hit2.distance <= enemyDistance) {
                if (hit2.collider.tag == "Tree") {
                    Tree tree = hit2.collider.gameObject.GetComponent<Tree>();
                    tree.PlayAnim();
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
        }
        if (Input.GetMouseButtonUp(0)) {
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
            if(!gameOver) {
                if (longPass) {
                    body.velocity = LongTimePass(left, Time.deltaTime);
                } else {
                    body.velocity = TimePass(left, Time.deltaTime);
                }
            } else {
                body.velocity = Vector2.zero;
            }
        }

        if (mousePressed) {
            emmition.enabled = true;
        } else {
            emmition.enabled = false;
        }
    }

    public Vector2 TimePass(bool left, float delta) {
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

    public Vector2 LongTimePass(bool left, float delta) {
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
        GameOverAndRestart();
    }

    public void GameOverAndNextLevel() {
        gameOver = true;
        Debug.Log("Game Over !!! and Win!!");
        Destroy(this.gameObject.GetComponent<SpriteRenderer>());
        this.gameObject.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        NextLevelPanel.SetActive(true);
    }

    public void GameOverAndRestart() {
        gameOver = true;
        Debug.Log("Game Over !!!");
        Destroy(this.gameObject.GetComponent<SpriteRenderer>());
        this.gameObject.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        GameOverPanel.SetActive(true);
    }
}
