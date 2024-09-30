using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GoalTrigger : MonoBehaviour
{
    // Inspector 中可分配的 GameObject 列表，代表需要监控的角色对象
    public List<GameObject> assignedObjects;

    // 允许通过的角色类型，例如 "Water", "Fire", "Electric"
    public string allowedTag;

    // 用于追踪已进入的对象
    private HashSet<GameObject> objectsInTrigger = new HashSet<GameObject>();

    // TextMeshPro对象，用于显示分配对象的计数
    public TMP_Text assignedObjectsCountText;

    void Start()
    {
        // 初始化 TMP 文本
        UpdateAssignedObjectsCount();
    }

    // 当角色进入goal区域时
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(allowedTag))
        {
            objectsInTrigger.Add(other.gameObject);
            UpdateAssignedObjectsCount();
        }
    }

    // 当角色离开goal区域时
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(allowedTag))
        {
            objectsInTrigger.Remove(other.gameObject);
            UpdateAssignedObjectsCount();
        }
    }

    // 检查所有分配的对象是否都在触发区域中
    public bool AreAllAssignedObjectsInTrigger()
    {
        return objectsInTrigger.Count == assignedObjects.Count;
    }

    // 更新显示已分配对象数量的 TMP 文本
    private void UpdateAssignedObjectsCount()
    {
        if (assignedObjectsCountText != null)
        {
            assignedObjectsCountText.text = $"Assigned Objects: {objectsInTrigger.Count}/{assignedObjects.Count}";
        }
    }
}
