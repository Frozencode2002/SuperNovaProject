using System.Collections;
using UnityEngine;

namespace HumanBody
{
    public class HandGrab : MonoBehaviour
    {
        private FixedJoint fixedJoint; // 用于手部和墙面之间的连接关节
        private bool isNPC;
        private void OnCollisionEnter(Collision collision)
        {
            // 检查手部是否已抓取墙面
            if (fixedJoint == null && Input.GetMouseButton(0))
            {
                // 创建 FixedJoint 关节并连接手部和墙面
                    Rigidbody wallRigidbody;
                    if (!collision.gameObject.TryGetComponent<Rigidbody>(out wallRigidbody))
                    {
                        isNPC = false;
                        // wallRigidbody = collision.gameObject.AddComponent<Rigidbody>();
                    }
                    else
                    {
                        isNPC = true;
                        wallRigidbody = collision.gameObject.GetComponent<Rigidbody>();
                    }
                    // 这里有bug
                    // wallRigidbody.isKinematic = true;
                    
                    fixedJoint = gameObject.AddComponent<FixedJoint>();
                    
                    if (isNPC)
                    {
                        fixedJoint.connectedBody = wallRigidbody;
                    }
                    
                    StartCoroutine(ReleaseAfterDelay(5f));
            }
        }

        private void Update()
        {
            Release();
        }
        
        public void Release()
        {
            if (!Input.GetMouseButton(0))
            {
                ForceRelease();
            }
        }
        
        private void ForceRelease()
        {
            if (fixedJoint != null)
            {
                Rigidbody wallRigidbody = fixedJoint.connectedBody;

                Destroy(fixedJoint);
                fixedJoint = null;

                // if (!isNPC) Destroy(wallRigidbody);
            }
        }
        
        private IEnumerator ReleaseAfterDelay(float delay)
        {
            yield return new WaitForSeconds(delay);
            ForceRelease();
        }
    }
}
