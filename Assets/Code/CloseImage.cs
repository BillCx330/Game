using UnityEngine;
using UnityEngine.SceneManagement;

public class CloseImage : MonoBehaviour
{
    [Header("要跳转的场景名（必须和Build Settings里完全一致！）")]
    public string targetSceneName = "mainpanel 1";

    private SpriteRenderer _sr;

    void Awake()
    {
        // 1. 强制提升关闭按钮的层级（远高于拼图的2/3/4），彻底避免被遮挡
        _sr = GetComponent<SpriteRenderer>();
        if (_sr != null)
        {
            _sr.sortingOrder = 10; 
            _sr.sortingLayerName = "Default"; // 确保和拼图在同一Sorting Layer（或更高层级）
            Debug.Log($"[{gameObject.name}] 已强制提升层级至 {_sr.sortingOrder}", this);
        }
        else
        {
            Debug.LogWarning($"[{gameObject.name}] 未找到SpriteRenderer，层级提升失败，请手动设置！", this);
        }

        // 2. 自动添加2D碰撞体（如果没有的话），确保OnMouseDown能被触发
        Collider2D collider = GetComponent<Collider2D>();
        if (collider == null)
        {
            collider = gameObject.AddComponent<BoxCollider2D>();
            Debug.LogWarning($"[{gameObject.name}] 自动添加了BoxCollider2D，请在Inspector调整碰撞范围覆盖按钮！", this);
        }
        else
        {
            Debug.Log($"[{gameObject.name}] 已检测到Collider2D：{collider.GetType().Name}", this);
        }
    }

    // 点击图片时触发（依赖Collider2D）
    private void OnMouseDown()
    {
        Debug.Log($"✅ [{gameObject.name}] 关闭按钮被点击！开始执行跳转逻辑...", this);

        // 3. 安全清空旧单例，避免新场景Manager混乱
        if (PuzzleGameManager.Instance != null)
        {
            PuzzleGameManager.Instance = null;
            Debug.Log("已清空PuzzleGameManager单例", this);
        }

        // 4. 严格校验场景名，避免跳转失败
        if (string.IsNullOrEmpty(targetSceneName))
        {
            Debug.LogError($"❌ [{gameObject.name}] targetSceneName为空！请在Inspector配置正确的场景名", this);
            return;
        }

        // 5. 执行场景跳转
        SceneManager.LoadScene(targetSceneName);
        Debug.Log($"🚀 已成功跳转到场景：{targetSceneName}", this);
    }
}