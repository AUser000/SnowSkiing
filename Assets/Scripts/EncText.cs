using UnityEngine;

public class EncText : MonoBehaviour {

    private float destroyTime = 2f;
    TextMesh text;
    public static EncText instance;

    private void Awake() {
        instance = this;
    }

    public static bool isThere() {
        return instance != null;
    }

    void Start() {
        this.gameObject.GetComponent<TextMesh>().text = TencesHolder.GetText();
        Destroy(gameObject, destroyTime);
    }
}
