using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

[RequireComponent(typeof(CharacterController))]
public class juese : MonoBehaviour
{
    [Header("移动设置")]
    public float moveSpeed = 5f;
    public float RotationSmoothTime = 0.1f;
    public float jumpForce = 6f;

    [Header("动画设置")]
    public AnimationClip jumpAnimation;
    [Range(0.01f, 0.3f)]
    public float jumpTransitionTime = 0.1f;

    [Header("WebGL设置")]
    public GameObject webGLTouchPanel;
    public Button webGLTouchButton;

    // 私有变量
    private CharacterController _controller;
    private GameObject _mainCamera;
    private Animator _animator;
    private Vector2 _move;
    private float _targetRot;
    private float _rotationVelocity;
    private float _verticalVelocity;
    private Vector3 _moveDirection;
    private float _currentSpeed;
    private int _jumpAnimationHash;
    private bool _isJumping;

    void Start()
    {
        // 初始化组件
        _mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
        _controller = GetComponent<CharacterController>();
        _animator = GetComponent<Animator>();

        // WebGL特定设置
        #if UNITY_WEBGL
        webGLTouchButton.onClick.AddListener(OnWebGLTouch);
        webGLTouchPanel.SetActive(true);
        Application.targetFrameRate = 60;
        #else
        webGLTouchPanel.SetActive(false);
        #endif

        // 动画系统设置
        _animator.updateMode = AnimatorUpdateMode.AnimatePhysics;
        if(jumpAnimation != null)
            _jumpAnimationHash = Animator.StringToHash(jumpAnimation.name);
    }

    void Update()
    {
        HandleGravity();
        HandleJump();
        UpdateAnimation();
    }

    void FixedUpdate()
    {
        HandleMovement();
    }

    void HandleGravity()
    {
        _verticalVelocity = _controller.isGrounded ? 
            Mathf.Max(-0.5f, _verticalVelocity) : 
            _verticalVelocity - 9.81f * Time.deltaTime;
    }

    void HandleJump()
    {
        if (Keyboard.current.spaceKey.wasPressedThisFrame && _controller.isGrounded)
        {
            _verticalVelocity = jumpForce;
            
            if(jumpAnimation != null)
            {
                _animator.CrossFade(_jumpAnimationHash, jumpTransitionTime);
                _isJumping = true;
            }
        }
    }

    void HandleMovement()
    {
        if (_move != Vector2.zero)
        {
            // 计算旋转角度
            Vector3 inputDir = new Vector3(_move.x, 0, _move.y).normalized;
            _targetRot = Mathf.Atan2(inputDir.x, inputDir.z) * Mathf.Rad2Deg 
                        + _mainCamera.transform.eulerAngles.y;
            
            float rotation = Mathf.SmoothDampAngle(
                transform.eulerAngles.y, 
                _targetRot, 
                ref _rotationVelocity, 
                RotationSmoothTime
            );
            
            transform.rotation = Quaternion.Euler(0, rotation, 0);
            _moveDirection = Quaternion.Euler(0, _targetRot, 0) * Vector3.forward;
            _currentSpeed = moveSpeed;
        }
        else
        {
            _currentSpeed = 0;
        }

        // 应用运动
        Vector3 motion = (_moveDirection * _currentSpeed) + (Vector3.up * _verticalVelocity);
        _controller.Move(motion * Time.fixedDeltaTime);
    }

    void UpdateAnimation()
    {
        // 移动动画混合
        float axisY = Mathf.Lerp(
            _animator.GetFloat("AxisY"), 
            _move.magnitude, 
            5f * Time.deltaTime
        );
        _animator.SetFloat("AxisY", axisY);

        // 跳跃状态重置
        if(_isJumping && _controller.isGrounded)
        {
            _isJumping = false;
            _animator.Play("Base Layer.Locomotion", 0, 0.1f);
        }
    }

    void OnMove(InputValue value)
    {
        _move = value.Get<Vector2>();
    }

    #if UNITY_WEBGL
    void OnWebGLTouch()
    {
        webGLTouchPanel.SetActive(false);
        Application.ExternalEval(@"
            var canvas = document.getElementById('canvas');
            canvas.focus();
            canvas.addEventListener('click', function(){canvas.focus();});
        ");
    }
    #endif
}