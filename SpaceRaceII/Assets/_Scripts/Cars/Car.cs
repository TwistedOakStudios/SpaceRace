using UnityEngine;
using System;
using System.Collections;

public class Car : MonoBehaviour {
	static float brakeAmount = 30.0f;
	static float brakeTime = 0.25f;

    public bool critical;
    public float velocity;

    public Lane currentLane;
    public int nextLane;
    public bool isMoving = false;
    public float sideKillTime = 0.5f;
    public float moveTime = 0.0f;
    public float draftBonus = 0.0f;
    public Car draftingBehind;
    public ParticleEmitter backParticles;
    public ParticleEmitter sideParticles;
    public int currentLaneIndex;
    
    private int startingX = 512;
    private int zLaneOffset = -2;
	private float storedVelocity;
	private float laneChangeTime = 0.05f;
	private Transform d;
	private RayCaster[] rayCasters;
	private KeyboardControl control;
	float laneDistance;
    Stream stream;

    tk2dSprite sprite;

    public void Init() {
        control = GetComponent<KeyboardControl>();
        sprite = GetComponent<tk2dSprite>();
        stream = GetComponentInChildren<Stream>();
        rayCasters = gameObject.GetComponentsInChildren<RayCaster>();
        Static.Events.MiddlePointReached += MiddlePointReached;
        Static.Events.CriticalPointReached += CriticalPointReached;
        Static.Events.CriticalPointExited += CriticalPointExited;
        Static.Events.DeathZoneReached += DeathZoneReached;
        Static.Events.CameraUpdateComplete += UpdatePosition;
    }

    public void Deploy(int laneIndex, float startingVelocity) {
        gameObject.SetActiveRecursively(true);
        control.enabled = true;
        transform.parent = Static.LevelData.LaneManager.transform;
        Vector3 lp = Static.LevelData.LaneManager.GetLanePos(laneIndex);
        transform.localPosition = new Vector3(startingX, lp.y, lp.z + zLaneOffset);
        currentLaneIndex = laneIndex;
        currentLane = Static.LevelData.LaneManager.GetLane(laneIndex);
        velocity = startingVelocity;
    }

    // Any car entering the middle point means all cars are no longer critical
    void MiddlePointReached(Car car) {
        critical = false;
    }

    void CriticalPointReached(Car car) {
        if (car == this) {
            critical = true;
        }
    }

    void CriticalPointExited(Car car) {
        if (car == this) {
            critical = false;
        }
    }

    void UpdatePosition(RaceCamera camera) {
        if (!draftingBehind) {
            transform.position += Vector3.right * (velocity - camera.velocity) * Time.deltaTime; 
        } else {
            transform.position += Vector3.right * (draftingBehind.velocity - velocity) * Time.deltaTime;
        }
    }
	
	void DeathZoneReached(GameObject go) {
        if (go == gameObject) {
            Kill(true);
        }
	}

    void Update() {
        // TODO: decide if we want continual acceleration
        //velocity += Time.deltaTime * 1.5f;

        if (currentLane.isPower) {
            velocity += Time.deltaTime * currentLane.relativeVelocity;
        }

        if (Static.LevelData.Round.ended) {
            DriveOffOnVictory();
        }

        if (draftingBehind) {
            // if the other guy's velocity is greater, you no longer draft. if he slows down you slow down.
            if (draftingBehind.currentLaneIndex != currentLaneIndex) StopDrafting();
            if (draftingBehind.velocity > storedVelocity) StopDrafting();
            else if (draftingBehind.velocity < velocity)
                velocity = draftingBehind.velocity;
        }
    }

    void OnTriggerEnter(Collider other) {
        Car otherCar = other.GetComponent<Car>();
        if (otherCar) {
            HandleCarCollision(otherCar);
        }
    }

    void HandleCarCollision(Car otherCar) {
        if (currentLaneIndex == otherCar.currentLaneIndex) {
            if (otherCar.transform.position.x > transform.position.x) {
                StartDrafting(otherCar);
            }
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
            if (canMove) {
                StartCoroutine(Moving(direction));
                //stream.Drop();
            }
		}
		
	}
	
	public void Kill(bool diedInBack) {
        // TODO: back explosion
		//if (diedInBack) Instantiate(backParticles, transform.position, Quaternion.identity);
		//else Instantiate(sideParticles, transform.position, Quaternion.identity);
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
        critical = false;
		Kill(false);
	}
	
	public IEnumerator Moving(int direction) {
		isMoving = true;
		nextLane = currentLaneIndex + direction;

        TurnHead(direction);
		
		if (draftingBehind) {
			draftingBehind = null;
			velocity = storedVelocity;
		}
		
		moveTime = 0.0f;
		while (moveTime < laneChangeTime) {
			moveTime += Time.deltaTime;
			float currentY = Mathf.SmoothStep(Static.LevelData.LaneManager.GetLanePos(currentLaneIndex).y, 
                                              Static.LevelData.LaneManager.GetLanePos(nextLane).y, 
                                              moveTime * (1/laneChangeTime));
            float currentZ = Mathf.SmoothStep(Static.LevelData.LaneManager.GetLanePos(currentLaneIndex).z + zLaneOffset,
                                              Static.LevelData.LaneManager.GetLanePos(nextLane).z + zLaneOffset,
                                              moveTime * (1 / laneChangeTime));
			transform.localPosition = new Vector3(transform.localPosition.x, currentY, currentZ);
			yield return 1;
		}
		
		
        this.ChangeToLane(nextLane);
		isMoving = false;
		moveTime=0.0f;
        StartCoroutine(ResetHead());
		yield break;
	}

    void TurnHead(int direction) {
        if (direction < 0) {
            sprite.spriteId = 2;
        } else {
            sprite.spriteId = 1;
        }
    }

    public IEnumerator ResetHead() {
        float time = 0.0f;

        while (time < 0.3f) {
            time += Time.deltaTime;
            yield return null;
        }
        sprite.spriteId = 0;
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
}

public partial class Events {
	public Action<Car> CarKilled;
	public void OnCarKilled(Car car) {
		CarKilled.Invoke(car);
	}
}