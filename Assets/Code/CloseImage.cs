using UnityEngine;
using UnityEngine.SceneManagement;

public class CloseImage : MonoBehaviour
{
    [Header("要跳转的场景名（必须和Build Settings里完全一致！）")]
    public string targetSceneName = "mainpanel 1";

    // 点击图片时触发
    private void OnMouseDown()
    {
        // 跳转前清空旧单例，避免新场景Manager混乱
        PuzzleGameManager.Instance = null;
        // 跳转到目标关卡
        SceneManager.LoadScene(targetSceneName);
        Debug.Log($"已跳转到场景：{targetSceneName}");
    }
}