using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LaneManager : MonoBehaviour {
	public int laneDistance;
    public int yPlacementOnLane;
    public int zLaneDistance; 
	public int numberOfLanes;
    public Lane lanePrefab;
	public List<Lane> lanes;
	
	public Color[] lerpPoints = new Color[]{Color.red, Color.blue};
    public float hueBottom = 0.93f;
    public float hueTop = 0.5f;
	
	void Awake() {
        CreateLanes();
	}
	
	void CreateLanes() {
        lanes = new List<Lane>(numberOfLanes);
     
        for (int i = 0; i < numberOfLanes; i++) {
			var lane = Instantiate(lanePrefab, Vector3.zero, Quaternion.identity) as Lane;
			//lane.GetComponent<tk2dSprite>().color = lerpPoints.GetRangeValue((float)i/(float)numberOfLanes);
            float h = Mathf.Lerp(hueBottom, hueTop, (float)i / (float)numberOfLanes);
            var hsb = new HSBColor(0.6f, h, 1.0f, 0.3f);
            lane.GetComponent<tk2dSprite>().color = hsb.ToColor();
            lane.transform.parent = transform;
            lane.transform.localPosition = new Vector3(0, i * laneDistance, i * zLaneDistance);
            if (i == 0 || i == 4) {
                lane.isPower = true;
            }
            lanes.Add(lane);
        }
	}

    public bool IsValidLane(int laneNumber) {
        return (laneNumber >= 0 && laneNumber < numberOfLanes);
	}

    public Vector3 GetLanePos(int laneNumber) {
        return new Vector3(0, laneDistance * laneNumber + yPlacementOnLane, laneNumber * zLaneDistance);
    }

    public Lane GetLane(int laneNumber) {
	 	if(IsValidLane(laneNumber)) 
			return lanes[laneNumber];
		else 
			return null; 
    }

    public void Update() {

    }
}

public partial class LevelData {
	public LaneManager laneManager;	
	public LaneManager LaneManager {
	 	get { return laneManager; }
	 }
}

public static class Helper {
	public static UnityEngine.Color GetRangeValue(this IList<UnityEngine.Color> range, float n) {
		float inc = 1.0f/(float)(range.Count-1);
		float mod = n % inc;		
		if (mod == 0){
	 		return range[(int)(n/inc)];
		}
		var low = (int)(n/(1.0f/(float)(range.Count-1)));
		return (range[low]+(range[low+1]-range[low])*(mod/inc));	
	}
}