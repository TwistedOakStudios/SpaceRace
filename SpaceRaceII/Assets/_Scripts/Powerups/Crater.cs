using UnityEngine;
using System.Collections;

public class Crater : Powerup {
	public override void Activate(Car car) {
		car.HitCrater();
		Static.Events.OnPowerupDestroyed(this);
	}
}
