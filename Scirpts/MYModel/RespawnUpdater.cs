using System.Collections;
using System.Collections.Generic;
using HumanBody;
using UnityEngine;
using Behaviour = HumanBody.Behaviour;

public class RespawnUpdater : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] public int RespawnIndex;
    [SerializeField] public RaceTimer raceTimer;
    void OnTriggerEnter(Collider other)
    {
        // 检查是否是玩家进入触发器
        CapMoveScript characterController = other.GetComponent<CapMoveScript>();
        if (characterController != null)
        {
            if (RespawnIndex == 5)
            {
                raceTimer.FinishRace();
                return;
            }
            // 更新玩家的重生点
            characterController.UpdateRespawnPoint(RespawnIndex);
        }
    }
}
