using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;

public partial class Events : MonoBehaviour {

}

public partial class Static {
    public Events events;
    public static Events Events {
        get {
            return instance.events;
        }
    }
}

