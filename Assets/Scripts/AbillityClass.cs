using System;
using UnityEngine;

[Serializable]
public class AbillityClass
{
    // ���ҽ� ä�� �� ��ȭ�� �߰������� ��� ���� �߰� �ɷ��� �����ϴ� �����̳���.

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
