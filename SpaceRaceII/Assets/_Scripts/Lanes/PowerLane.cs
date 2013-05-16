using UnityEngine;
using System.Collections;

public class PowerLane : MonoBehaviour {
    tk2dSprite sprite;
    HSBColor color;
	// Use this for initialization
	void Start () {
        sprite = GetComponent<tk2dSprite>();
        color = HSBColor.FromColor(sprite.color);
	}
	
	// Update is called once per frame
	void Update () {
        color.s += Time.deltaTime;
        if (color.s > 1) { color.s = 0; }
        sprite.color = color.ToColor();
	}
}
