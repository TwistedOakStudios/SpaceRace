using UnityEngine;
using System.Collections;

public class Predictor : MonoBehaviour {
	void OnTriggerEnter(Collider other) {
		Destroy(gameObject);
	}
}
