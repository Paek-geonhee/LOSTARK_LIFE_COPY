using System;
using UnityEngine;

[Serializable]
public class AbillityClass
{
    // 리소스 채집 시 재화를 추가적으로 얻는 등의 추가 능력을 관리하는 컨테이너임.

    public float normalGetProb;
    public float rareGetProb;
    public float epicGetProb;
    public float collectSpeed;
    public float specialGetProb;
    public float safetyProb;

    public AbillityClass(float normalGetProb=0, float rareGetProb = 0, float epicGetProb = 0, float collectSpeed=0, float specialGetProb = 0, float safetyProb = 0)
    {
        this.normalGetProb = normalGetProb;
        this.rareGetProb = rareGetProb;
        this.epicGetProb = epicGetProb;
        this.collectSpeed = collectSpeed;
        this.specialGetProb = specialGetProb;
        this.safetyProb = safetyProb;
    }   
}
