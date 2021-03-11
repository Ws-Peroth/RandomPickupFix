using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextPrefebs : MonoBehaviour
{
    private void OnDisable()
    {
        Debug.Log("DestroyObj");
        Destroy(gameObject);
    }

    private void OnEnable()
    {
        gameObject.transform.localScale = new Vector3(1, 1, 1);
        Debug.Log("MakeObj");
    }

}