using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TencesHolder : MonoBehaviour {
    public static TencesHolder instance;
    public static int i;


    private void Awake() {
        i = -1;
        instance = this;    
    }

    private static string[] t = {"cool", "smooth", "Excelent", "wow", "common", "king of the mountan", "mountan is yours", "you won't loose"};

    public static string GetText() {
        i++;
        if (i == t.Length) {
            i = 1;
        }
        return t[i];
    }
}
