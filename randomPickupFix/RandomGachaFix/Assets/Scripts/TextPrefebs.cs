using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextPrefebs : MonoBehaviour
{
    private void OnDisable()
    {
        PrefebManager.manager.ObjEnque(gameObject);
    }



}