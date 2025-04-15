using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectible : MonoBehaviour
{
    [Header("音效设置")]
    public AudioClip collectSound;
    
    // 移除Range限制，允许更大值输入
    [Tooltip("建议值：0.1-10，超过5可能产生削波失真")]
    public float volumeScale = 2f; 

    void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            // 添加安全限制（可选）
            float safeVolume = Mathf.Clamp(volumeScale, 0f, 10f);
            
            // 使用3D空间音效模式增强表现力
            GameObject tempGO = new GameObject("TempAudio");
            tempGO.transform.position = transform.position;
            
            AudioSource audioSource = tempGO.AddComponent<AudioSource>();
            audioSource.spatialBlend = 0.3f; // 保留部分3D效果
            audioSource.PlayOneShot(collectSound, safeVolume);
            
            Destroy(tempGO, collectSound.length + 0.1f);
            
            Destroy(gameObject);
        }
    }
}