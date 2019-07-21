using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tree : MonoBehaviour {

    Animator anim;
    private bool animated;
    public GameObject FloatingTextPrefab;

    public void Awake() {
        anim = GetComponent<Animator>();
        animated = true;
    }

    public void PlayAnim() {
        if (animated) {
            animated = false;
            anim.SetTrigger("Big");
            Debug.Log("FloatingText Before");
            if (FloatingTextPrefab != null) {
                ShowFloatingText();
                Debug.Log("FloatingText Before");
            }
        }
    }

    void ShowFloatingText() {
        var go = Instantiate(FloatingTextPrefab, transform.position + new Vector3(0, .4f, 0), Quaternion.identity);
        go.GetComponent<TextMesh>().text = (Player.level * 2).ToString();
    }
}
