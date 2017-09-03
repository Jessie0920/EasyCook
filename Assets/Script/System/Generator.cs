using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class GameTask {
    public GameObject task;
    public Slider s;
    public float createTime;
    public int type;

    public GameTask(float _time = 0.0f,GameObject _go = null,int _type = 0) {
        createTime = _time;
        task = _go;
        type = _type;
    }
}

public class Generator : MonoBehaviour {

    private int maxTaskNum = 5;  //最大任务数
    public float interval;   //生成任务时间间隔
    public float overTime; //超时时间
    private GameTask[] tasks;   //任务数组
    private float timer = 0.0f;    //任务生成计时
    private int curTaskNum = 0;   //当前任务数
    private int newTaskPos = -640;

    private bool isPaused = false;

    public GameObject LeftUpUI;
    public GameObject plateTable;
    private GameObject player;

    public GameObject task0;
    public GameObject task1;

    private void PauseGame() {
        isPaused = true;
    }

    private void ResumeGame() {
        isPaused = false;
    }

    void generateTask() {
        GameObject go;
        int i = Random.Range(0, 2);
        if (i == 0) {    //随机生成不同的任务
            go = Instantiate(task0, new Vector3(-500, 0, 0), 
                Quaternion.identity, LeftUpUI.transform) as GameObject;
            go.transform.localPosition = new Vector3(newTaskPos+95, 0, 0);
            newTaskPos += 190;
        }
        else {
            go = Instantiate(task1, new Vector3(-500, 0, 0),
                Quaternion.identity, LeftUpUI.transform) as GameObject;
            go.transform.localPosition = new Vector3(newTaskPos+110, 0, 0);
            newTaskPos += 220;
        }
        tasks[curTaskNum] = new GameTask(Time.time, go);
        tasks[curTaskNum].s = tasks[curTaskNum].task.transform.GetComponentInChildren<Slider>();
        tasks[curTaskNum].s.maxValue = overTime;
        tasks[curTaskNum].s.minValue = 0;
        tasks[curTaskNum].s.value = overTime;
        tasks[curTaskNum].type = i;
        ++curTaskNum;
    }

    void CompleteTask() {  //完成订单
        Transform tr = player.transform.Find("plate");
        int tasktype = -1;
        foreach (Transform child in tr) {
            if (child.gameObject.activeSelf) {
                if (child.name == "soup")
                    tasktype = 1;
                else tasktype = 0;
                child.gameObject.SetActive(false);
            }
        }
        tr.gameObject.SetActive(false);
        plateTable.SendMessage("ReturnPlate");
        bool complete = false;
        for (int i = 0; i < curTaskNum; ++i) {
            if (tasks[i].type == tasktype) {
                int moveL = tasks[i].type * 30 + 190;
                newTaskPos -= moveL;
                //tips
                if (tasks[i].s.value >= overTime/2)
                    GameManager.gm.AddScore(2);
                else GameManager.gm.AddScore(0);
                Destroy(tasks[i].task);
                for (int j = i; j < curTaskNum - 1; ++j) {
                    tasks[j].s = tasks[j + 1].s;
                    tasks[j].createTime = tasks[j + 1].createTime;
                    tasks[j].type = tasks[j + 1].type;
                    tasks[j].task = tasks[j + 1].task;
                    tasks[j].task.transform.localPosition =
                        new Vector3(tasks[j].task.transform.localPosition.x - moveL, 0, 0);
                }
                tasks[curTaskNum - 1].task = null;
                tasks[curTaskNum - 1] = null;
                --curTaskNum;
                complete = true;
                break;
            }
        }
    }

    void TaskFail() {
        GameManager.gm.MinusScore();
    }

    void Start() {
        if (player == null)
            player = GameObject.FindGameObjectWithTag("Player");
        tasks = new GameTask[maxTaskNum];
        generateTask();
    }

	void Update () {
        if (!isPaused) {
            if (timer >= interval && curTaskNum < maxTaskNum) {
                generateTask();
                timer = 0;
            }
            for (int i = 0; i < curTaskNum; ++i) {  //更新silder显示，判断是否超时
                tasks[i].s.value = overTime - (Time.time - tasks[i].createTime);
                float t = tasks[i].s.value / overTime;
                tasks[i].s.fillRect.transform.GetComponent<Image>().color = new Vector4(1-t,t,0,1);
                if (tasks[i].s.value == 0) {
                    int moveL = tasks[i].type * 30 + 190;
                    newTaskPos -= moveL;
                    TaskFail();
                    Destroy(tasks[i].task);
                    for (int j = i; j < curTaskNum - 1; ++j) {
                        tasks[j].s = tasks[j + 1].s;
                        tasks[j].createTime = tasks[j + 1].createTime;
                        tasks[j].type = tasks[j + 1].type;
                        tasks[j].task = tasks[j + 1].task;
                        tasks[j].task.transform.localPosition =
                            new Vector3(tasks[j].task.transform.localPosition.x - moveL, 0, 0);
                    }
                    tasks[curTaskNum - 1].task = null;
                    tasks[curTaskNum - 1] = null;
                    --curTaskNum;
                }
            }
            timer += Time.deltaTime;
        }
	}

}
