using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PotonOven : MonoBehaviour {

    void OnTriggerEnter(Collider other) {
        if (other.transform.name == "OvenTable") {
            Debug.Log("in at");
            Debug.Log(Time.time);
        }
    }

    void OnTriggerExit(Collider other) {
        if (other.transform.name == "OvenTable") {
            Debug.Log("out at");
            Debug.Log(Time.time);
        }
    }
}
