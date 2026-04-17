using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Watch : MonoBehaviour
{
    private TextMeshProUGUI label;
    // Start is called before the first frame update
    void Start()
    {
        label = GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        var hour = System.DateTime.Now.Hour;
        var minute = System.DateTime.Now.Minute;
        var second = System.DateTime.Now.Second;

        label.text = $"{hour:00}:{minute:00}:{second:00}";
    }
}
