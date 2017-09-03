using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Returnplate : MonoBehaviour {

    private float timeIntv = 5.0f;
    private float timer = 0;

    private GameObject[] dirtyplates;
    public GameObject dirtyplatePrefab;
    private Vector3 pos = new Vector3(19.2f, 1.25f, 19.8f);

	// Use this for initialization
	void Start () {
        dirtyplates = new GameObject[2];
        for (int i = 0; i < 2; ++i) {
            dirtyplates[i] = Instantiate(dirtyplatePrefab,
                pos, Quaternion.identity) as GameObject;
            dirtyplates[i].name = "DirtyPlate";
            dirtyplates[i].SetActive(false);
        }
	}

    IEnumerator Countdown(int t)  
    {
        yield return new WaitForSeconds(t);
        RaycastHit TableHitInfo;
        if (Physics.Raycast(this.transform.position, Vector3.up, out TableHitInfo, 1.0f)) {
            for (int i = 0; i < 2; ++i) {
                if (!dirtyplates[i].activeSelf) {
                    dirtyplates[i].transform.SetParent(TableHitInfo.transform);
                    dirtyplates[i].transform.GetComponent<Collider>().enabled = false;
                    dirtyplates[i].transform.GetComponent<Rigidbody>().isKinematic = true;
                    dirtyplates[i].transform.localPosition = new Vector3(0, 0.1f, 0);
                    dirtyplates[i].SetActive(true);
                    break;
                }
            }
        }
        else {
            for (int i = 0; i < 2; ++i) {
                if (!dirtyplates[i].activeSelf) {
                    dirtyplates[i].transform.position = new Vector3(19f, 1.025f, 19f);
                    dirtyplates[i].transform.GetComponent<Collider>().enabled = true;
                    dirtyplates[i].transform.GetComponent<Rigidbody>().isKinematic = false;
                    dirtyplates[i].SetActive(true);
                    break;
                }
            }
        }
        StopCoroutine("Countdown");
    }  

    void ReturnPlate() {
        StartCoroutine(Countdown(5));
    }
}
