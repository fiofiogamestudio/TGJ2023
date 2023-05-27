using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffHolder : MonoBehaviour
{
    [Header("Buffs Info")]
    [ReadOnly]
    public BuffData[] BlessingBuffs;
    [ReadOnly]
    public BuffData[] CurseBuffs;

    void Awake()
    {
        loadBuffs();
    }

    void loadBuffs()
    {
        BuffWrapper buffWrapper = DataLoader.LoadJson<BuffWrapper>("Buffs");
        BlessingBuffs = buffWrapper.blessingBuffs;
        CurseBuffs = buffWrapper.curseBuffs;
    }
}
