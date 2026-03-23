using UnityEngine;
using System.Collections.Generic;

public class PuzzleGameManager : MonoBehaviour
{
    // 单例模式：方便其他脚本快速调用
    public static PuzzleGameManager Instance;

    [Header("游戏配置")]
    public List<PuzzlePiece> allPuzzlePieces; // 所有拼图块（手动拖入或自动获取）
    public bool autoFindPieces = true;        // 是否自动查找所有拼图块

    [Header("通关回调（可选）")]
    public GameObject winUI;                  // 通关UI
    public AudioClip winSound;                // 通关音效

    // 已拼对的拼图数量
    private int correctPieceCount;

    void Awake()
    {
        // 单例初始化（确保全局唯一）
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // 跨场景保留（可选）
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        // 自动查找所有拼图块（无需手动拖入）
        if (autoFindPieces)
        {
            allPuzzlePieces = new List<PuzzlePiece>(FindObjectsOfType<PuzzlePiece>());
        }

        // 初始化已拼对数量
        correctPieceCount = 0;
        winUI?.SetActive(false); // 隐藏通关UI
    }

    /// <summary>
    /// 拼图块拼对时的回调
    /// </summary>
    /// <param name="piece">拼对的拼图块</param>
    public void OnPieceCorrect(PuzzlePiece piece)
    {
        correctPieceCount++;
        Debug.Log($"拼图 [{piece.gameObject.name}] 拼对！已完成 {correctPieceCount}/{allPuzzlePieces.Count}");

        // 检查是否全部拼对
        CheckWinCondition();
    }

    /// <summary>
    /// 检查通关条件
    /// </summary>
    private void CheckWinCondition()
    {
        if (correctPieceCount >= allPuzzlePieces.Count)
        {
            Debug.Log("恭喜！所有拼图已完成！");
            OnGameWin();
        }
    }

    /// <summary>
    /// 通关逻辑（可扩展）
    /// </summary>
    private void OnGameWin()
    {
        // 显示通关UI
        winUI?.SetActive(true);

        // 播放通关音效
        if (winSound != null)
        {
            AudioSource.PlayClipAtPoint(winSound, Camera.main.transform.position);
        }

        // 可选：禁用鼠标控制（防止通关后继续操作）
        Mousecontrol mouseControl = FindObjectOfType<Mousecontrol>();
        if (mouseControl != null)
        {
            mouseControl.enabled = false;
        }
    }

    /// <summary>
    /// 重置游戏（可选：重新开始用）
    /// </summary>
    public void ResetGame()
    {
        correctPieceCount = 0;
        winUI?.SetActive(false);

        // 重置所有拼图块状态
        foreach (PuzzlePiece piece in allPuzzlePieces)
        {
            piece.isCorrect = false;
            // 可选：将拼图块随机位置摆放（需自己扩展）
        }

        // 重新启用鼠标控制
        Mousecontrol mouseControl = FindObjectOfType<Mousecontrol>();
        if (mouseControl != null)
        {
            mouseControl.enabled = true;
        }
    }
}