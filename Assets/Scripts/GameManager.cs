using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;
    public static GameManager Instance { 
        get { return instance; }
    }
    // for Singleton


    // �Ʒ��� ���ҽ��� �����ϱ� ���� �����̳���.
    private GameObject[] MeatField; // ���ҽ� ������Ʈ�� �ƴ�, �� ������Ʈ�� ã�ƾ� ��.
    private GameObject[] PlantField;
    private GameObject[] TreeField;
    private GameObject[] StoneField;
    private GameObject[] TreasureField;
    //public GameObject[] FishField;

    // �Ʒ��� �÷��̾��� ��ȭ�� �����ϴ� �����̳���.
    public enum ResourceType
    {
        Meat,
        Plant,
        Tree,
        Stone,
        Treasure,
        Fish
    }
    public ItemClass[] ItemClass;              // 0~5�� �����̳ʴ� ���� ���ҽ� ���� �� ��ȭ ���� ��Ÿ��
    public AbillityClass[] AbillityClasses;    // 0~5�� �����̳ʴ� ���� ���ҽ� ���� �� �߰� �ɷ��� ��Ÿ��

   [SerializeField] private int gold;           // �÷��̾ ���� ������ ��带 ��Ÿ��.
   [SerializeField] private float speed;        // �÷��̾��� ���� �̵� �ӵ��� ��Ÿ��.

    public float resourceSpawnTime;




    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else { 
            Destroy(gameObject);
        }

    }


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        ItemClass = new ItemClass[6];
        AbillityClasses = new AbillityClass[6];

        for (int i = 0; i < 6; i++) { 
            ItemClass[i] = new ItemClass();
            AbillityClasses[i] = new AbillityClass();
        }

        MeatField = GameObject.FindGameObjectsWithTag("Rabbit");
        PlantField = GameObject.FindGameObjectsWithTag("Plant");
        TreeField = GameObject.FindGameObjectsWithTag("Tree");
        StoneField = GameObject.FindGameObjectsWithTag("Stone");
        TreasureField = GameObject.FindGameObjectsWithTag("Treasure");
    }

    // Update is called once per frame
    void Update()
    {
        // GameManager�� ��ȭ ���� �� ���ҽ� �����ν��� ������ ������.

        // ���� ��ġ�� �����ϴ� ��� ���ҽ����� Ȱ�� ���¸� ������.
        // ���� Ư�� ���ҽ��� �÷��̾���� ��ȣ�ۿ����� ���� ��Ȱ�� ���¶��,
        // ������ �ð���ŭ ����� �� �ٽ� Ȱ��ȭ�ǵ��� �ϴ� �ڷ�ƾ �ڵ带 ������.
        Respawn();
    }



    void Respawn() {

        // GameManager�� �����ϴ� ��� ���ҽ� �ʵ带 Ȯ���ϸ�
        // ������ ���θ� Ȯ����.
        // ���� Ư�� ���ҽ��� ��Ȱ�� ������ ���, �������� �õ���.

        foreach (var resource in MeatField)
        {
            if (!resource.activeInHierarchy)
            {
                StartCoroutine(Respawning(resource));
            }
        }

        foreach (var resource in PlantField)
        {
            if (!resource.activeInHierarchy)
            {
                StartCoroutine(Respawning(resource));
            }
        }

        foreach (var resource in TreeField)
        {
            if (!resource.activeInHierarchy)
            {
                StartCoroutine(Respawning(resource));
            }
        }

        foreach (var resource in StoneField)
        {
            if (!resource.activeInHierarchy)
            {
                StartCoroutine(Respawning(resource));
            }
        }

        foreach (var resource in TreasureField)
        {
            if (!resource.activeInHierarchy)
            {
                StartCoroutine(Respawning(resource));
            }
        }
    }

    IEnumerator Respawning(GameObject resource) { 

        // ������ �ð��� ����� �� ���ҽ��� Ȱ��ȭ�ϴ� �ڷ�ƾ �Լ�.

        yield return new WaitForSeconds(resourceSpawnTime);
        resource.SetActive(true);
        yield return null;
    }
}
