using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGMController : MonoBehaviour
{
    public AudioClip musicClipOne;

    public AudioClip musicClipTwo;

    public AudioSource musicSource;

    int score = 0;
    int gameOver = 0;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(score == 4)
        {
            musicSource.clip = musicClipOne;
            musicSource.Play();
        }
        if(gameOver == 1)
        {
            musicSource.clip = musicClipTwo;
            musicSource.Play();
        }
    }
    public void ChangeBGM(int scoreAmount)
    {
        score += 1;
    }

    public void ChangeBGM2(int lose)
    {
        gameOver += 1;
    }
}
