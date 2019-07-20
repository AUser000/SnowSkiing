using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFlow : MonoBehaviour
{
    public Transform player;
    private float cvlaue;
    // Start is called before the first frame update
    void Start() {
        cvlaue = 2;//Screen.height/4f;
    }

    // Update is called once per frame
    void Update() {
        if (player.position.y >= -49f) {
            transform.position = new Vector3(transform.position.x, player.position.y - cvlaue, transform.position.z);
        }
    }
}
