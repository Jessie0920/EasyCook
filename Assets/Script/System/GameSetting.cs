using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class GameSetting : MonoBehaviour {

    public GameObject gm;

    public void ContinueGame() {
        gm.GetComponent<GameManager>().GameResume();
    }


    public void ExitGame() {
        SceneManager.LoadScene("GameStart");		//加载选关场景
    }

}
