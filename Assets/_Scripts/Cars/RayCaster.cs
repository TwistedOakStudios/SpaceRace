using UnityEngine;
using System.Collections;

public class RayCaster : MonoBehaviour {
	public bool CarInAdjacentLane(int direction) {
		RaycastHit hit;
		if (Physics.Raycast(transform.position, direction * Vector3.up, out hit, Static.LevelData.LaneManager.laneDistance)) {
			if (hit.transform.GetComponent<Car>()) return true;
		}
		return false;
	}
}
