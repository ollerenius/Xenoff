using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Event {

    List<Subscriber> subs = new List<Subscriber>();

    public void TriggerEvent() {
            
    }

}
