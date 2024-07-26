using UnityEngine;

public class ResourceHandler : MonoBehaviour
{
    // 플레이어가 리소스를 채집하는 과정에서 주고 받아야 하는 메세지를 위한 스크립트.

    public enum ResourceType {
        Meat,
        Plant,
        Tree,
        Stone,
        Treasure,
        Fish
    }
    public float collectTimer;


    [SerializeField]
    private ResourceType type;


    public int GetResourceType() {
        // 플레이어가 리소스와 상호작용 시, 리소스의 타입을 전달하기 위한 함수.

        return (int)type;
    }

    public ItemClass ResourceCreation(AbillityClass Abillity) {

        // 정해진 확률과 추가 능력에 의한 수치를 통해 추출된 결과로
        // 재화를 생성해 GameManager에 전달하는 것을 목적으로 하는 함수

        // Abillity 에 따른 resource 계산

        int normalCount = 2;
        int rareCount = 0;
        int epicCount = 0;
        int prob = Random.Range(0, 100);

        int normalProb = 80;
        int rareProb = 65;
        int epicProb = 50;

        int specialProb = (int)(Abillity.specialGetProb * 100);




        // 아래 코드는 일반, 레어, 에픽 등급의 리소스를 확률에 따라 계산하여 생성하는 코드임.

        int baseProb = normalProb;
        while (prob <= baseProb && normalCount < 6) {
            normalCount++;
            baseProb /= 2;
            prob = Random.Range(0, 100);
        }

        baseProb = rareProb;
        while (prob <= baseProb && rareCount < 6)
        {
            rareCount++;
            baseProb /= 2;
            prob = Random.Range(0, 100);
        }

        baseProb = epicProb;
        while (prob <= baseProb && epicCount < 4)
        {
            epicCount++;
            baseProb /= 2;
            prob = Random.Range(0, 100);
        }
        // 여기까지 일반적인 확률에 의한 리소스 생성 과정임.
        print(normalCount + ", " + rareCount + ", " + epicCount);
        return new ItemClass((int)(normalCount * (1 + Abillity.normalGetProb)),(int)(rareCount * (1 + Abillity.rareGetProb)),(int)(epicCount*(1+Abillity.epicGetProb)),0);
    }
}
