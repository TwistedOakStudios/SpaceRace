using UnityEngine;
using System;
using System.Collections;

public class Car : MonoBehaviour {
	static float baseVelocity = 30.0f;
	static float brakeAmount = 30.0f;
	static float brakeTime = 0.25f;
	public float velocity;
	private float storedVelocity;
	public int currentLaneIndex;
    public Lane currentLane;
	public int nextLane;
	public bool isMoving = false;
	private float laneChangeTime = 0.05f;
	public float sideKillTime = 0.5f;
	public float moveTime = 0.0f;
	public float draftBonus = 0.0f;
	public Car draftingBehind;
	public ParticleEmitter backParticles;
	public ParticleEmitter sideParticles;
	private Transform d;
	private RayCaster[] rayCasters;
	private KeyboardControl control;
	float laneDistance;
	
	void DeathZoneReached() {
		Kill(true);
	}
	
	public void Init() {
		control = GetComponent<KeyboardControl>();
	}
	
	public void Deploy(int laneIndex) {
		gameObject.SetActiveRecursively(true);
		control.enabled = true;
		laneDistance = Static.LevelData.LaneManager.laneDistance;
		currentLaneIndex = laneIndex;
		transform.position = new Vector3(0.0f, currentLaneIndex * laneDistance, -17.0f);
		transform.rotation = Quaternion.Euler(new Vector3(270, 0, 0));
		currentLane = Static.LevelData.LaneManager.GetLane(currentLaneIndex);
		velocity = baseVelocity;
		rayCasters = gameObject.GetComponentsInChildren<RayCaster>();
	}
	
	void DriveOffOnVictory() {
		float displacement = velocity  * Time.deltaTime;
		transform.position += Vector3.right * displacement;
	}
	
	public void MoveUp() {
		Move(1);
	}
	
	public void MoveDown() {
		Move(-1);
	}

	public void Move (int direction) {	
		if (isMoving) return;
			
		if (Static.LevelData.LaneManager.IsValidLane(currentLaneIndex + direction)) {
			bool canMove = true;
			foreach (RayCaster r in rayCasters) {
				if (r.CarInAdjacentLane(direction)) {
					canMove = false;
					break;
				}
			}
			if (canMove) StartCoroutine(Moving(direction)); 
		}
		
	}
	
	public void Kill(bool diedInBack) {
		if (diedInBack) Instantiate(backParticles, transform.position, Quaternion.identity);
		else Instantiate(sideParticles, transform.position, Quaternion.identity);
		isMoving = false;
		gameObject.SetActiveRecursively(false);
		Static.Events.OnCarKilled(this);
	}
	
    public void ChangeToLane(int laneIndex) {
        currentLaneIndex = laneIndex;
        currentLane = Static.LevelData.LaneManager.GetLane(currentLaneIndex);
    }

	public IEnumerator AccelerateByVelocityOverTime(float aVelocity, float time) {
		// Linear acceleration for now.
		velocity += aVelocity;
		yield break;
		/*
		float t = 0.0f;
		while (t < time) {
			velocity += velocity / time;
			Debug.Log(velocity);
			t += Time.deltaTime;
			yield return null;
		}*/
	}
	
	public void HitCrater() {
		// FIXME: if i'm a crtical car, i MAY NOT BE REMOVED from the cameras CRITICAL CAR LIST'
		Kill(false);
	}
	
	public IEnumerator Moving(int direction) {
		isMoving = true;
		nextLane = currentLaneIndex + direction;
		
		if (draftingBehind) {
			draftingBehind = null;
			velocity = storedVelocity;
		}
		
		moveTime = 0.0f;
		while (moveTime < laneChangeTime) {
			moveTime += Time.deltaTime;
			float currentY = Mathf.SmoothStep(currentLaneIndex * laneDistance, laneDistance * nextLane, moveTime * (1/laneChangeTime));
			transform.position = new Vector3(transform.position.x, currentY, transform.position.z);
			yield return 1;
		}
		
		
        this.ChangeToLane(nextLane);
		isMoving = false;
		moveTime=0.0f;
		yield break;
	}
	
	public IEnumerator Brake() {
		velocity -= brakeAmount;
		
		float time = 0.0f;
		
		while (time < brakeTime) {
			time += Time.deltaTime;
			yield return null;
		}
		
		velocity += brakeAmount;
	}
	
	void OnTriggerEnter(Collider other) {
		Car otherCar = other.GetComponent<Car>();
		if (otherCar) {
			if (currentLaneIndex == otherCar.currentLaneIndex) {
				if (other.transform.position.x > transform.position.x) {
					StartDrafting(otherCar);
				}
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (Static.LevelData.Round.ended) 
			DriveOffOnVictory();
			
		if (draftingBehind) {
			// if the other guy's velocity is greater, you no longer draft. if he slows down you slow down.
			if (draftingBehind.currentLaneIndex != currentLaneIndex) StopDrafting();
			if (draftingBehind.velocity > storedVelocity) StopDrafting();
			else if (draftingBehind.velocity < velocity)
				velocity = draftingBehind.velocity;
		}
	}

	void StartDrafting(Car otherCar) {
		draftingBehind = otherCar;
		storedVelocity = velocity;
		velocity = otherCar.velocity;
	}

	void StopDrafting() {
		draftingBehind = null;
		velocity = storedVelocity;
		storedVelocity = 0.0f;
	}
}

public partial class Events {
	public Action<Car> CarKilled;
	public void OnCarKilled(Car car) {
		CarKilled.Invoke(car);
	}
}
