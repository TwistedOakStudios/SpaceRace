using UnityEngine;
using System.Collections;

public class KeyboardControl : MonoBehaviour {
	KeyCode upKey = KeyCode.UpArrow;
	KeyCode downKey = KeyCode.DownArrow;
	public Car car;
	public static bool arrowsUsed = false;

	void Awake () {
		if (!arrowsUsed) {
			arrowsUsed=true;
		} else {
			upKey=KeyCode.Z;
			downKey=KeyCode.X;
			arrowsUsed = false;
		}
		car = GetComponent<Car>();	
	}

	void Update () {
        if (!Static.LevelData.Round.ended) {
            if (Input.GetKeyDown(upKey)) {
                car.MoveUp();
            }
            if (Input.GetKeyDown(downKey)) {
                car.MoveDown();
            }
        }
	}
}
