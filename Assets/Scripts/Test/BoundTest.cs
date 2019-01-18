using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[ExecuteInEditMode]
public class BoundTest : MonoBehaviour
{
    public Transform TestTransform = null;
    Collider m_Collider = null;
    // Start is called before the first frame update
    void Start()
    {
        m_Collider = GetComponent<Collider>();
    }

    // Update is called once per frame
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        if (TestTransform != null && m_Collider != null)
        {
            if (m_Collider.bounds.Contains(TestTransform.position))
            {
                Gizmos.color = Color.blue;
            }
        }
        Gizmos.matrix = transform.localToWorldMatrix;
        Gizmos.DrawWireCube(m_Collider.bounds.center - transform.position,m_Collider.bounds.size);
    }
}
