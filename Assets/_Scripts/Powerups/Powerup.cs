using UnityEngine;
using System;
using System.Collections;

public class Powerup : MonoBehaviour {
	public Lane lane;
	public float speed;
	public float speedMin;
	public float speedMax;
	public float zOffset;
	float xOffset = 50.0f;
	
	public void Deploy(Lane aLane) {
	 	speed = UnityEngine.Random.Range(speedMax, speedMin);
		lane = aLane;
		Vector3 pos = Static.LevelData.criticalZone.position;
		transform.position = new Vector3(pos.x + xOffset, lane.transform.position.y, pos.z + zOffset);
		//Static.Events.OnPowerupDeployed(this);
	}
	
	void DeathZoneReached() {
		Static.Events.OnPowerupDestroyed(this);
	}
	
	void OnTriggerEnter(Collider other) {
		Car car = other.GetComponent<Car>();
		if (car) {
			Activate(car);
		}
	}
	
	public virtual void Activate(Car car) {
		Static.Events.OnPowerupDestroyed(this);
	}
}

public partial class Events {
	public Action<Powerup> PowerupDeployed;
	public void OnPowerupDeployed(Powerup p) {
		PowerupDeployed.Invoke(p);
	}
	
	public Action<Powerup> PowerupDestroyed;
	public void OnPowerupDestroyed(Powerup p) {
		PowerupDestroyed.Invoke(p);
	}
}