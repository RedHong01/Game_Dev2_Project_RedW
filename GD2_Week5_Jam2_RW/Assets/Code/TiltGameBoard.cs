using UnityEngine;

public class TiltGameBoard : MonoBehaviour
{
    public Transform gameBoard;  // 游戏盘对象
    public float rotationSpeed = 5f;  // 控制旋转速度
    public float scrollRotationSpeed = 50f;  // 鼠标滚轮控制旋转的速度
    public bool invertScroll = false;  // 是否反转鼠标滚轮控制

    private Vector3 lastMousePosition;  // 上一帧鼠标位置
    private Quaternion initialRotation; // 记录初始旋转

    void Start()
    {
        // 记录游戏盘的初始旋转状态
        initialRotation = gameBoard.rotation;
    }

    void Update()
    {
        // 鼠标拖拽控制游戏盘旋转
        if (Input.GetMouseButtonDown(0))
        {
            lastMousePosition = Input.mousePosition;
        }

        if (Input.GetMouseButton(0))
        {
            Vector3 deltaMousePosition = Input.mousePosition - lastMousePosition;

            float rotateX = deltaMousePosition.y * rotationSpeed * Time.deltaTime;
            float rotateZ = -deltaMousePosition.x * rotationSpeed * Time.deltaTime;

            gameBoard.Rotate(rotateX, 0, rotateZ, Space.World);

            lastMousePosition = Input.mousePosition;
        }

        // 鼠标滚轮控制Y轴旋转
        HandleScrollRotation();

        // 如果按下 R 键，则重置游戏盘旋转
        if (Input.GetKeyDown(KeyCode.R))
        {
            ResetRotation();
        }
    }

    // 鼠标滚轮控制Y轴旋转
    void HandleScrollRotation()
    {
        float scrollInput = Input.GetAxis("Mouse ScrollWheel");

        if (scrollInput != 0f)
        {
            float rotationDirection = invertScroll ? -1f : 1f;  // 判断是否反转滚轮逻辑
            float rotateY = scrollInput * scrollRotationSpeed * rotationDirection * Time.deltaTime;

            gameBoard.Rotate(0, rotateY, 0, Space.World);
        }
    }

    // 重置游戏盘的旋转到初始状态
    void ResetRotation()
    {
        gameBoard.rotation = initialRotation;
    }
}