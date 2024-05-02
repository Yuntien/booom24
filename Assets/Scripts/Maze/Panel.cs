using UnityEngine;
using System.Collections.Generic;

public class Panel : MonoBehaviour
{
    private List<Screw> screws;  // The screws attached to this panel

    void Start()
    {
        screws = new List<Screw>(GetComponentsInChildren<Screw>());
    }

    void Update()
    {
        screws.RemoveAll(screw => screw == null);
        if (screws.Count == 0)
        {
            Debug.Log("卸除完成");
           // Destroy(gameObject);
        }
    }
}
