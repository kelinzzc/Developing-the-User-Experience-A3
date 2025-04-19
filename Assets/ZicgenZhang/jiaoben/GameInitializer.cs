using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameInitializer : MonoBehaviour
{
    [Header("鼠标设置")]
    public bool showCursor = true;  // 是否显示鼠标

    void Start()
    {
        // 初始化游戏状态
        Time.timeScale = 1f;
        
        // 设置鼠标状态
        Cursor.lockState = showCursor ? CursorLockMode.None : CursorLockMode.Locked;
        Cursor.visible = showCursor;

        
    }
}