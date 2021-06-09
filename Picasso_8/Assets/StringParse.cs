using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class StringParse : MonoBehaviour
{
    // Start is called before the first frame update
    public Vector3[] points = new Vector3[10];
    void Start()
    {
        for(int i = 0; i<points.Length; i++)
        {
            points[i] = new Vector3(i + 1, i + 1, i + 1);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnSeri()
    {
       
    }
}
