using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HumanBody;
using Behaviour = HumanBody.Behaviour;

public class MyFollow : MonoBehaviour
{
    [SerializeField] public Behaviour Info;
    [SerializeField] public float distanceAway = 75f;
    [SerializeField] public float AvoidLength = 20f;
    [SerializeField] public float distanceUp = 10f;			
    [SerializeField] public float smooth = 3;			
    [SerializeField] private float rotatespeed = 2;
    [SerializeField] private GameObject FollowTarget;
    [SerializeField] private bool hitFlag;
    private Vector3 targetPosition;		
    private Vector3 LastRotate;
    private Vector3 NewRotate;
    Transform follow;
	
    void Start(){
        follow = FollowTarget.transform;
        LastRotate = -follow.forward * distanceAway;
    }
	
    void Update ()
    {
        if (Time.timeScale == 0) return;
        float mouseX = Input.GetAxis("Mouse X") * rotatespeed;
        Quaternion RotateAngle = Quaternion.AngleAxis(mouseX, Vector3.up);
        NewRotate = RotateAngle * Vector3.ProjectOnPlane(LastRotate, Vector3.up).normalized;
        
        targetPosition = follow.position + Vector3.up * distanceUp + NewRotate * distanceAway;

        Vector3 dir = Quaternion.Euler(new Vector3(0, transform.rotation.eulerAngles.y, 0)) * Vector3.forward;
        // Debug.Log(dir);
        Ray ray = new Ray(targetPosition, dir);
        hitFlag = Physics.Raycast(ray, out RaycastHit info, distanceAway * 0.9f, ~((1 << FollowTarget.layer) | (1 << 14)));
        
        Debug.DrawRay(targetPosition, dir, Color.yellow);
        if (hitFlag)
        {
            // Debug.Log(info.collider.gameObject.name);
            targetPosition = follow.position + Vector3.up * distanceUp + NewRotate * AvoidLength;
        }
         //
        transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * smooth);

        transform.RotateAround(follow.position, Vector3.up, mouseX);
        
        LastRotate = NewRotate;
    }
}