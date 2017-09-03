using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Cooking : MonoBehaviour {

    public GameObject foodheadPrefab;

    private float cooktime = 5.0f;

    private GameObject ObjUI;
    private GameObject player;
    private GameObject head;
    private Slider progressSlider;

    public int num = 0;
    public bool isDone = false;

    private bool isCooking = false;

	// Use this for initialization
	void Start () {
        ObjUI = GameObject.Find("UI/ObjectUICanvas");
        if (player == null)
            player = GameObject.FindGameObjectWithTag("Player");
	}

    void addfood() {
        ++num;
        isDone = false;
        if (head == null) {
            head = Instantiate(foodheadPrefab, new Vector3(-500, 0, 0),
                Quaternion.identity, ObjUI.transform) as GameObject;
            head.transform.Find("FinishImage").gameObject.SetActive(false);
            head.transform.Find("WarningImage").gameObject.SetActive(false);
            head.transform.Find("MaterialImage1").gameObject.SetActive(true);
            head.transform.Find("MaterialImage2").gameObject.SetActive(false);
            head.transform.Find("MaterialImage3").gameObject.SetActive(false);
            progressSlider = head.transform.Find("ProgressBar").GetComponent<Slider>();
            progressSlider.maxValue = cooktime;
            progressSlider.minValue = 0;
            progressSlider.value = 0;
            Vector2 pos = RectTransformUtility.WorldToScreenPoint(Camera.main, transform.position);
            head.GetComponent<RectTransform>().position = pos;
        }
        else {
            if (!progressSlider.IsActive()) {
                if (num == 1)
                    progressSlider.value = 0;
                else progressSlider.value = cooktime / 2;
                head.transform.Find("ProgressBar").gameObject.SetActive(true);
            }
            else progressSlider.value /= 2;
            switch (num) {
                case 1:
                    head.transform.Find("MaterialImage1").gameObject.SetActive(true);
                    head.transform.Find("MaterialImage2").gameObject.SetActive(false);
                    head.transform.Find("MaterialImage3").gameObject.SetActive(false);
                    break;
                case 2:
                    head.transform.Find("MaterialImage1").gameObject.SetActive(true);
                    head.transform.Find("MaterialImage2").gameObject.SetActive(true);
                    head.transform.Find("MaterialImage3").gameObject.SetActive(false);
                    break;
                case 3:
                    head.transform.Find("MaterialImage1").gameObject.SetActive(true);
                    head.transform.Find("MaterialImage2").gameObject.SetActive(true);
                    head.transform.Find("MaterialImage3").gameObject.SetActive(true);
                    break;
            }
            ableFoodHead();
        }
    }

    void disableFoodHead() {
        head.SetActive(false);
    }

    void ableFoodHead() {
        head.SetActive(true);
    }
	
	// Update is called once per frame
	void Update () {
        if (isCooking) {
            if (progressSlider != null && progressSlider.value == cooktime) {
                head.transform.Find("ProgressBar").gameObject.SetActive(false);
                isDone = true;
            }
            else {
                if (num != 0) {
                    progressSlider.value += Time.deltaTime;
                }
            }
        }
        //更新位置
        if (head != null) {
            Vector2 pos = RectTransformUtility.WorldToScreenPoint(Camera.main, transform.position);
            head.GetComponent<RectTransform>().position = pos;
        }
	}

    void OnTriggerEnter(Collider other) {
        if (other.transform.name == "OvenTable" && this.transform.parent == null) {
            if (!isCooking)
                isCooking = true;
        }
    }

    void OnTriggerExit(Collider other) {
        if (other.transform.name == "OvenTable") {
            if (isCooking)
                isCooking = false;
        }
    }
}
