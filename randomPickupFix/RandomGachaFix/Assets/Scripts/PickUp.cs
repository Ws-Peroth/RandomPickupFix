using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public enum Rarities
{
    COMMON = 0,
    RARE,
    EPIC,
    LEGEND,
    EVENT_LEGEND
}

public class PickUp : MonoBehaviour
{
    public GameObject havingTextRect;
    public Text havingCharactorText;

    public Text count;
    public Text eventCount;

    int minRarity = 1;
    int maxRarity = 5;

    int topNumber = 90;
    int rotationNumber = 10;

    public bool isPickUpTurn = false;
    bool pickable = false;

    public int pickupCount = 0;
    public int pickupEventCount = 0;

    CharactorPool charactorPool;
    string path;

    float pickupPercent;
    public int picktryEvent;
    public int picktry;

    float[] rarity = new float[] { 50.0f, 35.0f, 10.0f, 0.6f }; // { common, rare, epic, legend }

    int rarityToInt(Rarities rarity)
    {
        switch(rarity)
        {
            case Rarities.COMMON:
                return 0;

            case Rarities.RARE:
                return 1;

            case Rarities.EPIC:
                return 2;

            case Rarities.LEGEND:
                return 3;

            case Rarities.EVENT_LEGEND:
                return 4;
        }

        return -1;
    }

    Rarities intToRarity(int rarity)
    {
        switch (rarity)
        {
            case 0:
                return Rarities.COMMON;

            case 1:
                return Rarities.RARE;

            case 2:
                return Rarities.EPIC;

            case 3:
                return Rarities.LEGEND;

            case 4:
                return Rarities.EVENT_LEGEND;
        }

        return Rarities.COMMON;
    }

    void LoadData()
    {
        // json에서 값 불러오기
        if (!File.Exists(path))
        {
            return;
        }
        else
        {
            var loadJson = File.ReadAllText(path);
            charactorPool = JsonConvert.DeserializeObject<CharactorPool>(loadJson);
        }
    }

    void UpdateData()
    {
        // 데이터 json에 저장
        string initJson = JsonConvert.SerializeObject(charactorPool, Formatting.Indented);
        File.WriteAllText(path, initJson);
    }

    void InitData()
    {
        path = Application.dataPath + "/charactorInformation.json";

        string[] charactorList_Common = new string[] { "Com_A", "Com_B", "Com_C", "Com_D", "Com_E" };
        string[] charactorList_Rare = new string[] { "Rar_F", "Rar_G", "Rar_H", "Rar_I", "Rar_J" };
        string[] charactorList_Epic = new string[] { "Epi_K", "Epi_L", "Epi_M", "Epi_N", "Epi_O" };
        string[] charactorList_Legend = new string[] { "LEG_P", "LEG_Q", "LEG_R", "LEG_S", "LEG_T" };
        string[] charactorList_EventLegend = new string[] { "EV_PickUp_A", "EV_PickUp_B" };

        pickupPercent = 50.0f;
        // pickupCharactorNumber = 2;
        picktry = 0;
        picktryEvent = 0;

        if (!File.Exists(path))
        {
            // Charactors 형태의 빈 파일 생성
            string initJson = JsonConvert.SerializeObject(
                new CharactorPool(),
                Formatting.Indented
            );
            File.WriteAllText(path, initJson);

            // 생성된 파일을 읽어옴, 그 후 Charactors 형으로 변형
            var loadJson = File.ReadAllText(path);
            CharactorPool initCharactor = JsonConvert.DeserializeObject<CharactorPool>(loadJson);

            initCharactor.AddInitData(charactorList_Common);
            initCharactor.AddInitData(charactorList_Rare);
            initCharactor.AddInitData(charactorList_Epic);
            initCharactor.AddInitData(charactorList_Legend);
            initCharactor.AddInitData(charactorList_EventLegend);

            string json = JsonConvert.SerializeObject(initCharactor, Formatting.Indented);
            File.WriteAllText(path, json);
        }

        LoadData();
    }

    bool IsPercentPass(float passPoint)
    {
        return Random.Range(0.0f, 100.0f) <= passPoint;
    }

    string ReturnCharactorPickup(Rarities rarity)
    {
        Debug.Log(rarity);

        if(rarity >= Rarities.LEGEND)
        {
            if(IsPercentPass(pickupPercent) || isPickUpTurn)
            {
                rarity = Rarities.EVENT_LEGEND;

                picktryEvent = 0;
                isPickUpTurn = false;
            }
            else
            {
                picktryEvent = 0;
                isPickUpTurn = true;
            }
        }

        Charactor pickedCharactor = charactorPool.GetCharactorList(rarity).GetRandomCharactor();
        pickedCharactor.HavingCount++;

        Debug.Log("pickup charactor = " + pickedCharactor.Name);
        return pickedCharactor.Name;
    }

    string ReturnCharactor(Rarities rarity) //해당 레어도의 캐릭터 하나를 반환
    {
        Debug.Log(rarity);

        if (rarity >= Rarities.LEGEND) // legend 확정
        {
            picktry = 0;
        }

        Charactor pickedCharactor = charactorPool.GetCharactorList(rarity).GetRandomCharactor();
        pickedCharactor.HavingCount++;

        Debug.Log("pickup charactor = " + pickedCharactor.Name);
        return pickedCharactor.Name;
    }

    Rarities SetRarity()
    {
        float value = Random.Range(0.0f, 100.0f);

        for(int i = 0; i < rarity.Length; i++)
        {
            value -= rarity[i];
            if(value <= 0.0f)
            {
                return intToRarity(i);
            }
        }

        return Rarities.COMMON;
    } // return 1 ~ 4

