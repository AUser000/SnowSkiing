using UnityEngine;

public class FloatingText : MonoBehaviour {

    private float destroyTime = 0.9f;

    void Start() {
        Destroy(gameObject, destroyTime);
    }
    
}
