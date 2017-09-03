using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Cutting : MonoBehaviour {

    public GameObject foodheadPrefab;

    private float cutTime = 5.0f;

    private GameObject objUI;
    private GameObject player;
    private GameObject head;

    private bool isCutting = false;
    public enum GameState 				//食物状态枚举
    { unCut, Cutting, Done };
    public GameState state;
    private Slider progressSlider;
    private float startTime;

    void StartOperate() {
        if (state == GameState.Done)
            isCutting = false;
        else {
            if (state == GameState.unCut) {  //第一次，创建进度条
                head = Instantiate(foodheadPrefab, new Vector3(-500, 0, 0),
                    Quaternion.identity, objUI.transform) as GameObject;
                head.transform.Find("FinishImage").gameObject.SetActive(false);
                head.transform.Find("WarningImage").gameObject.SetActive(false);
                head.transform.Find("MaterialImage1").gameObject.SetActive(false);
                head.transform.Find("MaterialImage2").gameObject.SetActive(false);
                head.transform.Find("MaterialImage3").gameObject.SetActive(false);
                progressSlider = head.transform.Find("ProgressBar").GetComponent<Slider>();
                progressSlider.maxValue = cutTime;
                progressSlider.minValue = 0;
                progressSlider.value = 0;
                Vector2 pos = RectTransformUtility.WorldToScreenPoint(Camera.main, transform.position);
                head.GetComponent<RectTransform>().position = pos;
                state = GameState.Cutting;
            }
            isCutting = true;
            player.SendMessage("StartOperate");
            startTime = Time.time - progressSlider.value;
        }
    }

    void DeleteFoodHead() {
        if (head != null)
            Destroy(head);
    }

    void StopOperate() {
        isCutting = false;
        player.SendMessage("StopOperate");
    }

    void Awake() {
        objUI = GameObject.Find("UI/ObjectUICanvas");
        state = GameState.unCut;
        if (player == null)
            player = GameObject.FindGameObjectWithTag("Player");
    }

	void Update () {
        if (isCutting) {  //更新进度条
            progressSlider.value = Time.time - startTime;
            if (progressSlider.value == cutTime) {
                isCutting = false;
                head.transform.Find("MaterialImage1").gameObject.SetActive(true);
                head.transform.Find("MaterialImage1").localPosition = new Vector3(0,44,0);
                head.transform.Find("ProgressBar").gameObject.SetActive(false);
                this.transform.Find("TomatoRaw").gameObject.SetActive(false);
                this.transform.Find("TomatoCutted").gameObject.SetActive(true);
                state = GameState.Done;
                player.SendMessage("StopOperate");
            }
        }
        //更新位置
        if (head != null) {
            Vector2 pos = RectTransformUtility.WorldToScreenPoint(Camera.main, transform.position);
            head.GetComponent<RectTransform>().position = pos;
        }
	}
}