    public void EventCharactorPickUp()
    {
        Debug.Log("이벤트 가챠");

        // isEventPickup = true;

        if (!pickable)
        {
            pickable = true;
            for (int i = 0; i < 10; i++)
            {
                pickupEventCount++;
                picktryEvent++;

                if (picktryEvent == topNumber) // 천장
                {
                    if(isPickUpTurn)
                    {
                        Charactor pickedCharactor = charactorPool.GetEventLegendCharactorList().GetRandomCharactor();

                        pickedCharactor.HavingCount++;
                        Debug.Log(
                            "pickup charactor = " + 
                            pickedCharactor.Name
                        );

                        isPickUpTurn = false;
                        picktryEvent = 0;
                    }
                    else // 반천장
                    {
                        ReturnCharactorPickup(Rarities.LEGEND);
                    }
                }
                else if (picktryEvent % rotationNumber == 0) // 10연차에 Epic 확정
                {
                    ReturnCharactorPickup(Rarities.EPIC);
                }
                else
                {
                    ReturnCharactorPickup(SetRarity());
                }

                UpdateData();
            }
        }
        pickable = false;
    }

    public void CharactorPickUp()
    {
        Debug.Log("상시 가챠");

        if (!pickable)
        {
            pickable = true;

            for (int i = 0; i < 10; i++)
            {
                pickupCount++;
                picktry++;

                if (picktry == topNumber)  // 상시 천장
                {
                    ReturnCharactor(Rarities.LEGEND);
                }

                else if (picktry % rotationNumber == 0) // 10연차에 Epic 확정
                {
                    ReturnCharactor(Rarities.EPIC);
                }
                else
                {
                    ReturnCharactor(SetRarity());
                }

                UpdateData();
            }
        }

        pickable = false;

    }

    public void ResetPickup()
    {
        if (File.Exists(path))
        {
            try
            {
                File.Delete(path);
            }
            catch (IOException e)
            {
                print(e.Message);
                return;
            }
        }

        InitData();

        pickupCount = 0;
        pickupEventCount = 0;
        picktryEvent = 0;
        picktry = 0;

    }

    public void ShowHavingCharactorList()
    {
        string text = "Charactor List\n\n";
        string[] rarityName = new string[] { "Common", "Rare", "Epic", "Legend", "Event Legend" };

        int listSize;
        Charactor writeChar;

        for (int i = 0; i < maxRarity; i++)
        {
            listSize = charactorPool.charactorListWithRarity[i].charactor.Count;
            text += "[" + rarityName[i] + "]\n";

            for (int j = 0; j < listSize; j++)
            {
                writeChar = charactorPool.charactorListWithRarity[i].charactor[j];
                if (writeChar.HavingCount > 0)
                {
                    text += " - " + writeChar.Name + "  :   " + writeChar.HavingCount + "\n";
                }
            }
            text += "\n";
        }
        Debug.Log(text);
        
        havingCharactorText.text = text;

        havingTextRect.SetActive(true);
    }

    public void CloseHavingCharactorList()
    {
        havingTextRect.SetActive(false);
    }
    
    public void ShowPickupCharactorList() {
        string[] charactorList_Common = new string[] { "Com_A", "Com_B", "Com_C", "Com_D", "Com_E" };
        string[] charactorList_Rare = new string[] { "Rar_F", "Rar_G", "Rar_H", "Rar_I", "Rar_J" };
        string[] charactorList_Epic = new string[] { "Epi_K", "Epi_L", "Epi_M", "Epi_N", "Epi_O" };

        string[] charactorList_Legendary = new string[] { "LEG_P", "LEG_Q", "LEG_R", "LEG_S", "LEG_T" };
        string[] charactorList_PickupLegendary = new string[] { "EV_PickUp_A", "EV_PickUp_B" };
    }

    void Start()
    {
        havingTextRect.SetActive(false);
        InitData();
    }

    private void Update()
    {
        count.text = "상시 뽑기 : " + pickupCount;
        eventCount.text = "이벤트 뽑기 : " + pickupEventCount;
    }
}

//하나의 캐릭터의 정보를 담고 있는 클래스
public class Charactor
{
    public string Name { get; set; }
    public int HavingCount { get; set; }

    public Charactor(string charactorName)
    {
        this.Name = charactorName;
        this.HavingCount = 0;
    }
}
//특정 레어도를 가진 캐릭터의 리스트
public class CharactorList
{
    public List<Charactor> charactor = new List<Charactor>();

    public Charactor GetRandomCharactor()
    {
        int randIndex = Random.Range(0, charactor.Count);
        return charactor[randIndex];
    }
}
//캐릭터 클래스
public class CharactorPool
{
    public List<CharactorList> charactorListWithRarity = new List<CharactorList>();

    public void AddInitData(string[] charactorList)
    {
        CharactorList tmpCharactorList = new CharactorList();

        foreach (string charactor in charactorList)
        {
            tmpCharactorList.charactor.Add(
                new Charactor(charactor)
            );
        }

        this.charactorListWithRarity.Add(tmpCharactorList);
    }

    public CharactorList GetCharactorList(Rarities rarity)
    {
        switch (rarity)
        {
            case Rarities.COMMON:
                return charactorListWithRarity[0];
            
            case Rarities.RARE:
                return charactorListWithRarity[1];
            
            case Rarities.EPIC:
                return charactorListWithRarity[2];

            case Rarities.LEGEND:
                return charactorListWithRarity[3];

            case Rarities.EVENT_LEGEND:
                return charactorListWithRarity[4];
        }

        return null;
    }

    public CharactorList GetEventLegendCharactorList()
    {
        return charactorListWithRarity[4];
    }
}
