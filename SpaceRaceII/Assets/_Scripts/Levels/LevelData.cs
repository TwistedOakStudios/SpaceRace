using UnityEngine;
using System.Collections;

public partial class LevelData : MonoBehaviour {
	public Transform background;
	public Transform criticalZone;
    public Transform middleZone;
	public Transform deathZone;
}

public partial class Static {
    public LevelData levelData;
	public static LevelData LevelData { 
        get {
            return instance.levelData;
        }
    }
} 