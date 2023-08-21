using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishUIController : MonoBehaviour
{
    [SerializeField] public GameScript gameScript;
    public GameObject FinishUI;
    private void Update()
    {
        FinishUI.SetActive(!gameScript.isPaused);
    }
}
