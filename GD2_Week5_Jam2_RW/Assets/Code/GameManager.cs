using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    [Header("Destroy Zone")]
    public Collider destroyZone;  // 摧毁区域
    public string targetTag = "Player";

    [Header("Player Settings")]
    public GameObject playerPrefab;  // 玩家预制件
    public Transform playerSpawnPoint;  // 玩家复活位置
    private GameObject playerInstance;  // 当前的玩家实例

    [Header("Timer Settings")]
    public float countdownTime = 60f;  // 倒计时设置
    public string nextSceneName;  // 倒计时结束后的场景名称
    public TextMeshProUGUI countdownText;  // TextMeshPro UI 元素

    [Header("Score Settings")]
    public int score = 0;  // 游戏分数

    void Start()
    {
        // 游戏开始时创建一个玩家的副本并设为非激活状态
        if (playerPrefab != null && playerSpawnPoint != null)
        {
            playerInstance = Instantiate(playerPrefab, playerSpawnPoint.position, playerSpawnPoint.rotation);
            playerInstance.SetActive(false);
        }
    }

    void Update()
    {
        // 更新倒计时
        countdownTime -= Time.deltaTime;

        // 更新显示倒计时文本
        if (countdownText != null)
        {
            countdownText.text = "Time Remaining: " + Mathf.CeilToInt(countdownTime).ToString();
        }

        // 每帧检查游戏中是否存在Player
        if (GameObject.FindGameObjectsWithTag("Player").Length <= 0)
        {
            RespawnPlayer();  // 如果没有玩家，复活玩家
        }

        // 检测按下 R 键重置玩家位置
        if (Input.GetKeyDown(KeyCode.R))
        {
            ResetPlayerPosition();
        }

        // 倒计时结束时切换场景
        if (countdownTime <= 0)
        {
            SwitchToNextScene();
        }
    }

    // 当对象进入触发器时调用
    private void OnTriggerEnter(Collider other)
    {
        // 如果指定了标签，只摧毁匹配标签的对象
        if (!string.IsNullOrEmpty(targetTag))
        {
            if (other.CompareTag(targetTag))
            {
                DestroyObject(other.gameObject);
            }
        }
    }

    // 玩家复活功能
    void RespawnPlayer()
    {
        if (playerInstance != null)
        {
            playerInstance.SetActive(true);  // 激活之前创建的玩家副本
        }
        else if (playerPrefab != null && playerSpawnPoint != null)
        {
            // 如果没有副本，则重新实例化一个新的玩家
            playerInstance = Instantiate(playerPrefab, playerSpawnPoint.position, playerSpawnPoint.rotation);
        }
    }

    // 重置玩家位置到生成点，并且设置为 public 以便从按钮点击中调用
    public void ResetPlayerPosition()
    {
        if (playerInstance != null && playerSpawnPoint != null)
        {
            // 将玩家的位置和旋转重置为生成点的位置和旋转
            playerInstance.transform.position = playerSpawnPoint.position;
            playerInstance.transform.rotation = playerSpawnPoint.rotation;
            Debug.Log("Player position reset to spawn point.");
        }
    }

    // 摧毁对象并打印日志
    private void DestroyObject(GameObject obj)
    {
        Debug.Log(obj.name + " entered the Destroy Zone and was destroyed.");
        Destroy(obj);  // 销毁进入触发器的对象
    }

    // 切换到下一个场景
    void SwitchToNextScene()
    {
        if (!string.IsNullOrEmpty(nextSceneName))
        {
            SceneManager.LoadScene(nextSceneName);
        }
        else
        {
            Debug.LogWarning("Next scene name is not set.");
        }
    }

    // 增加分数的方法，其他代码可以调用
    public void AddScore()
    {
        score += 1;
        Debug.Log("Score: " + score);
    }
}