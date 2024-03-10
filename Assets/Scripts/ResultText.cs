using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ResultText : MonoBehaviour
{
    [SerializeField] TMP_Text result;
    // Start is called before the first frame update
    void Start()
    {
        result.text = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>().result;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
