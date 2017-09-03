using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour {

    private Animator animator;		
    public float moveSpeed;
    private bool isPaused = false;

	// Use this for initialization
    void Start()
    {
        animator = GetComponent<Animator>();	//获取玩家Animator组件
    }

    void PauseGame() {
        isPaused = true;
    }

    void ResumeGame() {
        isPaused = false;
    }
	
	// Update is called once per frame
	void Update () {
       if (!isPaused) {
            float h = Input.GetAxisRaw("Horizontal");
            float v = Input.GetAxisRaw("Vertical");
            if (v > 0) {
                transform.rotation = Quaternion.Euler(0, 0, 0);
                transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);
            }
            else if (v < 0) {
                transform.rotation = Quaternion.Euler(0, 180, 0);
                transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);
            }
            if (h > 0) {
                transform.rotation = Quaternion.Euler(0, 90, 0);
                transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);
            }
            else if (h < 0) {
                transform.rotation = Quaternion.Euler(0, -90, 0);
                transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);
            }

            if (v != 0.0f || h != 0.0f)
                animator.SetBool("isMove", true);
            else animator.SetBool("isMove", false);
        }
	}
}
