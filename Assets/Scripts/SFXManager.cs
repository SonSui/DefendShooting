using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using Unity.VisualScripting;
using UnityEngine.SocialPlatforms.Impl;

[System.Serializable]
public struct soundAudio
{
    public string name;
    public AudioClip audioClip;
    public float volume;
}
public class SFXManager : MonoBehaviour
{
    public static SFXManager instance;
    public List<soundAudio> soundList = new List<soundAudio>();
    public AudioSource audioSource;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private void Start()
    {
        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
        }
    }
    public void PlayButtonSound()
    {
        soundAudio au = GetClipByName("Button");
        audioSource.clip = au.audioClip;
        audioSource.volume = au.volume;
        audioSource.Play();
    }
    public void PlayScoreSound()
    {
        soundAudio au = GetClipByName("Score");
        audioSource.clip = au.audioClip;
        audioSource.volume = au.volume;
        audioSource.Play();
    }
    public void PlayShotSound()
    {
        soundAudio au = GetClipByName("Shot");
        audioSource.clip = au.audioClip;
        audioSource.volume = au.volume;
        audioSource.Play();
    }
    public soundAudio GetClipByName(string name)
    {
        foreach (var s in soundList)
        {
            if (s.name == name)
            {
                return s;
            }
        }

        soundAudio nu = new soundAudio();
        return nu;
    }

}
