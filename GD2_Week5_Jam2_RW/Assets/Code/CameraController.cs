using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float speed = 5f;  // 移动速度
    public float edgeThreshold = 0.05f;  // 边缘检测阈值
    public float scrollSensitivity = 5f;  // 滚轮缩放灵敏度
    public Vector3 minPosition;  // 摄像头最小位置
    public Vector3 maxPosition;  // 摄像头最大位置

    private Vector3 initialPosition;  // 初始位置
    private bool isLocked = false;  // 是否锁定摄像机

    void Start()
    {
        // 记录摄像头的初始位置
        initialPosition = transform.position;
    }

    void Update()
    {
        // 切换锁定状态
        if (Input.GetKeyDown(KeyCode.Space))
        {
            isLocked = !isLocked;
            Debug.Log("Camera Locked: " + isLocked);
        }

        // 如果没有锁定，处理移动和滚轮缩放
        if (!isLocked)
        {
            HandleEdgeMovement();
            HandleScrollZoom();
        }

        // 检测键盘 R 键重置摄像头位置
        if (Input.GetKeyDown(KeyCode.R))
        {
            ResetCameraPosition();
        }

        // 限制摄像机的位置在指定范围内
        ClampCameraPosition();
    }

    void HandleEdgeMovement()
    {
        Vector3 movement = Vector3.zero;  // 移动增量初始化

        if (Input.mousePosition.x >= Screen.width * (1 - edgeThreshold))
        {
            movement += transform.right * speed * Time.deltaTime;
        }
        if (Input.mousePosition.x <= Screen.width * edgeThreshold)
        {
            movement += -transform.right * speed * Time.deltaTime;
        }
        if (Input.mousePosition.y >= Screen.height * (1 - edgeThreshold))
        {
            movement += transform.forward * speed * Time.deltaTime;
        }
        if (Input.mousePosition.y <= Screen.height * edgeThreshold)
        {
            movement += -transform.forward * speed * Time.deltaTime;
        }

        // 在移动前对位置进行预限制，避免超出边界引发抖动
        Vector3 newPosition = transform.position + movement;

        // 限制摄像机的移动范围，防止超出边界
        newPosition.x = Mathf.Clamp(newPosition.x, minPosition.x, maxPosition.x);
        newPosition.z = Mathf.Clamp(newPosition.z, minPosition.z, maxPosition.z);

        transform.position = newPosition;
    }

    void HandleScrollZoom()
    {
        float scrollInput = Input.GetAxis("Mouse ScrollWheel");
        if (Mathf.Abs(scrollInput) > 0.01f)  // 避免微小滚动误差
        {
            Vector3 newPosition = transform.position;
            newPosition.y += scrollInput * scrollSensitivity;

            // 限制 Y 轴的高度范围
            newPosition.y = Mathf.Clamp(newPosition.y, minPosition.y, maxPosition.y);

            transform.position = newPosition;
        }
    }

    void ClampCameraPosition()
    {
        // 不再需要在这里对 x 和 z 进行限制，HandleEdgeMovement 已经处理
        Vector3 clampedPosition = transform.position;
        clampedPosition.y = Mathf.Clamp(clampedPosition.y, minPosition.y, maxPosition.y);
        transform.position = clampedPosition;
    }

    void ResetCameraPosition()
    {
        transform.position = initialPosition;
    }
}