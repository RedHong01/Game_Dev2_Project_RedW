using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TiltGameBoard : MonoBehaviour
{
    public Transform gameBoard;  // 游戏盘对象
    public float rotationSpeed = 5f;  // 控制旋转速度

    private Vector3 lastMousePosition;  // 上一帧鼠标位置
    private Quaternion initialRotation; // 记录初始旋转

    // Start is called before the first frame update
    void Start()
    {
        // 记录游戏盘的初始旋转状态
        initialRotation = gameBoard.rotation;
    }

    // Update is called once per frame
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

        // 如果按下 R 键，则重置游戏盘旋转
        if (Input.GetKeyDown(KeyCode.R))
        {
            ResetRotation();
        }
    }

    // 重置游戏盘的旋转到初始状态
    void ResetRotation()
    {
        gameBoard.rotation = initialRotation;
    }
}