using UnityEngine;
using System.Collections;

public class RayCaster : MonoBehaviour {
    Car myCar;

    void Start() {
        myCar = transform.parent.GetComponent<Car>();
    }

	public bool CarInAdjacentLane(int direction) {
		RaycastHit hit;
		if (Physics.Raycast(transform.position, direction * Vector3.up, out hit, Static.LevelData.LaneManager.laneDistance)) {
            if (hit.transform.GetComponent<Car>() && hit.transform.GetComponent<Car>() != myCar) {
                Debug.Log("allalala"); return true;
            }
        }
		return false;
	}
}
