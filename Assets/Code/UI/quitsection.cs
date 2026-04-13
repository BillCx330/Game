using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class quitsection : MonoBehaviour
{
    public void Quit()
    {
        // 编辑器下退出播放模式
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
            // 打包后关闭程序
            Application.Quit();
#endif
    }
}
