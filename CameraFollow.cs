using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HumanBody;

public class MyFollow : MonoBehaviour
{
    [SerializeField] public float distanceAway = 50;			
    [SerializeField] public float distanceUp = 2;			
    [SerializeField] public float smooth = 3;			
    [SerializeField] private float rotatespeed = 2;
    [SerializeField] private GameObject FollowTarget;
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

        float mouseX = Input.GetAxis("Mouse X") * rotatespeed;
        Quaternion RotateAngle = Quaternion.AngleAxis(mouseX, Vector3.up);
        NewRotate = RotateAngle * Vector3.ProjectOnPlane(LastRotate, Vector3.up).normalized;
        
        targetPosition = follow.position + Vector3.up * distanceUp + NewRotate * distanceAway;
        
        transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * smooth);

        transform.RotateAround(follow.position, Vector3.up, mouseX);
        
        LastRotate = NewRotate;
    }
}