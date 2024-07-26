using System;
using UnityEngine;

[Serializable]
public class ItemClass
{
    // 리소스로부터 얻은 재화를 저장하는 컨테이너임.

    public int normalCount;
    public int rareCount;
    public int epicCount;
    public int legendCount;

    public ItemClass(int normalCount=0, int rareCount=0, int epicCount=0, int legendCount=0)
    {
        this.normalCount = normalCount;
        this.rareCount = rareCount;
        this.epicCount = epicCount;
        this.legendCount = legendCount;
    }

    public bool Substractable(ItemClass other) {
        return 
            normalCount - other.normalCount >= 0 && 
            rareCount - other.rareCount >= 0 && 
            epicCount - other.epicCount >= 0 && 
            legendCount - other.legendCount >= 0;
    }

    public static ItemClass operator +(ItemClass a, ItemClass b)
    {
        return new ItemClass(a.normalCount + b.normalCount, a.rareCount + b.rareCount, a.epicCount + b.epicCount, a.legendCount + b.legendCount);
    }

    public static ItemClass operator -(ItemClass a, ItemClass b) {
        return new ItemClass(a.normalCount - b.normalCount, a.rareCount - b.rareCount, a.epicCount - b.epicCount, a.legendCount - b.legendCount);
    }
}
