using UnityEngine;
using System.Collections;

public class VictoryText : MonoBehaviour {
	void Start() {
		Static.Events.RoundEnded += EnableText;
		Static.Events.RoundStarted += DisableText;
	}

	void EnableText(Round round) {
		gameObject.SetActiveRecursively(true);
	}
	
	void DisableText(Round round) {
		gameObject.SetActiveRecursively(false);
	}
}
