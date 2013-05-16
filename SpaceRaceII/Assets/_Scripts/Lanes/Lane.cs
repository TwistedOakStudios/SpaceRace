using UnityEngine;
using System.Collections;

public class Lane : MonoBehaviour {
	public Transform predictorPlacement;
    public bool isPower;
    public float relativeVelocity = 64;
    tk2dSprite sprite;
    HSBColor color;
    bool goingUp = true;
    // Use this for initialization
    void Start() {
        sprite = GetComponent<tk2dSprite>();
        color = HSBColor.FromColor(sprite.color);
    }

    // Update is called once per frame
    void Update() {
        if (isPower) {
            if (goingUp) {
                color.s += Time.deltaTime;
            } else {
                color.s -= Time.deltaTime;
            }
            if (color.s > 1) { color.s = 0.99f; goingUp = false; }
            if (color.s < 0) { color.s = 0.01f; goingUp = true; }
            sprite.color = color.ToColor();
        }
    }
}
    