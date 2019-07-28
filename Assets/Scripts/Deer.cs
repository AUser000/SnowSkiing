using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deer : MonoBehaviour {

    public GameObject player;

    private Rigidbody2D body;
    private bool isMoving;
    private float time;

    void Start() {
        isMoving = false;
        body = this.GetComponent<Rigidbody2D>();
        time = 0;
    }

    void Update() {
        if (!isMoving) {
            if (Mathf.Abs(player.transform.position.y - this.transform.position.y) < 3) {
                this.body.velocity = new Vector2(1f, 0);
                isMoving = true;
            }
        }
        if (isMoving) {
            if (time < 4f) {
                time += Time.deltaTime;
            } else {
                Destroy(this);
            }
        }
    }
}
