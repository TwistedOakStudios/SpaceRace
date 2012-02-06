using UnityEngine;
using System.Collections;

public class Wheel : MonoBehaviour {
	public bool bigWheel;
	float tiling;
	int i;
	float time = 0.0f;
	Car car;
	float startingSpeed = 1.0f;
	
	void Start() {
		i = (int)UnityEngine.Random.Range(0, 4);
		car = transform.root.GetComponent<Car>();
		if (bigWheel) 
			tiling = 0.3125f;
		else
			tiling = 0.375f;
	}
	
	void Update () {
		time += Time.deltaTime;
		if (time < startingSpeed / car.velocity) return;
		time = 0.0f;
		
		Vector2 offset;
		if (i == 0) offset = new Vector2(tiling, 0.0f);
		else if (i == 1) offset = new Vector2(0.0f, tiling);
		else if (i == 2) offset = new Vector2(0.0f, tiling);
		else offset = new Vector2(tiling, tiling);
		renderer.material.mainTextureOffset = offset;
		i = (i + 1) % 4;
		
	}
}
