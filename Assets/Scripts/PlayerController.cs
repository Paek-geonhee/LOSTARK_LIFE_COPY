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
        // ���콺 ��Ŭ�� �� ī�޶� �������� Raycasting�� ����.
        // Ray�� �浹�� Plane�� �÷��̾� ��ü�� �������� �����Ͽ� Navigating ����.

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
        // �����̽��� �Է� �� ������ �������� ������ �̵���.
        // �ʿ� ��� : ��Ÿ�� ����.
        // ���� ��Ÿ���� ������ �ʾ� dashable�� false�� ��� �Լ��� ��� ����.

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
        // ī�޶��� ��ġ�� �÷��̾ �������� ������Ŵ.
        // �÷��̾ �����ӿ� ���� ī�޶� �÷��̾ ����.

        CameraCont.transform.position = new Vector3(transform.position.x, CameraCont.transform.position.y, transform.position.z);
    }


    void GetResourceFromRiver()
    {
        // ���ô� �ٸ� ���ҽ� ä�� ����̶� �ٸ�.

        // �� �������� EŰ�� ��� �Ϸ��� ������ ����.
        if (Input.GetKeyDown(KeyCode.E) && !isCollecting)
        {
            var ray = mainCam.ScreenPointToRay(Input.mousePosition);

            Physics.Raycast(ray, out var river);
            print("���ýõ�"+ river.collider.gameObject.layer);
            if (river.collider.gameObject.layer == (7))
            {
                AG.SetDestination(transform.position);
                print("���� ����");
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


                // Ư�� ȹ����� ���� ���ҽ��� �ѹ� �� ȹ���� �� ����.
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
        // �÷��̾ ���ҽ� �ֺ����� GŰ�� �Է��ϴ� ��� ä���� �õ���.
        // �÷��̾� ���� ���� �Ÿ� ���� ���ҽ��� ���� ��� ����.
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
            
            // �ݿ� �Ÿ� �� ���ҽ��� �ν�
   
            AG.SetDestination(transform.position);
            // ������ ���ҽ��� �ִ� ��� ���� ����� ���ÿ� �������� ����.

            
            StartCoroutine(ResourceCreate(resources[0]));
            // ä�� Ÿ�̸� ����. ä�� �� �ٸ� �ൿ �õ� �� �Լ��� ����.

        }
    }

    IEnumerator ResourceCreate(Collider resource) {

        // ������ ���Ŀ� ���� ���ҽ��� ���� ��ȭ�� �����Ͽ� ItemClass�� ��ȯ�ϴ� �Լ���.
        // ��ȣ�ۿ� ���� �� ���� �ð� ����� �� ��ȭ�� ������.
        // ���� ���ð� ���� �ٸ� �ൿ�� �õ��ϸ� �ش� �Լ��� ������. (getKey �� true�� ��� ��ȭ ���� �Ⱒ)


        float timer = 4000;
        bool getKey = false;
        isCollecting = true;

        AnnounceKey.SetActive(false);
        UIContainer.SetActive(true);

        ResourceHandler RH = resource.GetComponent<ResourceHandler>();
        int rtype = RH.GetResourceType();
        timer *= (1-GameManager.Instance.AbillityClasses[rtype].collectSpeed);


        //////////////////////////////////
    
        // ������ �ð���ŭ ����ϵ��� ���־����.
        // ���� . FPS�� �޶����� ����ϴ� �ð��� ����

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







        // ���� ä�� �� �ٸ� Ű�� ������ �ʾ� ���������� ä���� �Ϸ�� ���
        // Ȥ�� ä�� �Ϸ� �� ���ҽ��� �����Ǹ� ä���� �ߴܵǾ�� ��.

        if (!getKey)
        {
            // �νĵ� ���ҽ��� ������ ������ ������ ��� ������ ��
            // ��ȭ�� �����Ͽ� GameManager�� ������.
            

            ItemClass result = RH.ResourceCreation(GameManager.Instance.AbillityClasses[rtype]);


            // Ư�� ȹ����� ���� ���ҽ��� �ѹ� �� ȹ���� �� ����.

            int specialProb = (int)(GameManager.Instance.AbillityClasses[rtype].specialGetProb * 100);
            int prob = Random.Range(0, 100);
            if (prob < specialProb)
            {
                result += RH.ResourceCreation(GameManager.Instance.AbillityClasses[rtype]);
            }
            GameManager.Instance.ItemClass[rtype] += result;

            // ���� ���ҽ��� ��Ȱ��ȭ
            resource.gameObject.SetActive(false);
            yield return null;
        }
    }
}
