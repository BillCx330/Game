using UnityEngine;
using UnityEngine.SceneManagement; // 必须引入场景管理的命名空间

public class CloseImage : MonoBehaviour
{
    // 点击图片时触发
    private void OnMouseDown()
    {
        // 跳转到 mainpanel 1 场景
        SceneManager.LoadScene("mainpanel 1");
    }
}