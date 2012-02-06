using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CameraController : MonoBehaviour {
	public float velocity;
	public Car criticalCar;
    private Material materialToScroll;
	private List<Car> criticalCars;
	
	void Start() {
		Static.Events.RoundEnded += RoundEnded;
		Static.Events.CriticalPointReached += CriticalPointReached;
		Static.Events.CriticalPointExited += CriticalPointExited;
		Static.Events.MiddlePointReached += MiddlePointReached;
		materialToScroll = Static.LevelData.background.renderer.material;
		criticalCars = new List<Car>();
	}
	
	public void CriticalPointReached(Car car) {
		criticalCars.Add(car);
	}
	
	public void CriticalPointExited(Car car) {
		if (criticalCars.Contains(car	))
			criticalCars.Remove(car);
	}
	
	void MiddlePointReached(Car car) {
		criticalCars.Clear();
	}
	
	public void RemoveCar(Car car) {
		Static.LevelData.Round.cars.Remove(car);
	}
	
	float GetAverageCarVelocity(List<Car> cars) {
		float avg = 0.0f;

		foreach (Car car in cars) {
			avg += car.velocity;	
		}
		avg /= cars.Count;	
		
		return avg;
	}
	
	void MoveCars(bool followCritical) {
		List<Car> carsToMove = Static.LevelData.Round.cars;
		List<Car> carsToAvg;
		if (followCritical) carsToAvg = criticalCars; 
		else carsToAvg = carsToMove;
		
		velocity = GetAverageCarVelocity(carsToAvg);
		
		foreach (Car c in carsToMove) {
			float displacement;
			if (c.draftingBehind) 
				displacement = (c.draftingBehind.velocity - velocity) * Time.deltaTime; 
			else 
				displacement = (c.velocity - velocity) * Time.deltaTime;
			
			c.transform.position += Vector3.right * displacement;
		}
	}
	
	void ScrollBackground() {
		// that magic number at the end is to make the background scrolling line up exactly with the craters scrolling
		materialToScroll.mainTextureOffset += velocity * Vector2.right * Time.deltaTime * 0.0045f;
	}
	
	// Assumes the velocity for the current frame has already been set
	void MovePowerups() {
		foreach (Powerup p in Static.LevelData.PowerupManager.powerups) {
			p.transform.position -= Vector3.right * (velocity + p.speed) * Time.deltaTime;
		}
	}
	
	void LateUpdate () {
		if (!Static.LevelData.Round.ended) {
			ScrollBackground();
			if (criticalCars.Count > 0) MoveCars(true);
			else MoveCars(false);
			MovePowerups();
		}
	}
	
	void RoundEnded(Round r) {
	}
}
