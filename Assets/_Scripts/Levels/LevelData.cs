using UnityEngine;
using System.Collections;

public partial class LevelData : MonoBehaviour {
	public Transform background;
	public Transform criticalZone;
	public Transform deathZone;
	
	void Awake() {
        if (Static.LevelData) {
            Debug.LogError("level data already exists");
        }
        Static.LevelData = this;
    }
}

public partial class Static {
	public static LevelData LevelData { get; set; }
} 