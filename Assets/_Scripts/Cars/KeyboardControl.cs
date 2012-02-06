using UnityEngine;
using System.Collections;

public class KeyboardControl : MonoBehaviour {
	KeyCode upKey = KeyCode.UpArrow;
	KeyCode downKey = KeyCode.DownArrow;
	KeyCode specialKey = KeyCode.LeftArrow;
	public Car car;
	public static bool arrowsUsed = false;
	void Awake () {
		if (!arrowsUsed) {
			arrowsUsed=true;
		} else {
			upKey=KeyCode.Z;
			downKey=KeyCode.X;
			specialKey = KeyCode.C;
			arrowsUsed = false;
		}
		car = GetComponent<Car>();	
	}

	void Update () {
		if (Input.GetKeyDown(upKey)) {
			car.Move(1);
		} 
		if (Input.GetKeyDown(downKey)) {
			car.Move(-1);
		}
		if (Input.GetKeyDown(specialKey)) {
			StartCoroutine(car.Brake()); 
		}
	}
}
