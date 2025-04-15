using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class juese : MonoBehaviour
{
    private CharacterController _controller;
    private GameObject _mainCamera;

    // Start is called before the first frame update
    void Start()
    {
        if (_mainCamera == null)
        {
            _mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
        }
        _controller = GetComponent<CharacterController>();
        _animator = GetComponent<Animator>();
    }

    Animator _animator;
    float _targetRot = 0.6f;
    public float RotationSmoothTime = 0.1f;
    float _rotationVelocity;
    public float moveSpeed = 100000f;
    // Update is called once per frame
    void Update()
    {

        if (_move != Vector2.zero)
        {
            Vector3 inputDir = new Vector3(_move.x,0.0f,_move.y).normalized;
            _targetRot = Mathf.Atan2(inputDir.x,inputDir.z) * Mathf.Rad2Deg + _mainCamera.transform.eulerAngles.y;
            float rotation = Mathf.SmoothDampAngle(transform.eulerAngles.y,_targetRot, ref _rotationVelocity, RotationSmoothTime);

            transform.rotation = Quaternion.Euler(0.0f,rotation,0.0f);
        }
        var axisY = _animator.GetFloat("AxisY");
        axisY = Mathf.MoveTowards(axisY,_move.magnitude,5f * Time.deltaTime);
        _animator.SetFloat("AxisY",axisY);

         if (_move != Vector2.zero) 
    {
        // 计算移动方向（基于角色旋转后的正前方）
        Vector3 moveDirection = Quaternion.Euler(0.0f, _targetRot, 0.0f) * Vector3.forward;
        // 应用移动速度并执行移动
        _controller.Move(moveDirection.normalized * moveSpeed * Time.deltaTime);
    }

    }
    Vector2 _move;
    void OnMove(InputValue value)
    {
        _move = value.Get<Vector2>();
    }
}