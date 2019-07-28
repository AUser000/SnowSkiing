using UnityEngine;

public class Tree : MonoBehaviour {

    Animator anim;
    private bool animated;
    public GameObject FloatingTextPrefab;
    public GameObject EncPrefab;
    //public AudioSource HitMusicSource;

    public void Awake() {
        anim = GetComponent<Animator>();
        animated = true;
    }

    public void PlayAnim() {
        if (animated) {
            animated = false;
            anim.SetTrigger("Big");
            if (FloatingTextPrefab != null) {
                ShowFloatingText();
                ShowEncText();
                LetsShake();
            }
        }
    }

    private void ShowFloatingText() {
        var go = Instantiate(FloatingTextPrefab, transform.position + new Vector3(0, .4f, 0), Quaternion.identity);
        go.GetComponent<TextMesh>().text = (Player.level * 2).ToString();
    }

    private void ShowEncText() {
        if (EncPrefab != null) { // not necessory
            if (Random.Range(1, 5) % 3 == 0) {
                Instantiate(EncPrefab, new Vector3(0, transform.position.y, 0), Quaternion.identity);
            }
        }
    }

    private void LetsShake() {
        if (GameManager.isVibrationOn) {
            Vibration.Vibrate(80);
        }
    }

}
