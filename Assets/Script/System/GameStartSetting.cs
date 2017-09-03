using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Globe {
    public static string loadName;
    public static bool isLoadedFirtime = true;
}

public class GameStartSetting : MonoBehaviour {

    public GameObject SelectLevelUI;

    void Start() {
        if (!Globe.isLoadedFirtime)
            SelectLevelUI.SetActive(true);
        Debug.Log("what");
    }

    public void SelectLevel() {
        SelectLevelUI.SetActive(true);
    }

    public void StartGame() {
        Globe.loadName = "level-1";
        Globe.isLoadedFirtime = false;
        SceneManager.LoadScene("LoadingScene");		//加载Loading场景
    }

    public void ReturnToStart() {
        SelectLevelUI.SetActive(false);
    }

    public void ExitGame() {
        Application.Quit();	//退出游戏
    }

}
