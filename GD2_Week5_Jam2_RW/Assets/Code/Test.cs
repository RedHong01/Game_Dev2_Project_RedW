using UnityEngine;

public class DestroyZoneTest : MonoBehaviour
{
    public string playerTag = "Player";  // 玩家标签

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Something collided with DestroyZone: " + other.gameObject.name);

        // 检查碰撞的物体是否带有Player标签
        if (other.CompareTag(playerTag))
        {
            Debug.Log("Player collided with DestroyZone, destroying Player.");
            Destroy(other.gameObject);  // 销毁 Player 对象
        }
    }
}