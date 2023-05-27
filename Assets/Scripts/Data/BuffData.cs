public enum BuffType
{
    Blessing, // 祝福
    Curse // 诅咒
}

[System.Serializable]
public class BuffData
{
    public BuffType buffType;
    public string buffName;
    public string buffDescription;
    public string[] buffEffects;
}

[System.Serializable]
public class BuffWrapper
{
    public BuffData[] blessingBuffs; // 祝福
    public BuffData[] curseBuffs; // 诅咒
}