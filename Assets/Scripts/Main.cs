using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main : MonoBehaviour
{
    private void Awake()
    {
        GameMgr.Instance.Init();
    }
    // Start is called before the first frame update
    void Start()
    {
        int iHandle = ResourceMgr.Instance.InstantiatePrefabAsync("Cube.prefab", AssetLoadCompleted);
    }

    void AssetLoadCompleted(int iHandle, UnityEngine.Object obj)
    {
        GameObject go = obj as GameObject;
        if (go != null)
        {
            go.transform.localScale = new Vector3(2.0f, 2.0f, 2.0f);
        }
    }
    // Update is called once per frame
    void Update()
    {
        GameMgr.Instance.Update();
    }

    private void OnDestroy()
    {
        GameMgr.Instance.Destroy();
    }
}
