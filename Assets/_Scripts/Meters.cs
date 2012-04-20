using UnityEngine;
using System.Collections;

public class Meters : MonoBehaviour {
	public float shieldSize =50;
	public float speedSize = 20;
	public Texture2D shields;
	public Texture2D speed;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void OnGUI () {
		float shieldHeight = (shieldSize/shields.width)*shields.height;
		Debug.Log(shieldHeight);
		GUI.DrawTexture(new Rect(Screen.width-shieldSize,0,shieldSize,shieldHeight),shields);
		
		GUI.DrawTexture(new Rect(Screen.width-speedSize,shieldHeight,speedSize,speedSize),speed);
	}
}
