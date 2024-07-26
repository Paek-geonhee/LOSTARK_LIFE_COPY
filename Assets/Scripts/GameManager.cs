using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;
    public static GameManager Instance { 
        get { return instance; }
    }
    // for Singleton


    // 아래는 리소스를 관리하기 위한 컨테이너임.
    private GameObject[] MeatField; // 리소스 오브젝트가 아닌, 몹 오브젝트를 찾아야 함.
    private GameObject[] PlantField;
    private GameObject[] TreeField;
    private GameObject[] StoneField;
    private GameObject[] TreasureField;
    //public GameObject[] FishField;

    // 아래는 플레이어의 재화를 관리하는 컨테이너임.
    public enum ResourceType
    {
        Meat,
        Plant,
        Tree,
        Stone,
        Treasure,
        Fish
    }
    public ItemClass[] ItemClass;              // 0~5번 컨테이너는 각각 리소스 유형 별 재화 수를 나타냄
    public AbillityClass[] AbillityClasses;    // 0~5번 컨테이너는 각각 리소스 유형 별 추가 능력을 나타냄

   [SerializeField] private int gold;           // 플레이어가 현재 소지한 골드를 나타냄.
   [SerializeField] private float speed;        // 플레이어의 현재 이동 속도를 나타냄.

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
        // GameManager는 재화 관리 및 리소스 맵으로써의 역할을 수행함.

        // 고정 위치에 존재하는 모든 리소스들의 활성 상태를 관리함.
        // 만약 특정 리소스가 플레이어와의 상호작용으로 인해 비활성 상태라면,
        // 지정된 시간만큼 경과한 뒤 다시 활성화되도록 하는 코루틴 코드를 실행함.
        Respawn();
    }



    void Respawn() {

        // GameManager가 관리하는 모든 리소스 필드를 확인하며
        // 리스폰 여부를 확인함.
        // 만약 특정 리소스가 비활성 상태인 경우, 리스폰을 시도함.

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

        // 지정된 시간이 경과한 뒤 리소스를 활성화하는 코루틴 함수.

        yield return new WaitForSeconds(resourceSpawnTime);
        resource.SetActive(true);
        yield return null;
    }
}
