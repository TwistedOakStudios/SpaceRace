using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class LaneManager : MonoBehaviour {
	public float laneDistance = 3.0f;
	public int numberOfLanes = 8;
    public Transform lanePrefab;
	public List<Transform> lanes;
	
	void Awake() {
        CreateLanes();
	}
	
	void CreateLanes() {
        lanes = new List<Transform>(numberOfLanes);
        for (int i = 0; i < numberOfLanes; i++) {
            Transform lane; 
			if (i == 0) {
				lane = Instantiate(lanePrefab, new Vector3(0, 0, -15), Quaternion.identity) as Transform;
				lane.localScale = new Vector3(80.0f, laneDistance, 0.1f);	
			}
			else {
				lane = Instantiate(lanePrefab, new Vector3(0, i * laneDistance, -15), Quaternion.identity) as Transform;
				lane.localScale = new Vector3(80.0f, laneDistance, 0.1f);
			}
			lane.renderer.material.color = new Color(1.0f - i * 0.15f, 0f, 1.0f, 0.75f);
				
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