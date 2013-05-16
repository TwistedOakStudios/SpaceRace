using UnityEngine;
using System.Collections;

public class Background : MonoBehaviour {
    void Start() {
        Static.Events.CameraUpdateComplete += Scroll;
    }

	void Scroll(RaceCamera camera) {
        renderer.material.mainTextureOffset -= camera.velocity * Vector2.right * Time.deltaTime * 0.0045f;
	}
}