using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class juese : MonoBehaviour
{
    private CharacterController _controller;
    private GameObject _mainCamera;
    private Animator _animator;

    // 移动参数
    public float moveSpeed = 5f;             // 基础移动速度
    public float RotationSmoothTime = 0.1f;
    public float jumpForce = 6f;
    
    // 新增变量声明
    private Vector2 _move;
    private float _targetRot;
    private float _rotationVelocity;
    private float _verticalVelocity;
    private Vector3 _moveDirection;         // 移动方向
    private float _currentSpeed;             // 当前实际速度

    void Start()
    {
        if (_mainCamera == null)
            _mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
        
        _controller = GetComponent<CharacterController>();
        _animator = GetComponent<Animator>();
    }

    void Update()
    {
        HandleGravity();
        HandleJump();
        HandleMovement();
        UpdateAnimation();
    }

    // 处理重力
    void HandleGravity()
    {
        if (!_controller.isGrounded)
            _verticalVelocity -= 9.81f * Time.deltaTime;
        else
            _verticalVelocity = -0.5f; // 轻微向下力确保接地
    }

    // 处理跳跃
    void HandleJump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && _controller.isGrounded)
            _verticalVelocity = jumpForce;
    }

    // 处理移动
    void HandleMovement()
    {
        if (_move != Vector2.zero)
        {
            // 计算目标旋转角度
            Vector3 inputDir = new Vector3(_move.x, 0.0f, _move.y).normalized;
            _targetRot = Mathf.Atan2(inputDir.x, inputDir.z) * Mathf.Rad2Deg + _mainCamera.transform.eulerAngles.y;
            float rotation = Mathf.SmoothDampAngle(transform.eulerAngles.y, _targetRot, ref _rotationVelocity, RotationSmoothTime);
            transform.rotation = Quaternion.Euler(0.0f, rotation, 0.0f);

            // 计算移动方向
            _moveDirection = Quaternion.Euler(0.0f, _targetRot, 0.0f) * Vector3.forward;
            _currentSpeed = moveSpeed; // 使用基础速度（可扩展冲刺逻辑）
        }
        else
        {
            _currentSpeed = 0f; // 无输入时速度为0
        }

        // 综合移动
        Vector3 motion = _moveDirection * _currentSpeed + Vector3.up * _verticalVelocity;
        _controller.Move(motion * Time.deltaTime);
    }

    // 更新动画参数
    void UpdateAnimation()
    {
        float axisY = _animator.GetFloat("AxisY");
        axisY = Mathf.MoveTowards(axisY, _move.magnitude, 5f * Time.deltaTime);
        _animator.SetFloat("AxisY", axisY);
    }

    // 输入回调
    void OnMove(InputValue value)
    {
        _move = value.Get<Vector2>();
    }
}