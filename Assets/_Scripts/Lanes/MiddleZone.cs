using UnityEngine;
using System;
using System.Collections;

public class MiddleZone : MonoBehaviour {
	void OnTriggerEnter(Collider other) {
		if (other.GetComponent<Car>()) {
			Static.Events.OnMiddlePointReached(other.GetComponent<Car>());
		}
	}
}

public partial class Events {
	public Action<Car> MiddlePointReached;
	public void OnMiddlePointReached(Car car) {
		MiddlePointReached.Invoke(car);
	}
}
