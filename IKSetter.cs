using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HumanBody
{
    public class IKSetter : MonoBehaviour
    {
        [SerializeField] private Animator AnimatorBody;
        [SerializeField] private bool IKFlag;
        private RootMotion.FinalIK.ArmIK ArmIK;
        
        private void Start()
        {
            IKFlag = false;
            ArmIK = AnimatorBody.GetComponent<RootMotion.FinalIK.ArmIK>();
        }

        private void Update()
        {
            if (Input.GetMouseButton(0))
            {
                IKFlag = true;
            }
            else
            {
                IKFlag = false;
            }
            ArmIK.enabled = IKFlag;
        }
    }
}
