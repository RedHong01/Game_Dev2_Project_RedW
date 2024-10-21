using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class Goal : MonoBehaviour
{
    public TextMeshProUGUI messageText;  // TMP文本对象
    public float displayTime = 3f;  // 显示时间
    public string nextSceneName;  // 下一个场景名称

    private void Start()
    {
        // 确保TMP文本对象初始时不显示
        if (messageText != null)
        {
            messageText.gameObject.SetActive(false);
        }
    }

    // 当有物体进入触发器时调用
    private void OnTriggerEnter(Collider other)
    {
        // 检查是否是带有Player标签的物体
        if (other.CompareTag("Player"))
        {
            StartCoroutine(DisplayMessageAndSwitchScene());
        }
    }

    // 协程：显示信息并切换场景
    private IEnumerator DisplayMessageAndSwitchScene()
    {
        if (messageText != null)
        {
            // 显示TMP文本对象
            messageText.gameObject.SetActive(true);
        }

        // 等待指定的显示时间
        yield return new WaitForSeconds(displayTime);

        // 隐藏TMP文本对象
        if (messageText != null)
        {
            messageText.gameObject.SetActive(false);
        }

        // 切换到下一个场景
        if (!string.IsNullOrEmpty(nextSceneName))
        {
            SceneManager.LoadScene(nextSceneName);
        }
        else
        {
            Debug.LogWarning("Next scene name is not set.");
        }
    }
}