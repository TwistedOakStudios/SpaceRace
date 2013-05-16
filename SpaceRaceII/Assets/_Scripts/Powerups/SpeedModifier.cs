using UnityEngine;
using System.Collections;

public class SpeedModifier : Powerup {
	public float relativeVelocity = 10.0f;
	public float time = 1.0f;
	
	public override void Activate(Car car) {
		StartCoroutine(car.AccelerateByVelocityOverTime(relativeVelocity, time));
		Static.Events.OnPowerupDestroyed(this);
	}
}
