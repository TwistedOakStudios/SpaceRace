using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LaneManager : MonoBehaviour {
	public float laneDistance = 3.0f;
	public int numberOfLanes = 8;
    public Transform lanePrefab;
	public List<Transform> lanes;
	
	public Color[] lerpPoints = new Color[]{Color.red, Color.green, Color.blue};
	
	void Awake() {
        CreateLanes();
	}
	
	void CreateLanes() {
        lanes = new List<Transform>(numberOfLanes);
        for (int i = 0; i < numberOfLanes; i++) {
            Transform lane; 
				lane = Instantiate(lanePrefab, new Vector3(0, i * laneDistance, -15), Quaternion.identity) as Transform;
				lane.localScale = new Vector3(80.0f, laneDistance, 0.1f);
				lane.renderer.material.color = lerpPoints.GetRangeValue((float)i/(float)numberOfLanes);
			
			//lane.GetComponent<Lane>().speedModifier = 1.0f;
            lanes.Add(lane);
        }
	}

    public bool IsValidLane(int laneNumber) {
		if (laneNumber < 0 || laneNumber >= numberOfLanes) {
			return false;	
		} else {
			return true;
		}
	}

    public Lane GetLane(int laneNumber) {
	 	if(IsValidLane(laneNumber)) 
			return lanes[laneNumber].GetComponent<Lane>();
		else 
			return null; 
			
    }
}

public partial class LevelData {
	public LaneManager laneManager;	
	public LaneManager LaneManager {
	 	get { return laneManager; }
	 }
}
public static class Helper{
	public static UnityEngine.Color GetRangeValue(this IList<UnityEngine.Color> range, float n) {
		float inc = 1.0f/(float)(range.Count-1);
		float mod = n % inc;		
		if (mod ==0 ){
	 		return range[(int)(n/inc)];
		}
		var low = (int)(n/(1.0f/(float)(range.Count-1)));
//		var c = ; var d = range[low];
		// Color c = Color.magenta - Color.blue;//c-d;
		return (range[low]+(range[low+1]-range[low])*(mod/inc));	
	}
}