using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingText : MonoBehaviour {

    private float destroyTime = 1.2f;

    void Start() {
        Destroy(gameObject, destroyTime);
    }
    
}
