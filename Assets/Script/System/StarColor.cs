using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class StarColor : MonoBehaviour {
    public GameObject star1,star2,star3;

    public int threeStars;
    public int twoStars;
    public int oneStar;

	// Use this for initialization
	void Start () {
        int score = PlayerPrefs.GetInt("level1_best");
        Debug.Log(score);
        if (score >= threeStars) {
            star1.GetComponent<Image>().color = Color.white;
            star2.GetComponent<Image>().color = Color.white;
            star3.GetComponent<Image>().color = Color.white;
        }
        else if (score >= twoStars) {
            star1.GetComponent<Image>().color = Color.white;
            star2.GetComponent<Image>().color = Color.white;
            star3.GetComponent<Image>().color = Color.black;
        }
        else if (score >= oneStar) {
            star1.GetComponent<Image>().color = Color.white;
            star2.GetComponent<Image>().color = Color.black;
            star3.GetComponent<Image>().color = Color.black;
        }
        else {
            star1.GetComponent<Image>().color = Color.black;
            star2.GetComponent<Image>().color = Color.black;
            star3.GetComponent<Image>().color = Color.black;
        }
	}
}
