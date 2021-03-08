using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
