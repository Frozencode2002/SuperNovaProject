using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pendulum : MonoBehaviour
{
    public HingeJoint hingeJoint; 
    public float motorSpeed = 20f;
    public float motorForce = 150000000f;
    public float AngleLimit = 30f;
    public bool ClockWise = true;
    [SerializeField] public AnimationCurve JointSpeed;

    private JointMotor motor;
    private bool trigger = true;
    private bool isClockwise = true;

    void Start()
    {
        motor = hingeJoint.motor;
        motor.targetVelocity = motorSpeed;
        motor.force = motorForce;
        hingeJoint.motor = motor;
        hingeJoint.useMotor = true;
    }

    void Update()
    {
        motor = hingeJoint.motor;
        motor.targetVelocity = Mathf.Lerp(0, motorSpeed, Mathf.Cos(hingeJoint.angle * 2f * Mathf.Deg2Rad)) * (ClockWise ? 1 : -1);
        // Debug.Log(hingeJoint.angle + " V: " + motor.targetVelocity);
        if (hingeJoint.angle < -40f && trigger)
        {
            trigger = false;
            ClockWise = !ClockWise;
        }
        if (-1f < hingeJoint.angle && hingeJoint.angle < 1f)
        {
            trigger = true;
        }
        if (hingeJoint.angle > 40f && trigger)
        {
            trigger = false;
            ClockWise = !ClockWise;
        }
        hingeJoint.motor = motor;
    }
    
}