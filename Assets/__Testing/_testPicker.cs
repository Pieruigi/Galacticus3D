using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class _testPicker : MonoBehaviour
{
    [SerializeField]
    GameObject content;

    [SerializeField]
    OMTB.Gameplay.Picker picker;
    // Start is called before the first frame update
    void Start()
    {
        picker.Init(content);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
