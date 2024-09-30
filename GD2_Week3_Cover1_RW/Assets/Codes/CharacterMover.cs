using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;

public class CharacterMover : MonoBehaviour
{
    private List<SelectableObject> selectedCharacters = new List<SelectableObject>();

    public LayerMask groundLayer;
    public float moveRadius = 1f;
    public float maxDistributeRadius = 2f;
    public GameObject prefabToInstantiate;

    private GameObject instantiatedPrefab;

    void Update()
    {
        if (selectedCharacters.Count > 0 && Input.GetMouseButtonDown(0))
        {
            Vector3 mousePosition = Input.mousePosition;
            Ray ray = Camera.main.ScreenPointToRay(mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, groundLayer))
            {
                // 确保场景中只能有一个 "des" 对象
                EnsureOnlyOneDesObject();

                instantiatedPrefab = Instantiate(prefabToInstantiate, hit.point, Quaternion.identity);
                instantiatedPrefab.tag = "des";  // 设置tag为 "des"
                MoveSelectedCharactersToPosition(hit.point);
            }
        }

        if (selectedCharacters.Count > 0 && instantiatedPrefab != null)
        {
            if (AllCharactersStopped())
            {
                Destroy(instantiatedPrefab);
            }
        }
    }

    // 确保场景中只有一个 "des" 对象，超过则删除最早的
    void EnsureOnlyOneDesObject()
    {
        GameObject[] desObjects = GameObject.FindGameObjectsWithTag("des");

        if (desObjects.Length > 0)
        {
            // 按创建顺序删除最早的一个
            GameObject oldestDesObject = desObjects[0];
            Destroy(oldestDesObject);
        }
    }

    public void SetSelectedCharacters(List<SelectableObject> characters)
    {
        selectedCharacters = characters;
    }

    public void MoveSelectedCharactersToPosition(Vector3 targetPosition)
    {
        List<Vector3> distributedPositions = DistributePositionsAround(targetPosition);

        for (int i = 0; i < selectedCharacters.Count; i++)
        {
            SelectableObject character = selectedCharacters[i];
            NavMeshAgent agent = character.GetComponent<NavMeshAgent>();
            if (agent != null)
            {
                Vector3 adjustedPosition = GetAdjustedPosition(character, distributedPositions[i]);
                agent.SetDestination(adjustedPosition);
            }
        }
    }

    private Vector3 GetAdjustedPosition(SelectableObject character, Vector3 targetPosition)
    {
        Vector3 finalPosition = targetPosition;

        foreach (SelectableObject otherCharacter in selectedCharacters)
        {
            if (otherCharacter != character)
            {
                float distance = Vector3.Distance(otherCharacter.transform.position, finalPosition);
                if (distance < moveRadius)
                {
                    Vector3 direction = (finalPosition - otherCharacter.transform.position).normalized;
                    finalPosition = otherCharacter.transform.position + direction * moveRadius;
                }
            }
        }

        return finalPosition;
    }

    private List<Vector3> DistributePositionsAround(Vector3 targetPosition)
    {
        List<Vector3> distributedPositions = new List<Vector3>();

        float angleStep = 360f / selectedCharacters.Count;

        for (int i = 0; i < selectedCharacters.Count; i++)
        {
            float angle = i * angleStep;
            Vector3 offset = new Vector3(Mathf.Sin(angle * Mathf.Deg2Rad), 0, Mathf.Cos(angle * Mathf.Deg2Rad)) * maxDistributeRadius;
            distributedPositions.Add(targetPosition + offset);
        }

        return distributedPositions;
    }

    private bool AllCharactersStopped()
    {
        foreach (SelectableObject character in selectedCharacters)
        {
            NavMeshAgent agent = character.GetComponent<NavMeshAgent>();
            if (agent != null && (agent.pathPending || agent.remainingDistance > agent.stoppingDistance))
            {
                return false;
            }
        }
        return true;
    }
}
