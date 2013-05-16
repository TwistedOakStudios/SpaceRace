using UnityEngine;
using System;
using System.Collections;

public class Powerup : MonoBehaviour {
	public Lane lane;
    public AudioClip clip;
	
	public void Deploy(Lane aLane) {
		lane = aLane;
		Vector3 pos = Static.LevelData.criticalZone.position;
		transform.position = new Vector3(pos.x, lane.transform.position.y, 1);
        Static.Events.CameraUpdateComplete += Move;
		Static.Events.OnPowerupDeployed(this);
	}

    public void Move(RaceCamera camera) {

    }
	
	void DeathZoneReached() {
		Static.Events.OnPowerupDestroyed(this);
	}
	
	void OnTriggerEnter(Collider other) {
		Car car = other.GetComponent<Car>();
		if (car) {
            
            //Static.SoundMaster.Play(clip);
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