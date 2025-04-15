using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CollectibleManager : MonoBehaviour
{
    public static CollectibleManager Instance;
    
    [Header("UI Settings")]
    public GameObject victoryPanel;
    public Text counterText;
    
    private int totalCollectibles;
    private int collectedCount;

    void Awake()
{
    Instance = this;
    totalCollectibles = GameObject.FindGameObjectsWithTag("Collectible").Length;
    
    // 自动查找Text组件（如果手动拖拽失败）
    if (counterText == null)
        counterText = GameObject.Find("RemainingText").GetComponent<Text>();
    
    UpdateCounter();
    victoryPanel.SetActive(false);
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
        victoryPanel.SetActive(true);
        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.None;
    }
}