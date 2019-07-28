using UnityEngine;

public class CameraFlow : MonoBehaviour {
    public Transform player;
    private float cvlaue;

    void Start() {
        cvlaue = 2;
    }

    void Update() {
        if (player.position.y >= -49f) {
            transform.position = new Vector3(transform.position.x, player.position.y - cvlaue, transform.position.z);
        }
    }
}
