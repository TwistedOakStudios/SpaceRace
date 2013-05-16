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
		spaceraceText.transform.position = transform.position;
		spaceraceText.SetActiveRecursively(true);
	}
	
	void DisableText(Round round) {
		spaceraceText.SetActiveRecursively(false);
	}
}
