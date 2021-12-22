using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using UnityEngine.SceneManagement;
using TMPro;
[System.Serializable] public struct StartButton
{
    public Button button;
    public string musicName;
    
    public void Init()
    {
       //button.GetComponent<TextMeshPro>().text = musicName;
    }
}
public class GameStartScene : MonoBehaviour
{
    [SerializeField] private List<StartButton> buttonList = new List<StartButton>();
    private void Awake()
    {
        foreach(var button in buttonList)
        {
            button.Init();
            button.button.OnClickAsObservable().Subscribe(_=> {
                SoundManager.instance.SelectMusic(button.musicName);
                SceneManager.LoadScene("CircleMove");
                }).AddTo(this);
        }
    }
}
