using UnityEngine;
using System.Collections;

public class MobileControl : MonoBehaviour {
	private Car car;
	public GUITexture up;
	public GUITexture down;
	public GUITexture brake;
	
	void Awake() {
		car = transform.root.GetComponent<Car>();
		transform.parent = null;
		transform.position = new Vector3(0,0,0);
		transform.rotation = Quaternion.identity;
	}
	
	void Update() {
		if (!car) return;
		for (int i = 0; i < Input.touchCount; i++) {
			Touch touch = Input.GetTouch(i);
			bool upHit = up.HitTest(touch.position);
			bool downHit = down.HitTest(touch.position);
			
			if (touch.phase == TouchPhase.Began) {
				if (upHit) car.MoveUp();
				else if (downHit) car.MoveDown();
			}
		}
	}
	
	/*
	void OnGUI() {
		
		if (GUI.Button(new Rect(x, y, width, height),"Up")) {
			car.Move(1);
		}
		if (GUI.Button(new Rect(x + width, y, width, height),"Down")) {
			car.Move(-1);
		}
		if (GUI.Button(new Rect(x + 2 * width, y, width, height),"Brake")) {
			StartCoroutine(car.Brake()); 
		}
	}
	*/
}

/*
public partial class Events {
	public event Action up1Press;
	public void OnUp1Press() {
		up1Press.Invoke();
	}
}
*/
