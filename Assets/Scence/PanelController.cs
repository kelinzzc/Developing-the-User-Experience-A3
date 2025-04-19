using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelController : MonoBehaviour
{
    [Header("UI控制")]
    public GameObject targetPanel; // 拖入需要控制的Panel

    // 切换面板显示状态
    public void TogglePanel()
    {
        if (targetPanel != null)
        {
            bool isActive = targetPanel.activeSelf;
            targetPanel.SetActive(!isActive);
        }
    }
}