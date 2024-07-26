using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    NavMeshAgent AG;
    Camera mainCam;

    private bool isDash;
    private bool dashable;
    private bool isCollecting;
    private bool canAttack;
    

    public float dashCooltime;

    public Image processBar;
    public TMP_Text count;

    public GameObject UIContainer;
    public GameObject CameraCont;
    public GameObject AnnounceKey;
    public GameObject AnnounceGetFish;

    public GameObject Weapon;
    void Start()
    {
        AG = GetComponent   <NavMeshAgent>();
        mainCam = FindFirstObjectByType<Camera>();
        dashable = true;
        isCollecting = false;
        canAttack = true;

        AnnounceGetFish.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        GetResourceFromRiver();
        NavDestination();
        AttackToRabbit();
        DashToDestinationDirection();
        MoveWithCamera();
        GetResourceFromObject();
    }

    void NavDestination() {
        // 마우스 우클릭 시 카메라를 기준으로 Raycasting을 수행.
        // Ray가 충돌한 Plane을 플레이어 객체의 목적지로 지정하여 Navigating 수행.

        if (Input.GetMouseButton(1) && !isDash && !isCollecting) {
            var ray = mainCam.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit)){ 
                var dst = hit.point;
                AG.SetDestination(dst);
            }
        }
    }

    void AttackToRabbit() {
        

        if (Input.GetKeyDown(KeyCode.Q)) {
            AG.SetDestination(transform.position);
            WeaponMove WM = Weapon.GetComponent<WeaponMove>();

            var ray = mainCam.ScreenPointToRay(Input.mousePosition);
            Physics.Raycast(ray, out RaycastHit hit);

            WM.dest = hit.point;
            WM.transform.position = new Vector3(transform.position.x, transform.position.y + 3f, transform.position.z);


            Weapon.SetActive(true);
            WM.SetVelocity();
        }
    }
    void DashToDestinationDirection() {
        // 스페이스바 입력 시 지정된 방향으로 빠르게 이동함.
        // 필요 요소 : 쿨타임 적용.
        // 만약 쿨타임이 지나지 않아 dashable이 false인 경우 함수를 즉시 종료.

        if (Input.GetKeyDown(KeyCode.Space) && dashable){
            isDash = true;
            StartCoroutine(Dash());
        }
    }

    IEnumerator Dash() {
        var ray = mainCam.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            float mult = 5;

            var dst = (hit.point - transform.position).normalized * mult;
            AG.speed = AG.speed * mult;
            AG.acceleration = AG.acceleration * mult;
            AG.SetDestination(dst + transform.position);

            yield return new WaitForSeconds(0.2f);

            AG.speed = AG.speed / mult;
            AG.acceleration = AG.acceleration / mult;

            isDash = false;
            dashable = false;
        }
        StartCoroutine(refreshDash());

        yield return null;
    }

    IEnumerator refreshDash() {
        yield return new WaitForSeconds(dashCooltime);
        dashable = true;
    }

    void MoveWithCamera() { 
        // 카메라의 위치를 플레이어를 기준으로 고정시킴.
        // 플레이어가 움직임에 따라 카메라가 플레이어를 따라감.

        CameraCont.transform.position = new Vector3(transform.position.x, CameraCont.transform.position.y, transform.position.z);
    }


    void GetResourceFromRiver()
    {
        // 낚시는 다른 리소스 채집 방식이랑 다름.

        // 강 영역에서 E키를 경우 일련의 로직을 수행.
        if (Input.GetKeyDown(KeyCode.E) && !isCollecting)
        {
            var ray = mainCam.ScreenPointToRay(Input.mousePosition);

            Physics.Raycast(ray, out var river);
            print("낚시시도"+ river.collider.gameObject.layer);
            if (river.collider.gameObject.layer == (7))
            {
                AG.SetDestination(transform.position);
                print("낚시 시작");
                StartCoroutine(Fishing(river.collider.gameObject));
            }
        }
    }



    IEnumerator Fishing(GameObject river) {
        float castingSpeed = 3000;
        bool isCollectable = false;
        int rtype = (int)GameManager.ResourceType.Fish;

        ResourceHandler RH = river.GetComponent<ResourceHandler>(); 

        float timer = (castingSpeed * (1-GameManager.Instance.AbillityClasses[rtype].collectSpeed))/1000;
        yield return new WaitForSeconds(0.2f);
        isCollecting = true;
        for (float i = 0; i < timer && !isCollectable; i += Time.deltaTime) {
            print(i);
            if (Input.GetKeyDown(KeyCode.E)) { 
                isCollectable = true;
            }
            yield return new WaitForNextFrameUnit();
        }

        if (isCollectable) {
            isCollecting = false;
            yield break;
        }




        isCollectable = true;
        AnnounceGetFish.SetActive(true);
        for (float i = 0; i < 3f && isCollectable; i += Time.deltaTime)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                //GameManager.Instance.ItemClass[rtype] += new ItemClass(2, 2, 2, 2);

                ItemClass result = RH.ResourceCreation(GameManager.Instance.AbillityClasses[rtype]);


                // 특수 획득률에 의해 리소스를 한번 더 획득할 수 있음.
                int specialProb = (int)(GameManager.Instance.AbillityClasses[rtype].specialGetProb * 100);
                int prob = Random.Range(0, 100);
                if (prob < specialProb)
                {
                    result += RH.ResourceCreation(GameManager.Instance.AbillityClasses[rtype]);
                }
                GameManager.Instance.ItemClass[rtype] += result;
                isCollectable =false;
            }
            yield return new WaitForNextFrameUnit();
        }
        isCollecting=false;
        AnnounceGetFish.SetActive(false);
    }
    /// <summary>
    /// /////////////////////////////////////////////////////////////////////////////////////////////////////
    /// </summary>
    void GetResourceFromObject() {
        // 플레이어가 리소스 주변에서 G키를 입력하는 경우 채집을 시도함.
        // 플레이어 기준 일정 거리 내에 리소스가 없는 경우 무시.
        Collider[] resources = Physics.OverlapSphere(transform.position, 3f, (1 << 6));
        if (resources.Length == 0)
        {
            AnnounceKey.SetActive(false);
            return;
        }
        else {
            AnnounceKey.SetActive(!isCollecting);
        }


        if (Input.GetKeyDown(KeyCode.G) && !isCollecting) {
            
            // 반원 거리 내 리소스를 인식
   
            AG.SetDestination(transform.position);
            // 주위에 리소스가 있는 경우 수집 수행과 동시에 움직임을 멈춤.

            
            StartCoroutine(ResourceCreate(resources[0]));
            // 채집 타이머 실행. 채집 중 다른 행동 시도 시 함수를 종료.

        }
    }

    IEnumerator ResourceCreate(Collider resource) {

        // 정해진 공식에 의해 리소스에 대한 재화를 생성하여 ItemClass를 반환하는 함수임.
        // 상호작용 수행 시 일정 시간 대기한 후 재화를 생성함.
        // 만약 대기시간 전에 다른 행동을 시도하면 해당 함수를 종료함. (getKey 가 true인 경우 재화 생성 기각)


        float timer = 4000;
        bool getKey = false;
        isCollecting = true;

        AnnounceKey.SetActive(false);
        UIContainer.SetActive(true);

        ResourceHandler RH = resource.GetComponent<ResourceHandler>();
        int rtype = RH.GetResourceType();
        timer *= (1-GameManager.Instance.AbillityClasses[rtype].collectSpeed);


        //////////////////////////////////
    
        // 지정된 시간만큼 경과하도록 해주어야함.
        // 현재 . FPS가 달라지면 경과하는 시간이 변함

        float sec = timer / 1000;
        for (float i = 0; i < sec && !getKey; i += Time.deltaTime) {
            //print(i);
            processBar.fillAmount = (sec - i) / sec;
            count.text = (sec-i).ToString("0.0s");

            if (Input.anyKeyDown && !Input.GetKeyDown(KeyCode.G) || Input.GetMouseButton(1) || !resource.gameObject.activeInHierarchy) getKey = true;
            yield return new WaitForNextFrameUnit();
        }

        ///////////////////////////////////
        



        processBar.fillAmount = 1f;
        UIContainer.SetActive(false);

        isCollecting = false;







        // 만약 채집 중 다른 키를 누르지 않아 정상적으로 채집이 완료된 경우
        // 혹은 채집 완료 전 리소스가 디스폰되면 채집이 중단되어야 함.

        if (!getKey)
        {
            // 인식된 리소스의 정보를 가져와 적절한 장비를 선택한 뒤
            // 재화를 생성하여 GameManager로 전달함.
            

            ItemClass result = RH.ResourceCreation(GameManager.Instance.AbillityClasses[rtype]);


            // 특수 획득률에 의해 리소스를 한번 더 획득할 수 있음.

            int specialProb = (int)(GameManager.Instance.AbillityClasses[rtype].specialGetProb * 100);
            int prob = Random.Range(0, 100);
            if (prob < specialProb)
            {
                result += RH.ResourceCreation(GameManager.Instance.AbillityClasses[rtype]);
            }
            GameManager.Instance.ItemClass[rtype] += result;

            // 이후 리소스를 비활성화
            resource.gameObject.SetActive(false);
            yield return null;
        }
    }
}
