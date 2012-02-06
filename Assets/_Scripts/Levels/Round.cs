using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class Round : MonoBehaviour {
	public int numPlayers = 2;
	public GameObject carPrefab1;
	public GameObject carPrefab2;
	public bool ended;
	private bool canRestart;
	public List<Car> cars;
	public List<Car> killedCars;
	
	void Start() {
		Static.Events.CarKilled += CarKilled;

		InitializeCars();
		StartRound();
	}
	
	void InitializeCars() {
		cars = new List<Car>();
		killedCars = new List<Car>();
		
		for (int i = 0; i < cars.Count; i++) {
			Car car = cars[i];
			cars.Remove(car);
			Destroy(car.gameObject);
		}
		
		for (int i = 0; i < numPlayers; i++) {
			GameObject car;
			if (i == 0) car = Instantiate(carPrefab1, Vector3.zero, Quaternion.identity) as GameObject;
			else car = Instantiate(carPrefab2, Vector3.zero, Quaternion.identity) as GameObject;
			cars.Add(car.GetComponent<Car>());
		}
	}
	
	public void StartRound() {
		for (int i = 0; i < killedCars.Count; i++) {
			cars.Add(killedCars[i]);
			killedCars[i].gameObject.SetActiveRecursively(true);
		}
		killedCars.Clear();
		
		for (int i = 0; i < cars.Count; i++) {
			Car car = cars[i];
			car.Init();
			car.Deploy(Static.LevelData.LaneManager.numberOfLanes/2 - i);
			//car.Deploy(i);
		}
		
		ended = false;
		canRestart = false;
		Static.Events.OnRoundStarted(this);
	}
	
	void CarKilled(Car car) {
		cars.Remove(car);
		killedCars.Add(car);
		if (cars.Count <= 1) {
			StartCoroutine(EndRound());
		}
	}
	
	IEnumerator EndRound() {
		ended = true;
		Static.Events.OnRoundEnded(this);
		yield return new WaitForSeconds(1.0f);	
		canRestart = true;
	}
	
	void Update() {
		if (ended && canRestart) {
			if (Input.GetKeyDown(KeyCode.Space) || Input.touchCount > 0) {
				Static.LevelData.Round.StartRound();
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