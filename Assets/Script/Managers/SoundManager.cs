using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum MusicType
{
    Music,
    SFX,
    BGM
}

public class SoundManager : MonoBehaviour
{
    public int BPM { get {return curMusic.bpm;} }
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource sfxSource;
    [SerializeField] private AudioSource bgmSource;
    [SerializeField] private MusicSO curMusic;

    [Header("BGM")]
    [SerializeField] private List<AudioClip> bgmClips;
    [Header("SFX")]
    [SerializeField] private List<AudioClip> sfxClips;

    private static SoundManager Instance=null;
    public static SoundManager instance { get { return Instance; } }

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            if (Instance != this)
                DestroyImmediate(this);
        }

        if(musicSource==null)
        {
            var instance = new GameObject();
            instance.transform.SetParent(this.transform);
            musicSource=instance.AddComponent<AudioSource>();
        }

        if(sfxSource==null)
        {
            var instance = new GameObject();
            instance.transform.SetParent(this.transform);
            sfxSource =instance.AddComponent<AudioSource>();
        }
        if(
            bgmSource==null)
        {
            var instance = new GameObject();
            instance.transform.SetParent(this.transform);
            bgmSource =instance.AddComponent<AudioSource>();
        }
    }

    public void PlayMusic()
    {
        musicSource.loop = true;
        musicSource.Play();
        Debug.Log("¿Ωæ« Ω√¿€");
    }

    public void PlayBgm(int num)
    {
        bgmSource.clip = bgmClips[num];
        bgmSource.loop = true;
        bgmSource.Play();
    }

    public void PlaySFX(int num)
    {
        sfxSource.PlayOneShot(sfxClips[num]);
    }

    public void SelectMusic(string name)
    {
        var selectedMusic = Resources.Load(name) as MusicSO;
        if (selectedMusic == null)
            return;
        curMusic = selectedMusic;
        musicSource.clip = curMusic.music;
        Debug.Log(musicSource.clip.name);
    }
}
