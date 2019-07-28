using UnityEngine;

public class ColorStyle {
    public Color32 LoaderColor;
    public Color BallColor;
    public Color32 BackGroudColor;
    public Color TreeColor;

    public ColorStyle(Color loadercolor, Color ballColor, Color backGroundColor, Color treeColor) {
        LoaderColor = loadercolor;
        BackGroudColor = backGroundColor;
        BallColor = ballColor;
        TreeColor = treeColor;
    }
}

public class ColorStyleHolder : MonoBehaviour {

    #region STATIC VARIBLES
    public static int colorValuSet;
    public static ColorStyle[] colors = { new ColorStyle(new Color32(255, 55, 55, 1),
                                                            new Color(255, 55, 55, 1),
                                                            new Color32(241, 248, 233, 1),
                                                            new Color(0.48627f,  0.70196f,  0.25882f)), // Green
                                            new ColorStyle(new Color32(255, 55, 55, 1),
                                                            new Color(255, 55, 55, 1),
                                                            new Color32(239, 224, 224, 1),
                                                            new Color(0.4375f, 0.4375f, 0.3098f)), // Brown
                                            new ColorStyle(new Color32(255, 255, 255, 1),
                                                            new Color32(255, 255, 255, 1),
                                                            new Color32(249,251,231, 1),
                                                            new Color(0.75294f,  0.79216f,  0.20000f)), // Lime
                                            new ColorStyle(new Color32(255, 255, 255, 1),
                                                            new Color32(255, 255, 255, 1),
                                                            new Color32(255, 243, 224, 1),
                                                            new Color(0.98431f,  0.54902f,  0.00000f)), // Orange
                                            new ColorStyle(new Color32(255, 255, 255, 1),
                                                            new Color32(255, 255, 255, 1),
                                                            new Color32(255, 235, 238 , 1),
                                                            new Color(0.8254f,  0.7787f,  0.0431f)), // Red
                                            new ColorStyle(new Color32(255, 255, 255, 1),
                                                            new Color32(255, 255, 255, 1),
                                                            new Color32(243, 229, 245, 1),
                                                            new Color(0.81f, 0.47f, 0.51f)), // Purple
                                        };
    public static ColorStyleHolder instance;
    #endregion

    public void Awake() {
        instance = this;
    }

    public static ColorStyle GetAColorSet() {
        Debug.Log("color value set");
        colorValuSet = Random.Range(0, colors.Length);        
        return colors[colorValuSet];
    }
}
