using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityStandardAssets.CrossPlatformInput;

public class PlayerPick2 : MonoBehaviour {

    public GameObject tomatoPrefab;

    private bool isPaused = false;
    private bool isHolding = false;
    public float maxDist;
    private Vector3 holdingPos = new Vector3(0, 1.389f, 0.744f);
    private GameObject holdingObj = null;
    private GameObject operatingTable = null;
    private GameObject gameManager;

    // Use this for initialization
    void Start() {
        gameManager = GameObject.Find("GameManager");
    }

    void PauseGame() {
        isPaused = true;
    }

    void ResumeGame() {
        isPaused = false;
    }

    void PickUpObj(GameObject obj) {
        isHolding = true;
        GetComponent<IKController>().isActive = true;
        holdingObj = obj;
        holdingObj.transform.SetParent(this.transform);
        holdingObj.transform.localPosition = holdingPos;
        holdingObj.transform.rotation = Quaternion.identity;
        Collider[] colliders = holdingObj.GetComponents<Collider>();
        foreach (Collider c in colliders)
           c.enabled = false;
        Rigidbody r = holdingObj.GetComponent<Rigidbody>();
        if (r!=null)
            r.isKinematic = true;
    }

    void GetObj(string name) {
        GameObject obj = Instantiate(tomatoPrefab, new Vector3(0, 0, 0), Quaternion.identity);
        PickUpObj(obj);
        obj.name = "Tomato";
    }

    void DropObj() {
        isHolding = false;
        GetComponent<IKController>().isActive = false;
        holdingObj.transform.parent = null;
        Collider[] colliders = holdingObj.GetComponents<Collider>();
        foreach (Collider c in colliders)
            c.enabled = true;
        Rigidbody r = holdingObj.GetComponent<Rigidbody>();
        if (r != null)
            r.isKinematic = false;
        holdingObj = null;
    }

    void PutObj(GameObject OperatingTable) {
        isHolding = false;
        GetComponent<IKController>().isActive = false;
        holdingObj.transform.parent = null;
        Vector3 tablepos = OperatingTable.transform.position;
        float objHeight = holdingObj.GetComponent<Renderer>().bounds.size.y/2;
        float tableHeight = OperatingTable.GetComponent<Renderer>().bounds.size.y/2+0.1f;
        holdingObj.transform.position = new Vector3(tablepos.x,tablepos.y + tableHeight + objHeight,tablepos.z);
        holdingObj.transform.rotation = Quaternion.identity;
        Collider[] colliders = holdingObj.GetComponents<Collider>();
        foreach (Collider c in colliders)
            c.enabled = true;
        Rigidbody r = holdingObj.GetComponent<Rigidbody>();
        if (r != null)
            r.isKinematic = false;
        holdingObj = null;
    }

