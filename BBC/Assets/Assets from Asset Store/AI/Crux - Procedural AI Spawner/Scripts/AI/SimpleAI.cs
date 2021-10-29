using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Crux.Example
{
    public class SimpleAI : MonoBehaviour
    {
        int WanderCheck = 10;
        public int wanderRange = 40;
        public float StoppingDistance = 2f;
        public bool UseAnimations = true;
        UnityEngine.AI.NavMeshAgent Agent;
        Vector3 startPosition;
        Vector3 destination;
        Animator m_Animator;

        void Start()
        {
            WanderCheck = Random.Range(8, 16);
            InvokeRepeating("Wander", 1.0f, WanderCheck);
            InvokeRepeating("UpdateAnimations", 1.0f, 0.5f);
            m_Animator = GetComponent<Animator>();
            Agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
            startPosition = transform.position;
            Agent.acceleration = 50;
            Agent.angularSpeed = 1000;
            Agent.stoppingDistance = StoppingDistance;


            if (m_Animator == null)
            {
                UseAnimations = false;
            }
        }

        void Wander()
        {
            destination = startPosition + new Vector3(Random.Range((int)-wanderRange * 0.5f - 2, (int)wanderRange * 0.5f + 2), 0, Random.Range((int)-wanderRange * 0.5f - 2, (int)wanderRange * 0.5f + 2));

            if (Agent.pathStatus != UnityEngine.AI.NavMeshPathStatus.PathInvalid)
            {
                Agent.SetDestination(destination);
            }
        }

        void UpdateAnimations()
        {
            if (UseAnimations && gameObject.activeSelf)
            {
                if (Agent.remainingDistance <= StoppingDistance)
                {
                    m_Animator.SetBool("Idle", true);
                    m_Animator.SetBool("Move", false);
                }
                else if (Agent.remainingDistance > StoppingDistance || Agent.velocity.sqrMagnitude > 0)
                {
                    m_Animator.SetBool("Move", true);
                    m_Animator.SetBool("Idle", false);
                }
            }
        }
    }
}