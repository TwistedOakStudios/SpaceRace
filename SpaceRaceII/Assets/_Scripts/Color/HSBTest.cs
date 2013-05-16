using UnityEngine;
using System.Collections;

public class HSBTest : MonoBehaviour {
    HSBColor hsl;
    public bool HSLToRGB;
    public float h;
    public float s;
    public float l;

    public Color rgbCol;

    public float r;
    public float g;
    public float b;

    // Use this for initialization
    void Start() {
    }

    // Update is called once per frame
    void Update() {
        if (HSLToRGB) {
            hsl = new HSBColor(h, s, l);
            rgbCol = hsl.ToColor();

            renderer.material.color = rgbCol;

            r = rgbCol.r;
            g = rgbCol.g;
            b = rgbCol.b;
        } else {
            hsl = HSBColor.FromColor(rgbCol);
            Color temp = hsl.ToColor();

            renderer.material.color = temp;

            h = hsl.h;
            s = hsl.s;
            l = hsl.b;
            r = temp.r;
            g = temp.g;
            b = temp.b;
        }
    }
}
