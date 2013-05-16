using UnityEngine;
using System.Collections;

public class Wheel : MonoBehaviour {
	int i;
	float time = 0.0f;
	Car car;
	float startingSpeed = 1.0f;
    tk2dSprite sprite;
	
	void Start() {
        sprite = GetComponent<tk2dSprite>();
		i = (int)UnityEngine.Random.Range(0, 4);
        car = transform.parent.GetComponent<Car>();
	}
	
	void LateUpdate () {
		time += Time.deltaTime;
		if (time < startingSpeed / car.velocity) return;
		time = 0.0f;
		
        sprite.spriteId = i;
		i = (i + 1) % 4;
	}
}
