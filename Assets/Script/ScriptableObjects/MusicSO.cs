using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName ="MusicSO", menuName ="Scriptable Object/MusicSO",order =int.MaxValue)]
public class MusicSO : ScriptableObject 
{
    public string musicName;
    public int bpm;
    public AudioClip music;
}
