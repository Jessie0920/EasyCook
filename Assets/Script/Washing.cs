using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Washing : MonoBehaviour {

    private GameObject[] cleanPlates; //由水池管理的盘子对象池
    public GameObject platePrefab;
    public GameObject foodheadPrefab;
    private Vector3 pos1 = new Vector3(28f, 1.05f, 16f);
    private Vector3 pos2 = new Vector3(29f, 1.05f, 16f);

    private float washTime = 5.0f;

    private GameObject player;
    private GameObject objUI;
    private GameObject head;
    private Slider progressSlider;
    private int cnt = 0;
    private bool isOperating = false;
    private float startTime;

	// Use this for initialization
	void Start () {
        cleanPlates = new GameObject[2];
        GameObject p0 = Instantiate(platePrefab,
                pos1, Quaternion.identity) as GameObject;
        p0.name = "plate";
        GameObject p1 = Instantiate(platePrefab,
                pos2, Quaternion.identity) as GameObject;
        p1.name = "plate";
        cleanPlates[0] = p0;
        cleanPlates[1] = p1;
        objUI = GameObject.Find("UI/ObjectUICanvas");
        if (player == null)
            player = GameObject.FindGameObjectWithTag("Player");
	}

    void AddPlate(int num) {
        cnt += num;
        if (head == null) {
            head = Instantiate(foodheadPrefab, new Vector3(-500, 0, 0),
                Quaternion.identity, objUI.transform) as GameObject;
            head.transform.Find("FinishImage").gameObject.SetActive(false);
            head.transform.Find("WarningImage").gameObject.SetActive(false);
            head.transform.Find("MaterialImage1").gameObject.SetActive(false);
            head.transform.Find("MaterialImage2").gameObject.SetActive(false);
            head.transform.Find("MaterialImage3").gameObject.SetActive(false);
            progressSlider = head.transform.Find("ProgressBar").GetComponent<Slider>();
            progressSlider.maxValue = washTime;
            progressSlider.minValue = 0;
            progressSlider.value = 0;
            Vector2 pos = RectTransformUtility.WorldToScreenPoint(Camera.main, transform.position);
            head.GetComponent<RectTransform>().position = pos;
        }
        else {
            if (!progressSlider.IsActive()) {
                head.transform.Find("ProgressBar").gameObject.SetActive(true);
                progressSlider.value = 0;
            }
        }
    }

    void StartOperate() {
        if (cnt != 0) {
            player.SendMessage("StartOperate");
            startTime = Time.time - progressSlider.value;
            isOperating = true;
        }
    }

    void StopOperate() {
        player.SendMessage("StopOperate");
        isOperating = false;
    }
	
	// Update is called once per frame
	void Update () {
        if (isOperating) {
            progressSlider.value = Time.time - startTime;
            if (progressSlider.value == washTime) {
                --cnt;
                if (cnt == 0) {
                    isOperating = false;
                    head.transform.Find("ProgressBar").gameObject.SetActive(false);
                    player.SendMessage("StopOperate");
                    this.transform.Find("Plate").gameObject.SetActive(false);
                }
                else {
                    progressSlider.value = 0;
                    startTime = Time.time;
                }
                for (int i = 0; i < 2; ++i) {
                    if (!cleanPlates[i].activeSelf) {
                        cleanPlates[i].transform.position = new Vector3(31f, 1.25f, 18.87f);
                        cleanPlates[i].GetComponent<Collider>().enabled = true;
                        cleanPlates[i].GetComponent<Rigidbody>().isKinematic = false;
                        cleanPlates[i].SetActive(true);
                        break;
                    }
                }
            }
        }
	}
}
