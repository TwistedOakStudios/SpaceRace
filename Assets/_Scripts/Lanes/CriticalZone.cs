using UnityEngine;
using System;
using System.Collections;

public class CriticalZone : MonoBehaviour {
	void OnTriggerEnter(Collider other) {
		if (other.GetComponent<Car>()) {
			Static.Events.OnCriticalPointReached(other.GetComponent<Car>());
		}
	}
	
	void OnTriggerExit(Collider other) {
		if (other.GetComponent<Car>()) {
			Static.Events.OnCriticalPointExited(other.GetComponent<Car>());
		}
	}
}

public partial class Events {
	public Action<Car> CriticalPointReached;
	public void OnCriticalPointReached(Car car) {
		CriticalPointReached.Invoke(car);
	}
	
	public Action<Car> CriticalPointExited;
	public void OnCriticalPointExited(Car car) {
		CriticalPointExited.Invoke(car);
	}
}
