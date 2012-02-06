using UnityEngine;
using System.Collections;

public class VictoryText : MonoBehaviour {
	public GameObject spaceraceTextPrefab;
	private GameObject spaceraceText;
	
	void Start() {
		Static.Events.RoundEnded += EnableText;
		Static.Events.RoundStarted += DisableText;
		spaceraceText = Instantiate(spaceraceTextPrefab, Vector3.zero, Quaternion.identity) as GameObject;
	}

	void EnableText(Round round) {
		/*
		Vector3 p;
		if (round.cars.Count > 0) {
			Car car = round.cars[0];
			p = car.transform.position;
		}	 
		else 
			p = Vector3.zero; 
		*/
		//spaceraceText.transform.position = new Vector3(p.x, p.y, p.z + 1);
		spaceraceText.transform.position = transform.position;
		spaceraceText.SetActiveRecursively(true);
	}
	
	void DisableText(Round round) {
		spaceraceText.SetActiveRecursively(false);
	}
}
