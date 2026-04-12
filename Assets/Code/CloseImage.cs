using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems; // 新增：UGUI事件系统

// 实现IPointerClickHandler，替代OnMouseDown
public class CloseImage : MonoBehaviour, IPointerClickHandler
{
    [Header("要跳转的场景名（必须和Build Settings里完全一致！）")]
    public string targetSceneName = "mainpanel 1";

    private SpriteRenderer _sr;
    private Camera _mainCamera; // 新增：缓存主相机

    // 延迟到Start初始化（相机已加载完成）
    void Start()
    {
        // 1. 获取主相机（确保场景切换后相机正确）
        _mainCamera = Camera.main;
        if (_mainCamera == null)
        {
            Debug.LogError($"[{gameObject.name}] 未找到主相机！", this);
            return;
        }

        // 2. 强制提升关闭按钮的层级
        _sr = GetComponent<SpriteRenderer>();
        if (_sr != null)
        {
            _sr.sortingOrder = 10; 
            _sr.sortingLayerName = "Default";
            Debug.Log($"[{gameObject.name}] 已强制提升层级至 {_sr.sortingOrder}", this);
        }
        else
        {
            Debug.LogWarning($"[{gameObject.name}] 未找到SpriteRenderer，层级提升失败！", this);
        }

        // 3. 自动添加/修复2D碰撞体
        Collider2D collider = GetComponent<Collider2D>();
        if (collider == null)
        {
            collider = gameObject.AddComponent<BoxCollider2D>();
            Debug.LogWarning($"[{gameObject.name}] 自动添加BoxCollider2D，请调整碰撞范围！", this);
        }
        // 关键：确保碰撞体不是Trigger（OnMouseDown/IPointerClick都需要非Trigger）
        collider.isTrigger = false; 
        Debug.Log($"[{gameObject.name}] 碰撞体设置完成：{collider.GetType().Name} | Trigger={collider.isTrigger}", this);

        // 4. 给主相机添加Physics2DRaycaster（跨场景后可能丢失）
        if (_mainCamera.GetComponent<Physics2DRaycaster>() == null)
        {
            _mainCamera.gameObject.AddComponent<Physics2DRaycaster>();
            Debug.Log($"[{_mainCamera.name}] 自动添加Physics2DRaycaster组件", this);
        }

        // 5. 确保场景有EventSystem（UGUI事件系统必需）
        if (FindObjectOfType<EventSystem>() == null)
        {
            GameObject eventSystem = new GameObject("EventSystem");
            eventSystem.AddComponent<EventSystem>();
            eventSystem.AddComponent<StandaloneInputModule>();
            Debug.Log("自动创建EventSystem（UGUI事件系统核心）", this);
        }
    }

    // 替代OnMouseDown：UGUI点击事件（更稳定）
    public void OnPointerClick(PointerEventData eventData)
    {
        // 只响应左键点击
        if (eventData.button != PointerEventData.InputButton.Left) return;

        Debug.Log($"✅ [{gameObject.name}] 关闭按钮被点击！开始跳转...", this);

        // 安全清空单例
        if (PuzzleGameManager.Instance != null)
        {
            PuzzleGameManager.Instance = null;
            Debug.Log("已清空PuzzleGameManager单例", this);
        }

        // 校验场景名
        if (string.IsNullOrEmpty(targetSceneName))
        {
            Debug.LogError($"❌ [{gameObject.name}] targetSceneName为空！", this);
            return;
        }

        // 执行场景跳转
        SceneManager.LoadScene(targetSceneName);
        Debug.Log($"🚀 跳转到场景：{targetSceneName}", this);
    }
}