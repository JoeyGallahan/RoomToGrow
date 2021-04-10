using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class AudioController
{
    [SerializeField] private AudioSource source;
    [SerializeField] private Slider volumeSlider;

    [SerializeField] private List<AudioClip> digSFX;
    [SerializeField] private List<AudioClip> pruneSFX;
    [SerializeField] private AudioClip winSFX;
    [SerializeField] private AudioClip loseSFX;
    [SerializeField] private List<AudioClip> music;

    [SerializeField] private int curMusicIndex = -1;

    public enum SFXType
    {
        PLANT,
        PRUNE,
        WIN,
        LOSE
    }

    public void PlayMusic()
    {
        source.volume = volumeSlider.value;
        if (!source.isPlaying)
        {
            NextSong();
        }
    }
    
    //Randomly Selects a new song
    private void NextSong()
    {
        int rand;
        int dont = 0;//for infinite loops yadda yadda yadda

        do
        {
            rand = Random.Range(0, music.Count);

            dont++;
        }
        while (rand == curMusicIndex && dont < 25);

        curMusicIndex = rand;

        source.clip = music[curMusicIndex];
        source.Play();
    }

    //Play a single sound effect once
    public void PlaySound(SFXType type)
    {
        int rand;
        switch (type)
        {
            case SFXType.PLANT: rand = Random.Range(0, digSFX.Count); //If there is more than one type of a single sound effect, get a random one
                source.PlayOneShot(digSFX[rand]);
                break;
            case SFXType.PRUNE: rand = Random.Range(0, pruneSFX.Count); //If there is more than one type of a single sound effect, get a random one
                source.PlayOneShot(pruneSFX[rand]);
                break;
            case SFXType.WIN:
                source.PlayOneShot(winSFX);
                break;
            case SFXType.LOSE:
                source.PlayOneShot(loseSFX);
                break;
        }
    }
}
