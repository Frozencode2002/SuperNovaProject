using UnityEngine;
using HumanBody;
public class TriggerZone : MonoBehaviour
{

    private void OnTriggerEnter(Collider other)
    {
        CapMoveScript characterController = other.GetComponent<CapMoveScript>();
        if (characterController != null)
        {
            characterController.SlowFlag = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        CapMoveScript characterController = other.GetComponent<CapMoveScript>();
        if (characterController != null)
        {
            characterController.SlowFlag = false;
        }
    }
}