using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

public class Round : MonoBehaviour {
    public List<GameObject> carPrefabs;
	public bool ended;
	private bool canRestart;
	public List<Car> cars;

    public float startingVelocity;
	
	void Start() {
		Static.Events.CarKilled += CarKilled;

		InitializeCars();
		StartRound();
	}
	
	void InitializeCars() {
        cars = new List<Car>();

        for (int i = 0; i < carPrefabs.Count; i++) {
            GameObject car = Instantiate(carPrefabs[i], Vector3.zero, Quaternion.identity) as GameObject;
            car.GetComponent<Car>().Init();
            cars.Add(car.GetComponent<Car>());
        }
	}
	
	public void StartRound() {
        for (int i = 0; i < cars.Count; i++) {
            cars[i].Deploy(Static.LevelData.LaneManager.numberOfLanes / 2 - i, startingVelocity);
        }

        ended = false;
        canRestart = false;
        Static.Events.OnRoundStarted(this);
	}
	
	void CarKilled(Car car) {
        if ((from c in cars select car.isDead).Count() >= carPrefabs.Count - 1) {
            EndRound();
        }
	}

    void EndRound() {
        ended = true;
        Static.Events.OnRoundEnded(this);
        StartCoroutine(WaitBeforeAllowingRestart());
    }
	
	IEnumerator WaitBeforeAllowingRestart() {
		yield return new WaitForSeconds(1.0f);
        canRestart = true;
	}
	
	void Update() {
		if (ended && canRestart) {
			if (Input.GetKeyDown(KeyCode.Space) || Input.touchCount > 0) {
                StartRound();
			}
		}
	}
}

public partial class LevelData {
	public Round round;
	public Round Round {
		get { return round; }
	}
}

public partial class Events {
	public Action<Round> RoundEnded;
	public void OnRoundEnded(Round round) {
		RoundEnded.Invoke(round);
	}
	
	public Action<Round> RoundStarted;
	public void OnRoundStarted(Round round) {
		RoundStarted.Invoke(round);
	}
}