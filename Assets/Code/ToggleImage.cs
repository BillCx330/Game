using UnityEngine;

public class ToggleWinImage : MonoBehaviour
{
    // 当鼠标点击该对象时触发
    private void OnMouseDown()
    {
        // 切换当前对象的激活状态（显示 ↔ 隐藏）
        gameObject.SetActive(!gameObject.activeSelf);
    }
}