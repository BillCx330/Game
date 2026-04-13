using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class MusicScrip : MonoBehaviour
{
    public Button musicBtn;

    // 背景音乐播放器（自己在场景建一个空物体，挂 AudioSource 并放背景音乐）
    public AudioSource bgMusic;

    // 记录点击次数
    private int clickCount = 0;

    void Start()
    {
        // 绑定按钮点击事件
        musicBtn.onClick.AddListener(click);
    }

    public void click()
    {
        while (true)
        {

            clickCount++;
            // 奇数：关闭
            if (clickCount % 2 == 1)
            {
                bgMusic.Stop();
            }
            // 偶数：打开
            else
            {
                bgMusic.Play();
            }
        }
        
    }

}
