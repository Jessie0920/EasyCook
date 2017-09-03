using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class audio : MonoBehaviour {
    public float musicVolume;
    public AudioSource gamePlayMusic;
    public AudioSource gameOverMusic;
    private bool isGameOver = false;

    // Use this for initialization
    void Start() {
        musicVolume = 0.2F;
    }

    void GameOver() {
        isGameOver = true;
    }

    // Update is called once per frame
    void Update() {
        if (!gamePlayMusic.isPlaying) {
            //播放音乐
            gamePlayMusic.Play();
        }
        if (isGameOver) {
            gamePlayMusic.Stop();
            gameOverMusic.Play();
        }
        
    }
}