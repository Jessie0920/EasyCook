using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitAnime : MonoBehaviour {

    Renderer MR;
    float moveSpeed = 0.4f;

    // Use this for initialization
    void Start() {
        MR = this.GetComponent<Renderer>();
    }

    // Update is called once per frame
    void Update() {
        MR.material.mainTextureOffset += new Vector2(Time.deltaTime * moveSpeed, 0);
    }
}
