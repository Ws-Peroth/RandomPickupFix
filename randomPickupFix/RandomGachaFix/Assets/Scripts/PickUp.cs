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
    public Text CharactorListText;
    public Text count;
    public Text eventCount;

    private CharactorPool charactorPool;

    readonly string[] rarityName = new string[] { "Common", "Rare", "Epic", "Legend", "Event Legend" };
    readonly float[] rarity = new float[] { 50.0f, 35.0f, 10.0f, 0.6f };
    readonly float pickupPercent = 50f;
    readonly int rotationNumber = 10;
    readonly int topNumber = 90;

    public bool isPickUpTurn = false;
    public int pickupEventCount = 0;
    public int pickupCount = 0;
    public int picktryEvent = 0;
    public int picktry = 0;
    
    private bool pickable = false;
    private string path;

    public void EventPickupButtonDown()
    {
        Debug.Log("이벤트 가챠");
        if (!pickable)
        {
            pickable = true;

            for (int i = 0; i < 10; i++)
            {
                EventCharactorPickUp();
            }

            pickable = false;
        }
    }

    public void PickupButtonDown()
    {
        Debug.Log("상시 가챠");

        if (!pickable)
        {
            pickable = true;

            for (int i = 0; i < 10; i++)
            {
                CharactorPickUp();
            }

            pickable = false;
        }
    }

    public void CloseCharactorListUI()
    {
        havingTextRect.SetActive(false);
    }
    
    public void ShowPickupCharactorList()
    {
        CharactorListText.text = WritePickupCharactorList();
        havingTextRect.SetActive(true);
    }

    private string WritePickupCharactorList()
    {
        string text = "Pickup Charactor List\n\n";
        int listSize;
        Charactor writeChar;

        for (int i = 0; i <= RarityToInt(Rarities.EVENT_LEGEND); i++)
        {
            listSize = charactorPool.charactorListWithRarity[i].charactor.Count;
            text += "[" + rarityName[i] + "]\n";

            for (int j = 0; j < listSize; j++)
            {
                writeChar = charactorPool.charactorListWithRarity[i].charactor[j];
                if (writeChar.HavingCount > 0)
                {
                    text += " - " + writeChar.Name + "\n";
                }
            }

            text += "\n";
        }

        Debug.Log(text);
        return text;
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
        CharactorListText.text = WriteHavingCharactorText();
        havingTextRect.SetActive(true);
    }

    private Rarities IntToRarity(int rarity)
    {
        switch (rarity)
        {
            case 0: return Rarities.COMMON;
            case 1: return Rarities.RARE;
            case 2: return Rarities.EPIC;
            case 3: return Rarities.LEGEND;
            case 4: return Rarities.EVENT_LEGEND;
            default: return Rarities.COMMON;
        }
    }

    private Rarities SetRarity()
    {
        float value = Random.Range(0.0f, 100.0f);

        for (int i = 0; i < rarity.Length; i++)
        {
            value -= rarity[i];
            if (value <= 0.0f)
            {
                return IntToRarity(i);
            }
        }

        return Rarities.COMMON;
    }

    private int RarityToInt(Rarities rarity)
    {
        switch (rarity)
        {
            case Rarities.COMMON: return 0;
            case Rarities.RARE: return 1;
            case Rarities.EPIC: return 2;
            case Rarities.LEGEND: return 3;
            case Rarities.EVENT_LEGEND: return 4;
            default: return -1;
        }
    }

    private bool IsPercentPass(float passPoint)
    {
        return Random.Range(0.0f, 100.0f) <= passPoint;
    }

    private string[] GetInitializeCharactorList(Rarities rarity)
    {
        switch (rarity)
        {
            case Rarities.COMMON: return new string[] { "Com_A", "Com_B", "Com_C", "Com_D", "Com_E" };
            case Rarities.RARE: return new string[] { "Rar_F", "Rar_G", "Rar_H", "Rar_I", "Rar_J" };
            case Rarities.EPIC: return new string[] { "Epi_K", "Epi_L", "Epi_M", "Epi_N", "Epi_O" };
            case Rarities.LEGEND: return new string[] { "LEG_P", "LEG_Q", "LEG_R", "LEG_S", "LEG_T" };
            case Rarities.EVENT_LEGEND: return new string[] { "EV_PickUp_A", "EV_PickUp_B" };
            default: return null;
        }
    }

    private string ReturnEventCharactor(Rarities rarity)
    {
        Debug.Log(rarity);

        if (rarity >= Rarities.LEGEND)
        {
            if (IsPercentPass(pickupPercent) || isPickUpTurn)
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

    private string ReturnCharactor(Rarities rarity)
    {
        Debug.Log(rarity);

        if (rarity >= Rarities.LEGEND)
        {
            picktry = 0;
        }

        Charactor pickedCharactor = charactorPool.GetCharactorList(rarity).GetRandomCharactor();
        pickedCharactor.HavingCount++;

        Debug.Log("pickup charactor = " + pickedCharactor.Name);
        return pickedCharactor.Name;
    }

    private string WriteHavingCharactorText()
    {
        string text = "Charactor Inentory\n\n";
        int listSize;
        Charactor writeChar;

        for (int i = 0; i <= RarityToInt(Rarities.EVENT_LEGEND); i++)
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
        return text;
    }

    private void LoadData()
    {
        // json에서 값 불러오기
        if (!File.Exists(path)) return;
        else
        {
            var loadJson = File.ReadAllText(path);
            charactorPool = JsonConvert.DeserializeObject<CharactorPool>(loadJson);
        }
    }

    private void UpdateData()
    {
        // 데이터 json에 저장
        string initJson = JsonConvert.SerializeObject(charactorPool, Formatting.Indented);
        File.WriteAllText(path, initJson);
    }

    private void InitData()
    {
        path = Application.dataPath + "/charactorInformation.json";

        if (!File.Exists(path))
        {
            string initJson = JsonConvert.SerializeObject(
                new CharactorPool(), Formatting.Indented
            );

            File.WriteAllText(path, initJson);

            var loadJson = File.ReadAllText(path);
            CharactorPool initCharactor = JsonConvert.DeserializeObject<CharactorPool>(loadJson);

            for (Rarities i = Rarities.COMMON; i <= Rarities.EVENT_LEGEND; i++)
            {
                initCharactor.AddInitData(GetInitializeCharactorList(i));
            }

            string json = JsonConvert.SerializeObject(initCharactor, Formatting.Indented);
            File.WriteAllText(path, json);
        }
        LoadData();
    }

    private void GetEventLegendCharactor()
    {
        Charactor pickedCharactor = charactorPool.GetEventLegendCharactorList().GetRandomCharactor();

        pickedCharactor.HavingCount++;

        Debug.Log("pickup charactor = " + pickedCharactor.Name);

        isPickUpTurn = false;
        picktryEvent = 0;
    }

    private void CharactorPickUp()
    {

        pickupCount++;
        picktry++;

        if (picktry == topNumber)
            ReturnCharactor(Rarities.LEGEND);

        else if (picktry % rotationNumber == 0)
            ReturnCharactor(Rarities.EPIC);

        else
            ReturnCharactor(SetRarity());

        UpdateData();

    }

    private void EventCharactorPickUp()
    {
        pickupEventCount++;
        picktryEvent++;

        if (picktryEvent == topNumber)
        {
            if (isPickUpTurn)
                GetEventLegendCharactor();
            else
                ReturnEventCharactor(Rarities.LEGEND);
        }

        else if (picktryEvent % rotationNumber == 0)
            ReturnEventCharactor(Rarities.EPIC);

        else
            ReturnEventCharactor(SetRarity());

        UpdateData();
    }

    private void Start()
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