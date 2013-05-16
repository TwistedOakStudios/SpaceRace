using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Stream : MonoBehaviour {
    public Streamer currentStreamer;
    public List<Streamer> streamerPool;

    void Start() {
        streamerPool = new List<Streamer>();
        currentStreamer = GetComponentInChildren<Streamer>(); 
    }

    public void Drop() {
        currentStreamer.dropped = true;
         
    }
}
