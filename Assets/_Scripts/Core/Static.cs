using UnityEngine;
using System.Collections;

public partial class Static : MonoBehaviour {
	public static Static instance;
    void Awake() {
        if (instance != null) {
            Destroy(gameObject);
            return;
        }
        instance = this;
		DontDestroyOnLoad(gameObject);
    }
}