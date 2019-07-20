﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Loder : MonoBehaviour {

    public Slider slider;
    public GameObject player;
    private Vector2 start;
    private Vector2 end;
    private float progress;

    void Start() {
        start = player.transform.position;
        end = start + new Vector2(0, 55);
        progress = 0f;
    }

    void Update() {
        if (player.transform.position.y < end.y) {
            progress = (player.transform.position.y - start.y) / (start.y - end.y);
            slider.value = progress;
        }
    }

    public void RestartButton() {
        SceneManager.LoadScene("Game");
    }
}