    // Update is called once per frame
    void Update() {
        if (!isPaused) {
            if  (Input.GetKeyDown(KeyCode.LeftControl)) {
                RaycastHit hitInfo;
                Vector3 A = new Vector3(transform.position.x, transform.position.y + 0.05f, transform.position.z);
                if (Physics.Raycast(A, transform.forward, out hitInfo, maxDist)) {
                    //pick up object on the ground
                    if (hitInfo.transform.CompareTag("Food") 
                        || hitInfo.transform.CompareTag("Plate")
                        || hitInfo.transform.CompareTag("Pot") ) {
                        if (!isHolding) {
                            GameObject obj = hitInfo.transform.gameObject;
                            PickUpObj(obj);
                        }
                        else {
                            DropObj();
                        }
                    }
                    else if (hitInfo.transform.CompareTag("Table")) {
                        RaycastHit TableHitInfo;
                        if (Physics.Raycast(hitInfo.transform.position, Vector3.up, out TableHitInfo, 1.0f)) {
                            GameObject go = TableHitInfo.transform.gameObject;
                            if (!isHolding) {       //空手，拿起桌上的物体
                                PickUpObj(go);
                            }
                            else {                //手上有物体，判断桌上有什么
                                if (holdingObj.CompareTag("Pot")) {    //锅
                                    if (holdingObj.GetComponent<CookingFSM>().isEmpty) {
                                        if (go.CompareTag("Food")
                                            && go.transform.Find("TomatoCutted").gameObject.activeSelf) {
                                                holdingObj.SendMessage("AddFood");
                                                holdingObj.transform.Find("soup").gameObject.SetActive(true);
                                                go.SendMessage("DeleteFoodHead");
                                                Destroy(go);
                                        }
                                        else if (go.transform.name == "plate"
                                                && go.transform.Find("soup").gameObject.activeSelf) {
                                            go.transform.Find("soup").gameObject.SetActive(false);
                                            holdingObj.transform.Find("soup").gameObject.SetActive(true);
                                            holdingObj.SendMessage("PourInSoup");
                                        }
                                    }
                                    else if (holdingObj.GetComponent<CookingFSM>().canPour) {
                                        if (go.transform.name == "plate"
                                            && !go.transform.Find("pieces").gameObject.activeSelf
                                            && !go.transform.Find("soup").gameObject.activeSelf) {
                                            go.transform.Find("soup").gameObject.SetActive(true);
                                            holdingObj.transform.Find("soup").gameObject.SetActive(false);
                                            holdingObj.SendMessage("PourOutSoup");
                                        }
                                    }
                                    else if (holdingObj.GetComponent<CookingFSM>().isHalfFull) {
                                        if (go.CompareTag("Food")
                                            && go.transform.Find("TomatoCutted").gameObject.activeSelf) {
                                            holdingObj.SendMessage("AddFood");
                                            holdingObj.transform.Find("soup").gameObject.SetActive(true);
                                            go.SendMessage("DeleteFoodHead");
                                            Destroy(go);
                                        }
                                    }
                                }
                                else if (holdingObj.name == "plate") {  //盘子
                                    if (holdingObj.transform.Find("soup").gameObject.activeSelf) {
                                        if (go.CompareTag("Pot")) {
                                            if (go.GetComponent<CookingFSM>().isEmpty) {
                                                holdingObj.transform.Find("soup").gameObject.SetActive(false);
                                                go.transform.Find("soup").gameObject.SetActive(true);
                                                go.SendMessage("PourInSoup");
                                            }
                                        }
                                    }
                                    else if (!holdingObj.transform.Find("pieces").gameObject.activeSelf) {
                                        if (go.CompareTag("Pot")) {
                                            if (go.GetComponent<CookingFSM>().canPour) {
                                                holdingObj.transform.Find("soup").gameObject.SetActive(true);
                                                go.transform.Find("soup").gameObject.SetActive(false);
                                                go.SendMessage("PourOutSoup");
                                            }
                                        }
                                        else if (go.CompareTag("Food")
                                            && go.transform.Find("TomatoCutted").gameObject.activeSelf) {
                                            go.SendMessage("DeleteFoodHead");
                                            Destroy(go);
                                            holdingObj.transform.Find("pieces").gameObject.SetActive(true);
                                        }
                                    }
                                    /*else {
                                        if (go.CompareTag("Pot")
                                            && go.GetComponent<CookingFSM>().isHalfFull) {
                                            go.SendMessage("AddFood"); 
                                            go.transform.Find("soup").gameObject.SetActive(true);
                                            HoldingObj.transform.Find("pieces").gameObject.SetActive(false);
                                        }
                                    }*/
                                }
                                else if (holdingObj.name == "DirtyPlate") {   //脏盘子
                                    if (go.name == "DirtyPlate") {
                                        holdingObj.transform.SetParent(go.transform);
                                        holdingObj.transform.GetComponent<Collider>().enabled = false;
                                        holdingObj.transform.GetComponent<Rigidbody>().isKinematic = true;
                                        holdingObj.transform.localPosition = new Vector3(0, 0.1f, 0);
                                        isHolding = false;
                                        GetComponent<IKController>().isActive = false;
                                        holdingObj = null;
                                    }
                                }
                                else if (holdingObj.CompareTag("Food")   //（切好的）番茄
                                    && holdingObj.transform.Find("TomatoCutted").gameObject.activeSelf) {
                                    if (go.transform.CompareTag("Pot")
                                        && (go.GetComponent<CookingFSM>().isEmpty
                                        || go.GetComponent<CookingFSM>().isHalfFull)) {
                                        go.SendMessage("AddFood");
                                        go.transform.Find("soup").gameObject.SetActive(true);
                                        holdingObj.SendMessage("DeleteFoodHead");
                                        Destroy(holdingObj);
                                        isHolding = false;
                                        GetComponent<IKController>().isActive = false;
                                        holdingObj = null;
                                    }
                                    else if (go.transform.name == "plate"
                                        && !go.transform.Find("pieces").gameObject.activeSelf
                                        && !go.transform.Find("soup").gameObject.activeSelf) {
                                        holdingObj.SendMessage("DeleteFoodHead");
                                        Destroy(holdingObj);
                                        isHolding = false;
                                        GetComponent<IKController>().isActive = false;
                                        holdingObj = null;
                                        go.transform.Find("pieces").gameObject.SetActive(true);
                                    }
                                }
                            }
                        }
                        else {                //桌子是空的
                            if (isHolding) {
                                if (hitInfo.transform.name == "OvenTable") {  //炉子只能放锅
                                    if (holdingObj.CompareTag("Pot"))
                                        PutObj(hitInfo.transform.gameObject);
                                }
                                else if (hitInfo.transform.name == "CutTable1" ||
                                    hitInfo.transform.name == "CutTable2") {  //砧板只能放食物
                                    if (holdingObj.CompareTag("Food"))
                                        PutObj(hitInfo.transform.gameObject);
                                }
                                else PutObj(hitInfo.transform.gameObject);
                            }
                            else if (hitInfo.transform.name == "BoxTable")
                                GetObj("Tomato");
                        }
                    }
                    else if (hitInfo.transform.CompareTag("Sink")) {
                        if (isHolding) {
                            if (holdingObj.name == "DirtyPlate") {
                                int num = 1 + holdingObj.transform.childCount;
                                hitInfo.transform.gameObject.SendMessage("AddPlate", num);
                                hitInfo.transform.Find("Plate").gameObject.SetActive(true);
                                holdingObj.SetActive(false);
                                Transform c = holdingObj.transform.Find("DirtyPlate");
                                if (c != null) {
                                    c.parent = null;
                                    c.gameObject.SetActive(false);
                                }
                                holdingObj.transform.parent = null;
                                isHolding = false;
                                GetComponent<IKController>().isActive = false;
                                holdingObj = null;
                            }
                        }
                        else {
                            Vector3 offset = new Vector3(0, 0, -0.7f);
                            RaycastHit TableHitInfo;
                            if (Physics.Raycast(hitInfo.transform.position + offset, Vector3.up, out TableHitInfo, 0.6f)) {
                                PickUpObj(TableHitInfo.transform.gameObject);
                            }
                        }
                    }
                    else if (hitInfo.transform.CompareTag("Garbage")) {
                        if (isHolding) {
                            if (holdingObj.name == "Tomato") {
                                holdingObj.SendMessage("DeleteFoodHead");
                                Destroy(holdingObj);
                                holdingObj = null;
                                isHolding = false;
                                GetComponent<IKController>().isActive = false;
                            }
                            else if (holdingObj.transform.CompareTag("Pot")) {
                                holdingObj.SendMessage("PourOutSoup");
                                holdingObj.transform.Find("soup").gameObject.SetActive(false);
                            }
                            else if (holdingObj.transform.name == "plate") {
                                foreach (Transform tr in holdingObj.transform)
                                    tr.gameObject.SetActive(false);
                            }
                        }
                    }
                    else if (hitInfo.transform.name == "Exit") {
                        if (isHolding) {
                            if (holdingObj.name == "plate") {
                                gameManager.SendMessage("CompleteTask");
                                holdingObj.transform.parent = null;
                                holdingObj = null;
                                isHolding = false;
                                GetComponent<IKController>().isActive = false;
                            }
                        }
                    }
                    else if (hitInfo.transform.name == "PlateTable") {
                        if (!isHolding) {
                            RaycastHit TableHitInfo;
                            if (Physics.Raycast(hitInfo.transform.position, Vector3.up, out TableHitInfo, 0.6f)) {
                                PickUpObj(TableHitInfo.transform.gameObject);
                            }
                        }
                    }
                }
                
                else {  //drop on the ground
                    if (isHolding) {
                        DropObj();
                    }
                }
                this.SendMessage("ChangeHolding", isHolding);
            }
        }
    }
}
