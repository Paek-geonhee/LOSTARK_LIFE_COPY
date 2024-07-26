using UnityEngine;

public class ResourceHandler : MonoBehaviour
{
    // �÷��̾ ���ҽ��� ä���ϴ� �������� �ְ� �޾ƾ� �ϴ� �޼����� ���� ��ũ��Ʈ.

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
        // �÷��̾ ���ҽ��� ��ȣ�ۿ� ��, ���ҽ��� Ÿ���� �����ϱ� ���� �Լ�.

        return (int)type;
    }

    public ItemClass ResourceCreation(AbillityClass Abillity) {

        // ������ Ȯ���� �߰� �ɷ¿� ���� ��ġ�� ���� ����� �����
        // ��ȭ�� ������ GameManager�� �����ϴ� ���� �������� �ϴ� �Լ�

        // Abillity �� ���� resource ���

        int normalCount = 2;
        int rareCount = 0;
        int epicCount = 0;
        int prob = Random.Range(0, 100);

        int normalProb = 80;
        int rareProb = 65;
        int epicProb = 50;

        int specialProb = (int)(Abillity.specialGetProb * 100);




        // �Ʒ� �ڵ�� �Ϲ�, ����, ���� ����� ���ҽ��� Ȯ���� ���� ����Ͽ� �����ϴ� �ڵ���.

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
        // ������� �Ϲ����� Ȯ���� ���� ���ҽ� ���� ������.
        print(normalCount + ", " + rareCount + ", " + epicCount);
        return new ItemClass((int)(normalCount * (1 + Abillity.normalGetProb)),(int)(rareCount * (1 + Abillity.rareGetProb)),(int)(epicCount*(1+Abillity.epicGetProb)),0);
    }
}
