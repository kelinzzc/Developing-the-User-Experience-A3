using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CollectibleManager : MonoBehaviour  
{
    public static CollectibleManager Instance;

    [Header("Scene Settings")]
    public string restartSceneName = "GameScene";
    public float restartDelay = 3f;

    [Header("Camera Settings")]
    public Camera playerCamera;
    public Camera victoryCamera;

    [Header("UI Settings")]
    public GameObject victoryPanel;
    public Text counterText;

    private int totalCollectibles;
    private int collectedCount;

    void Awake()
    {
        Instance = this;
        totalCollectibles = GameObject.FindGameObjectsWithTag("Collectible").Length;
        victoryCamera.gameObject.SetActive(false);
        UpdateCounter();
        
        // 注册场景加载完成事件
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDestroy()
    {
        // 注销事件防止内存泄漏
        SceneManager.sceneLoaded -= OnSceneLoaded;
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
        counterText.text = $"surplus: {totalCollectibles - collectedCount}/{totalCollectibles}";
    }

    void ShowVictory()
    {
        playerCamera.gameObject.SetActive(false);
        victoryCamera.gameObject.SetActive(true);
        victoryPanel.SetActive(true);
        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.None;
        
        StartCoroutine(RestartGame());
    }

    IEnumerator RestartGame()
    {
        yield return new WaitForSecondsRealtime(restartDelay);
        Time.timeScale = 1f;
        SceneManager.LoadScene(restartSceneName);
    }

    // 新场景加载完成时的回调
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if(scene.name == restartSceneName)
        {
            // 自动开始新游戏
            StartNewGame();
        }
    }

    void StartNewGame()
    {
        // 重置游戏状态
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.Locked;
        
        // 在这里添加其他初始化逻辑，例如：
        // - 重置玩家位置
        // - 重新生成收集物品
        // - 重置UI状态
        // - 启用玩家控制
    }
}