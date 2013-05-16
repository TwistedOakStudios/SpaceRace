using UnityEngine;
using System;
using System.Collections;

public class DeathZone : MonoBehaviour {
	void OnTriggerEnter(Collider other) {
        Static.Events.OnDeathZoneReached(other.gameObject);
	}
}

public partial class Events {
    public Action<GameObject> DeathZoneReached;
    public void OnDeathZoneReached(GameObject go) {
        DeathZoneReached.Invoke(go);
    }
}