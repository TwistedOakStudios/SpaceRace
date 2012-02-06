using UnityEngine;
using System;
using System.Collections;

public class DeathZone : MonoBehaviour {
	void OnTriggerEnter(Collider other) {
		other.SendMessage("DeathZoneReached");
	}
}
