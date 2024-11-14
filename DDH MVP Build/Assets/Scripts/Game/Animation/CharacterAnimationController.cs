using UnityEngine;
using UnityEngine.AI;

public class CharacterAnimationController : MonoBehaviour
{
    public Animator animator;
    private NavMeshAgent agent;

    void Start()
    {
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        //check remainingDistance for movement and set animation accordingly
        if (agent.hasPath && agent.remainingDistance > agent.stoppingDistance)
        {
            animator.SetFloat("Speed", 1f);  // trigger walk animation
        }
        else
        {
            animator.SetFloat("Speed", 0f);  // trigger idle animation
        }
    }
}