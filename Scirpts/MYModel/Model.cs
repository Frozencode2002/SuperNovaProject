using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HumanBody
{
    [RequireComponent(typeof(HumanBody.Ragdoll))]
    public class Model : MonoBehaviour
    {
    
        [SerializeField] protected HumanBody.Ragdoll m_activeragdoll;
        public HumanBody.Ragdoll TmpActiveRagdoll
        {
            get
            {
                return m_activeragdoll;
            }
        }
        private void OnValidate()
        {
            if (m_activeragdoll == null)
            {
                // 尝试获取m_activeragdoll，如果获取失败，则在控制台中输出提示语
                Debug.LogWarning("m_activeragdoll is null, please check the script of ActiveRagdoll");
            }
        }
    }
}
