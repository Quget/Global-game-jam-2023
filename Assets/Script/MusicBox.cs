using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicBox : MonoBehaviour
{
    [SerializeField]
    private AudioSource musicSource = null;

    [SerializeField]
    private float defaultVolume = 0.1f;

    public void PlayAudio(AudioClip audioClip)
    {
        StartCoroutine(CrossFade(audioClip));
    }

    private IEnumerator CrossFade(AudioClip newClip)
    {
        if(musicSource.clip == null)
        {
            musicSource.volume = defaultVolume;
            musicSource.clip = newClip;
            musicSource.Play();
        }
        else
        {
            yield return FadeOut();
            musicSource.clip = newClip;
            yield return FadeIn();
        }

    }

    private IEnumerator FadeIn()
    {
        float targetVolume = defaultVolume;

        while (musicSource.volume < targetVolume)
        {
           musicSource.volume += defaultVolume * Time.deltaTime;
           yield return new WaitForEndOfFrame();
        }
        musicSource.volume = defaultVolume;
    }

    private IEnumerator FadeOut()
    {
        float targetVolume = 0;

        while (musicSource.volume > targetVolume)
        {
            musicSource.volume -= defaultVolume * Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        musicSource.volume = 0;
    }

    public void StopAudio()
    {
        musicSource.Stop();
    }

}
