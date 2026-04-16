using System.Collections;
using System.Collections.Generic;
//using System.Threading.Tasks.Dataflow;
using UnityEngine;

public class Rotate : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0, 0.5f, 0);
    }
}
