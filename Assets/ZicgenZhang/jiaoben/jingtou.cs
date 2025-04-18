using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class jingtou : MonoBehaviour
{
    private GameObject _mainCamera;
    
    [Header("Cinemachine")]
    [Tooltip("Follow the target")]
    public GameObject CameraTarget;
    
    [Header("Rotation limitation")]
    [Tooltip("Maximum upward tilt Angle")]
    public float TopClamp = 70.0f;
    [Tooltip("The maximum Angle of descent")]
    public float BottomClamp = -30.0f;
    
    [Header("Control parameters")]
    [Tooltip("Mouse sensitivity")]
    public float lookSensitivity = 0.5f;

    private const float _threshold = 0.01f;
    private float _cinemachineTargetYaw;
    private float _cinemachineTargetPitch;
    private Vector2 _look;

    void Start()
    {
        if (_mainCamera == null)
            _mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
        
        // 初始化目标角度
        _cinemachineTargetYaw = CameraTarget.transform.rotation.eulerAngles.y;
        _cinemachineTargetPitch = CameraTarget.transform.rotation.eulerAngles.x;
    }

    private void Update()
    {
        // 处理输入
        if (_look.sqrMagnitude >= _threshold)
        {
            _cinemachineTargetYaw += _look.x * lookSensitivity;
            _cinemachineTargetPitch -= _look.y * lookSensitivity; // 关键修改：反向Y轴
        }

        // 限制角度范围
        _cinemachineTargetYaw = ClampAngle(_cinemachineTargetYaw, float.MinValue, float.MaxValue);
        _cinemachineTargetPitch = ClampAngle(_cinemachineTargetPitch, BottomClamp, TopClamp);
        
        // 应用旋转
        CameraTarget.transform.rotation = Quaternion.Euler(
            _cinemachineTargetPitch,
            _cinemachineTargetYaw,
            0.0f
        );
    }

    private static float ClampAngle(float angle, float min, float max)
    {
        if (angle < -360f) angle += 360f;
        if (angle > 360f) angle -= 360f;
        return Mathf.Clamp(angle, min, max);
    }

    public void OnLook(InputValue value)
    {
        _look = value.Get<Vector2>();
    }

}