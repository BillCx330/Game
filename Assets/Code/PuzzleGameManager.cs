using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class PuzzleGameManager : MonoBehaviour
{
    // 单例模式：方便其他脚本快速调用
    public static PuzzleGameManager Instance;

    [Header("游戏配置")]
    public List<PuzzlePiece> allPuzzlePieces; // 所有拼图块（手动拖入或自动获取）
    public bool autoFindPieces = true;        // 是否自动查找所有拼图块，保持勾选即可
    [Header("通关回调（可选）")]
    public GameObject winUI;                  // 通关UI（必须拖入当前场景里的Win对象）
    public AudioClip winSound;                // 通关音效，没有就空着

    // 已拼对的拼图数量
    private int correctPieceCount;

    void Awake()
    {
        // 单例逻辑，去掉跨场景保留，彻底解决引用丢失
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    // 场景加载时自动初始化，解决跳关卡不重置的问题
    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    // 场景加载完成后自动执行初始化
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        InitGame();
    }

    void Start()
    {
        // 游戏启动时第一次初始化
        InitGame();
    }

    // 统一初始化方法，每次进关卡都会执行，彻底解决计数错乱
    public void InitGame()
    {
        // 1. 自动查找当前场景的所有拼图块，不用手动拖
        if (autoFindPieces)
        {
            allPuzzlePieces = new List<PuzzlePiece>(FindObjectsOfType<PuzzlePiece>());
            Debug.Log($"当前关卡找到 {allPuzzlePieces.Count} 个拼图块");
        }

        // 2. 重置通关计数为0
        correctPieceCount = 0;

        // 3. 强制隐藏通关UI，确保进关卡时界面是关闭的
        if (winUI != null)
        {
            winUI.SetActive(false);
            Debug.Log("通关UI已初始化隐藏");
        }
        else
        {
            Debug.LogError("【严重错误】请在Inspector里给PuzzleGameManager拖入winUI对象！");
        }

        // 4. 重置所有拼图拖拽脚本的启用状态
        PuzzleDrag[] allDragScripts = FindObjectsOfType<PuzzleDrag>();
        foreach (PuzzleDrag drag in allDragScripts)
        {
            if (drag != null)
            {
                drag.enabled = true;
            }
        }

        // 5. 重置所有拼图块的状态，完整空值判断，杜绝空引用报错
        if (allPuzzlePieces != null)
        {
            foreach (PuzzlePiece piece in allPuzzlePieces)
            {
                if (piece != null)
                {
                    piece.isCorrect = false;
                    piece.SetSortingOrder(3);
                }
            }
        }
    }

    /// <summary>
    /// 拼图块拼对时自动调用
    /// </summary>
    public void OnPieceCorrect(PuzzlePiece piece)
    {
        correctPieceCount++;
        Debug.Log($"拼图 [{piece.gameObject.name}] 拼对！已完成 {correctPieceCount}/{allPuzzlePieces.Count}");
        CheckWinCondition();
    }

    /// <summary>
    /// 检查是否通关
    /// </summary>
    private void CheckWinCondition()
    {
        // 空值判断，避免无拼图时直接通关
        if (allPuzzlePieces == null || allPuzzlePieces.Count == 0)
        {
            Debug.LogWarning("当前场景没有找到拼图块，请检查拼图是否挂载了PuzzlePiece脚本");
            return;
        }

        if (correctPieceCount >= allPuzzlePieces.Count)
        {
            Debug.Log("恭喜！所有拼图已完成！");
            OnGameWin();
        }
    }

    /// <summary>
    /// 通关执行的逻辑
    /// </summary>
    private void OnGameWin()
    {
        // 显示通关UI，加非空判断
        if (winUI != null)
        {
            winUI.SetActive(true);
            Debug.Log("通关UI已显示");
        }
        else
        {
            Debug.LogError("通关UI显示失败！winUI变量为空，请检查PuzzleGameManager的Inspector设置");
        }

        // 播放通关音效
        if (winSound != null)
        {
            AudioSource.PlayClipAtPoint(winSound, Camera.main.transform.position);
        }

        // 通关后禁用所有拖拽操作，防止误触
        PuzzleDrag[] allDragScripts = FindObjectsOfType<PuzzleDrag>();
        foreach (PuzzleDrag drag in allDragScripts)
        {
            if (drag != null)
            {
                drag.enabled = false;
            }
        }
    }

    /// <summary>
    /// 重置当前关卡，可绑定到重玩按钮
    /// </summary>
    public void ResetGame()
    {
        InitGame();
    }
}