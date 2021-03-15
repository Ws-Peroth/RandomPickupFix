using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryElement : MonoBehaviour
{
    public Sprite[] charactorImages;
    private int selectImgNumber;
    
    private void OnDisable()
    {
        Destroy(gameObject);
    }

    public int SetImgNumber(int setNum)
    {
        if (setNum < charactorImages.Length)
        {
            selectImgNumber = setNum;
            gameObject.GetComponent<Image>().sprite = charactorImages[setNum];
        }
        else
        {
            selectImgNumber = -1;
            Debug.Log("set Num : Out Of Index");
        }
       
        return 0;
    }

    public void IsButtonDown()
    {
        Debug.Log("Call Information charactor Num : " + selectImgNumber);
    }

}
