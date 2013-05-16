using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

public class RaceCamera : MonoBehaviour {
	public float velocity;
	
	void LateUpdate () {
        // If any of the cars are in critical, average only those cars. Otherwise average all cars.
        bool critical = Static.LevelData.Round.cars.Exists(car => car.isCritical == true);
        velocity = (from car in Static.LevelData.Round.cars
                    where !car.isDead && car.isCritical == critical    
                    select car.velocity).Average();
        Static.Events.OnCameraUpdateComplete(this);
	}
}

public partial class Events {
    public Action<RaceCamera> CameraUpdateComplete;
    public void OnCameraUpdateComplete(RaceCamera camera) {
        CameraUpdateComplete.Invoke(camera);
    }
}