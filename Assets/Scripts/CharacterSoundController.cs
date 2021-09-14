using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSoundController : MonoBehaviour
{

    [Header("Sound")]
    public AudioClip jump;
    public AudioClip scoreHighlight;
    public AudioClip bgm;

    private AudioSource audioPlayer;

    // Start is called before the first frame update
    void Start()
    {
        audioPlayer = GetComponent<AudioSource>();
        audioPlayer.clip = bgm;
        audioPlayer.loop = true;
        audioPlayer.Play();
    }

    // Update is called once per frame
    public void PlayJump()
    {
        audioPlayer.PlayOneShot(jump);
    }

    public void PlayScoreHighlight()
    {
        audioPlayer.PlayOneShot(scoreHighlight);
    }
}
