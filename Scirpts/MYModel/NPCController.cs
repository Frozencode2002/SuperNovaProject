using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NPCController : MonoBehaviour
{
    // [SerializeField] private Animator PhysicBody;

    [SerializeField] private Transform Destination;
    
    private Vector3 Target;
    
    public NavMeshAgent Agent;
    
    // Update is called once per frame
    void Update()
    {
        // Target = PhysicBody.GetBoneTransform(HumanBodyBones.Hips).position;
        Target = Destination.position;
        Debug.Assert(Agent.isActiveAndEnabled);
        Agent.SetDestination(Target);
    }
}
