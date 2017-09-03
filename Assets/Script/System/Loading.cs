using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

public class Loading : MonoBehaviour {

    public Slider processBar;
    private AsyncOperation async;
    private int _nowprocess;

    void Start() {
        _nowprocess = 0;
        StartCoroutine(loadScene());
    }

    IEnumerator loadScene() {
        async = SceneManager.LoadSceneAsync(Globe.loadName);
        async.allowSceneActivation = false;
        yield return async;

    }
	
	// Update is called once per frame
	void Update () {
        if (async == null) {
            return;
        }

        int toProcess;
        if (async.progress < 0.9f)
        {
            toProcess = (int)(async.progress * 100);
        }
        else {
            toProcess = 100;
        }

        if (_nowprocess < toProcess) {  //即使瞬间加载完成也进行读条
            _nowprocess++;
        }

        processBar.value = _nowprocess / 100f;

        if (_nowprocess == 100)
        {
            async.allowSceneActivation = true;
        }
	}
}
