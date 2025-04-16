using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// 文件名必须为 Game.cs，类名必须为 Game
public class CollectibleManager : MonoBehaviour  // 类名从 GameManager 改为 Game
{
    public static CollectibleManager Instance;    // 同步修改静态实例类型

    [Header("摄像机设置")]
    public Camera playerCamera;    // 拖入主摄像机
    public Camera victoryCamera;   // 拖入胜利摄像机

    [Header("UI设置")]
    public GameObject victoryPanel;
    public Text counterText;

    private int totalCollectibles;
    private int collectedCount;

    void Awake()
    {
        Instance = this;
        totalCollectibles = GameObject.FindGameObjectsWithTag("Collectible").Length;
        victoryCamera.gameObject.SetActive(false); // 初始禁用胜利摄像机
        UpdateCounter();
    }

    public void CollectItem()
    {
        collectedCount++;
        UpdateCounter();

        if(collectedCount >= totalCollectibles)
            ShowVictory();
    }

    void UpdateCounter()
    {
        counterText.text = $"剩余物品: {totalCollectibles - collectedCount}/{totalCollectibles}";
    }

    void ShowVictory()
    {
        // 切换摄像机
        playerCamera.gameObject.SetActive(false);
        victoryCamera.gameObject.SetActive(true);

        // 显示UI
        victoryPanel.SetActive(true);
        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.None;
    }
}