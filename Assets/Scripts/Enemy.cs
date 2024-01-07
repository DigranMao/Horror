using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    private NavMeshAgent agent;
    private Animator anim;
    private Transform playerTransform;
    private bool isMoving = false;
    [SerializeField] private OpenableObject door;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        playerTransform = GameObject.Find("Player").transform;
    }

    // Вызывайте этот метод, чтобы агент двигался в позицию игрока
    public void MoveToPlayer()
    {
        if (playerTransform != null)
        {
            agent.SetDestination(playerTransform.position);
            isMoving = true;
        }
        else
        {
            Debug.LogError("Player not found!");
        }
    }

    void Update()
    {
        // Дополнительная логика агента, если необходимо
        if (door._canOpen)
        {
            MoveToPlayer();
        }
        else
        {
            isMoving = false;
        }

        UpdateAnimation();
    }

    void UpdateAnimation()
    {
        anim.SetBool("Move", isMoving);
    }
}
