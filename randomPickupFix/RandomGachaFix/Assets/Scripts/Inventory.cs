﻿using Newtonsoft.Json;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    public Sprite[] Charactorimgs;

    public GameObject charactorContent;
    public GameObject informationTextContent;

    public GameObject charactorProfilePrefeb;
    public GameObject informationTextPrefeb;

    string path;

    // Start is called before the first frame update
    void Start()
    {
        path = Application.dataPath + "/charactorInformation.json";
        var loadJson = File.ReadAllText(path);
        CharactorPool initCharactor = JsonConvert.DeserializeObject<CharactorPool>(loadJson);

        MakePrefebs(initCharactor);
    }


    private void MakePrefebs(CharactorPool initCharactor)
    {
        int charactorLength = initCharactor.charactorListWithRarity.Count;
        for (int i = 0; i < charactorLength; i++)
        {           
            for (int j = 0; j < initCharactor.charactorListWithRarity[i].charactor.Count; j++)
            {
                string name = initCharactor.charactorListWithRarity[i].charactor[j].Name;
                InitPrefeb((i * charactorLength) + j, Charactorimgs[i], name);                
            }
        }
    }

    private void InitPrefeb(int setNum, Sprite setImg, string name)
    {
        GameObject makeObj = Instantiate(charactorProfilePrefeb);
        makeObj.transform.SetParent(charactorContent.transform);
        makeObj.GetComponent<InventoryPrefeb>().InitInventoryElement(setNum, setImg);
        makeObj.transform.localScale = new Vector3(1, 1, 1);
        makeObj.transform.localPosition = new Vector3(1, 1, 1);
        makeObj.GetComponent<InventoryPrefeb>().name = name;
    }
}
