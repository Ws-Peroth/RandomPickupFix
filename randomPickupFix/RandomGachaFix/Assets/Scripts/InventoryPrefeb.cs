using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryPrefeb : MonoBehaviour
{
    public string name = "";
    public Image myImg;
    private int selectCharactorNumber;
    
    private void OnDisable()
    {
        Destroy(gameObject);
    }

    public void InitInventoryElement(int setNum, Sprite setImg)
    {
        selectCharactorNumber = setNum;
        myImg.sprite = setImg;
    }

    public void IsButtonDown()
    {
        Debug.Log("Call Information charactor Num : " + selectCharactorNumber + "\nName : " + name);
    }

}
