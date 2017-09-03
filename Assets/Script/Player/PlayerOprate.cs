using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityStandardAssets.CrossPlatformInput;

public class PlayerOprate : MonoBehaviour {

    private Animator animator;

    private bool isPaused = false;
    private bool isOperating = false;
    private bool isHolding = false;
    private bool isMoving = false;
    private GameObject gameManager;
    private GameObject operatingObj;

    // Use this for initialization
    void Start() {
        gameManager = GameObject.Find("GameManager"); 
        animator = GetComponent<Animator>();
    }

    void PauseGame() {
        isPaused = true;
    }

    void ResumeGame() {
        isPaused = false;
    }

    void ChangeHolding(bool state) {
        isHolding = state;
        if (isHolding == true && operatingObj != null)
            operatingObj.SendMessage("StopOperate");
    }

    void ChangeMoving(bool state) {
        isMoving = state;
    }

    void StartOperate() {
        isOperating = true;
        animator.SetBool("isOperating", true);
    }

    void StopOperate() {
        isOperating = false;
        animator.SetBool("isOperating", false);
    }


	
	// Update is called once per frame
	void Update () {
        if (!isPaused && !isHolding) {
            if (!isOperating) {
                if (Input.GetKeyDown(KeyCode.Space)) {
                    RaycastHit hitInfo;
                    Vector3 forwardRay = new Vector3(transform.position.x, transform.position.y + 0.08f, transform.position.z);
                    if (Physics.Raycast(forwardRay, transform.forward, out hitInfo, 1.5f)) {
                        if (hitInfo.transform.name == "CutTable1" ||
                                    hitInfo.transform.name == "CutTable2") {
                            RaycastHit TableHitInfo;
                            if (Physics.Raycast(hitInfo.transform.position, Vector3.up, out TableHitInfo, 1.0f)) {
                                operatingObj = TableHitInfo.transform.gameObject;
                                operatingObj.SendMessage("StartOperate");
                            }
                        }
                        if (hitInfo.transform.name == "Sink") {
                            operatingObj = hitInfo.transform.gameObject;
                            operatingObj.SendMessage("StartOperate");
                        }
                    }
                }
            }
            else {
                if ( Input.anyKeyDown ) {
                    operatingObj.SendMessage("StopOperate");
                    isOperating = false;
                }
            }
        }
	}
}
