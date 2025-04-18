using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class jingtou : MonoBehaviour
{
    [Header("摄像机组件")]
    public GameObject CameraTarget;
    [Tooltip("主摄像机标签")] 
    public string mainCameraTag = "MainCamera";
    
    [Header("旋转限制")]
    [Range(-90f, 90f)] public float TopClamp = 70.0f;
    [Range(-90f, 90f)] public float BottomClamp = -30.0f;
    
    [Header("控制参数")]
    [Range(0.1f, 2f)] public float lookSensitivity = 0.5f;
    [Tooltip("WebGL点击提示面板")] 
    public GameObject webGLTouchPanel;

    // 私有变量
    private float _cinemachineTargetYaw;
    private float _cinemachineTargetPitch;
    private Vector2 _look;
    private bool _hasFocus;

    #if UNITY_WEBGL
    private const float WebGLFOV = 60f;
    #endif

    void Start()
    {
        // 初始化摄像机参数
        var targetRotation = CameraTarget.transform.rotation.eulerAngles;
        _cinemachineTargetYaw = targetRotation.y;
        _cinemachineTargetPitch = targetRotation.x;

        // WebGL特定初始化
        #if UNITY_WEBGL
        if(webGLTouchPanel != null)
        {
            webGLTouchPanel.SetActive(true);
            Application.targetFrameRate = 60;
            Cursor.lockState = CursorLockMode.None;
        }
        Camera.main.fieldOfView = WebGLFOV;
        #endif
    }

    void Update()
    {
        #if UNITY_WEBGL
        if(!_hasFocus) return;
        #endif

        HandleCameraRotation();
    }

    void HandleCameraRotation()
    {
        if (_look.sqrMagnitude >= 0.01f)
        {
            float deltaTimeMultiplier = 1.0f / Time.deltaTime;
            
            _cinemachineTargetYaw += _look.x * deltaTimeMultiplier * lookSensitivity;
            _cinemachineTargetPitch -= _look.y * deltaTimeMultiplier * lookSensitivity;
        }

        _cinemachineTargetYaw = ClampAngle(_cinemachineTargetYaw, float.MinValue, float.MaxValue);
        _cinemachineTargetPitch = ClampAngle(_cinemachineTargetPitch, BottomClamp, TopClamp);

        CameraTarget.transform.rotation = Quaternion.Euler(
            _cinemachineTargetPitch,
            _cinemachineTargetYaw,
            0.0f
        );
    }

    #if UNITY_WEBGL
    public void OnWebGLFocus()
    {
        _hasFocus = true;
        webGLTouchPanel.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Application.ExternalEval(@"
            var canvas = document.getElementById('canvas');
            canvas.focus();
            canvas.addEventListener('click', function(){ canvas.focus(); });
        ");
    }
    #endif

    public void OnLook(InputValue value)
    {
        #if UNITY_WEBGL
        if(!_hasFocus) return;
        #endif
        
        _look = value.Get<Vector2>() * 0.1f; // 降低输入灵敏度
    }

    private static float ClampAngle(float angle, float min, float max)
    {
        if (angle < -360f) angle += 360f;
        if (angle > 360f) angle -= 360f;
        return Mathf.Clamp(angle, min, max);
    }
}