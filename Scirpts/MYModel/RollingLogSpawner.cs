using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RollingLogSpawner : MonoBehaviour
{
    [System.Serializable]
    public class SpawnInfo
    {
        public Vector3 localPosition;
        public Quaternion localRotation;
    }

    public GameObject rollingLogPrefab;
    public SpawnInfo[] spawnInfos;
    public float spawnInterval = 3f;
    public float destroyAfter = 25f;

    private void Start()
    {
        StartCoroutine(SpawnRollingLogs());
    }

    private IEnumerator SpawnRollingLogs()
    {
        while (true)
        {
            for (int i = 0; i < spawnInfos.Length; i++)
            {
                GameObject spawnedLog = Instantiate(rollingLogPrefab, Vector3.zero, Quaternion.identity, transform);
                spawnedLog.transform.localPosition = spawnInfos[i].localPosition;
                spawnedLog.transform.localRotation = spawnInfos[i].localRotation;
                // Debug.Log(spawnInfos[i].localPosition + " | " + spawnInfos[i].localRotation);
                Destroy(spawnedLog, destroyAfter);
            }
            yield return new WaitForSeconds(spawnInterval);
        }
    }
}