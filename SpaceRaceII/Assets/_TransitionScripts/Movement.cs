using UnityEngine;
using System.Collections;

public class Movement : MonoBehaviour {
    KeyCode upKey = KeyCode.UpArrow;
    KeyCode downKey = KeyCode.DownArrow;
    tk2dSprite sprite;

	// Use this for initialization
	void Start () {
        sprite = GetComponent<tk2dSprite>();
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(upKey)) {
            transform.position += new Vector3(0, 12, 0);
            sprite.spriteId = 1;
        }
        if (Input.GetKeyDown(downKey)) {
            transform.position -= new Vector3(0, 12, 0);
            sprite.spriteId = 2;
        }
        
	}
}
