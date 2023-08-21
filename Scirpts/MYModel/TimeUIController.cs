using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeUIController : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] public GameScript gameScript;
    public GameObject TimeUI;
    private void Update()
    {
        TimeUI.SetActive(!gameScript.isPaused);
    }

}
