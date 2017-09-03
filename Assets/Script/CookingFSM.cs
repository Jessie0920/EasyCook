using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CookingFSM : MonoBehaviour {

    public GameObject foodheadPrefab;
    public enum GameState { Cooking, Warning, Done, Idle };
    public GameState cookingState;
    public bool isEmpty = true;
    public bool isHalfFull = false;
    public bool canPour = false;

    private int num = 0;
    private bool isOnFire = false;
    private float doneTime = 3.0f;
    private float cookTime = 5.0f;
    //private float warningTime = 11.0f;

    private GameObject objUI;
    private GameObject player;
    private GameObject head;
    private Slider progressSlider;
    private float timer = -1;

    // Use this for initialization
    void Start() {
        objUI = GameObject.Find("UI/ObjectUICanvas");
        if (player == null)
            player = GameObject.FindGameObjectWithTag("Player");
        cookingState = GameState.Idle;
    }

    void AddFood() {
        ++num;
        isEmpty = false;
        if (num == 3) isHalfFull = false;
        else isHalfFull = true;
        if (cookingState == GameState.Done || cookingState == GameState.Warning) {
            cookingState = GameState.Cooking;
            timer = -1;
        }
        if (head == null) {
            head = Instantiate(foodheadPrefab, new Vector3(-500, 0, 0),
                Quaternion.identity, objUI.transform) as GameObject;
            head.transform.Find("FinishImage").gameObject.SetActive(false);
            head.transform.Find("WarningImage").gameObject.SetActive(false);
            head.transform.Find("MaterialImage1").gameObject.SetActive(true);
            head.transform.Find("MaterialImage2").gameObject.SetActive(false);
            head.transform.Find("MaterialImage3").gameObject.SetActive(false);
            progressSlider = head.transform.Find("ProgressBar").GetComponent<Slider>();
            progressSlider.maxValue = cookTime;
            progressSlider.minValue = 0;
            progressSlider.value = 0;
            Vector2 pos = RectTransformUtility.WorldToScreenPoint(Camera.main, transform.position);
            head.GetComponent<RectTransform>().position = pos;
        }
        else {
            if (!progressSlider.IsActive()) {
                if (num == 1)
                    progressSlider.value = 0;
                else progressSlider.value = cookTime / 2;
                head.transform.Find("ProgressBar").gameObject.SetActive(true);
            }
            else progressSlider.value /= 2;
            head.transform.Find("WarningImage").gameObject.SetActive(false);
            head.transform.Find("FinishImage").gameObject.SetActive(false);
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
            AbleFoodHead();
        }
    }

    void DisableFoodHead() {
        head.SetActive(false);
    }

    void AbleFoodHead() {
        head.SetActive(true);
    }

    void PourOutSoup() {
        cookingState = GameState.Idle;
        DisableFoodHead();
        timer = -1;
        num = 0;
        isEmpty = true;
        isHalfFull = false;
        canPour = false;
    }

    void PourInSoup() {
        cookingState = GameState.Done;
        head.transform.Find("FinishImage").gameObject.SetActive(true);
        head.transform.Find("WarningImage").gameObject.SetActive(false);
        head.transform.Find("MaterialImage1").gameObject.SetActive(true);
        head.transform.Find("MaterialImage2").gameObject.SetActive(true);
        head.transform.Find("MaterialImage3").gameObject.SetActive(true);
        head.transform.Find("ProgressBar").gameObject.SetActive(false);
        AbleFoodHead();
        timer = 0;
        num = 3;
        isEmpty = false;
        isHalfFull = false;
        canPour = true;
    }

    void OnTriggerEnter(Collider other) {
        if (other.transform.name == "OvenTable" && this.transform.parent == null) {
            isOnFire = true;
        }
    }

    // Update is called once per frame
    void Update() {
        if (this.transform.parent != null)
            isOnFire = false;
        switch (cookingState) {
            case GameState.Idle:
                if (num != 0 && isOnFire) {
                    if (timer >= doneTime) {
                        head.transform.Find("FinishImage").gameObject.SetActive(false);
                        cookingState = GameState.Warning;
                    }
                    else if (timer == -1)
                        cookingState = GameState.Cooking;
                    else cookingState = GameState.Done;
                }
                break;
            case GameState.Cooking:
                canPour = false;
                if (!isOnFire)
                    cookingState = GameState.Idle;
                if (progressSlider!=null && progressSlider.value == cookTime) {
                    head.transform.Find("ProgressBar").gameObject.SetActive(false);
                    cookingState = GameState.Done;
                    timer = 0;
                }
                else progressSlider.value += Time.deltaTime;
                break;
            case GameState.Warning:
                if ((int)Time.time % 2 == 0)
                    head.transform.Find("WarningImage").gameObject.SetActive(false);
                else head.transform.Find("WarningImage").gameObject.SetActive(true);
                if (!isOnFire) {
                    head.transform.Find("WarningImage").gameObject.SetActive(false);
                    if (num == 3)
                        head.transform.Find("FinishImage").gameObject.SetActive(true);
                    cookingState = GameState.Idle;
                }
                break;
            case GameState.Done:
                if (num == 3) {
                    head.transform.Find("FinishImage").gameObject.SetActive(true);
                    canPour = true;
                }
                if (timer >= doneTime) {
                    head.transform.Find("FinishImage").gameObject.SetActive(false);
                    cookingState = GameState.Warning;
                }
                else timer += Time.deltaTime;
                if (!isOnFire)
                    cookingState = GameState.Idle;
                break;
        }
        if (head != null) {
            Vector2 pos = RectTransformUtility.WorldToScreenPoint(Camera.main, transform.position);
            head.GetComponent<RectTransform>().position = pos;
        }
    }
}
