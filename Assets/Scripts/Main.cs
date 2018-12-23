using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main : MonoBehaviour
{
    private void Awake()
    {
        DontDestroyOnLoad(this);
        GameMgr.Instance.Init();
    }
    // Start is called before the first frame update
    private void Start()
    {
        GameMgr.Instance.Start();
    }

    private void FixedUpdate()
    {
        GameMgr.Instance.FixedUpdate();
    }

    // Update is called once per frame
    private void Update()
    {
        GameMgr.Instance.Update();
    }
    
    private void LateUpdate()
    {
        GameMgr.Instance.LateUpdate();
    }

    private void OnDestroy()
    {
        GameMgr.Instance.Destroy();
    }
}
