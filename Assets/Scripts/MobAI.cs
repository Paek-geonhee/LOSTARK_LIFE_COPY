using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class MobAI : MonoBehaviour
{
    public enum State { 
        Idle,
        Run,
        Dead,
        Wait
    }

    public State state;
    public Vector3 spawnPosition;
    public float DetectionRange;
    public int HP;


    public Vector3 dest;
    public GameObject player;

    private int curHP;
    private NavMeshAgent AG;
    private Animator AM;
    private ResourceHandler RH;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        AG = GetComponent<NavMeshAgent>();
        AM = GetComponent<Animator>();
        RH = GetComponent<ResourceHandler>();

        RH.enabled = false;
        SetDestinationForNav();
        spawnPosition = transform.position;

        AM.SetBool("isMove", true);

        curHP = HP;
       
    }

    private void OnDisable()
    {

        // 몹의 핵심은 Idle, Run 상태일땐 리소스가 아니며,
        // Dead(Wait) 상태일땐 리소스로 동작하는 것임.
        // 디스폰 이후 재생성될 경우 초기 상태로 돌아가야 함.
        gameObject.layer = 9;
        curHP = HP;
        transform.position = spawnPosition;
        RH.enabled = false;
        state = State.Idle;
    }

    // Update is called once per frame
    void Update()
    {
        // 리소스 상태가 아닐 경우 상시 플레이어를 체크하고 상태 변화를 수행함.

        CheckPlayer();
        FSM();
    }

    void SetDestinationForNav() {

        // 오브젝트를 중심으로 원형 단위의 랜덤 위치로 이동함.

        Vector3 randPos = Random.insideUnitSphere * 15f;
        randPos.y = 0;
        dest = spawnPosition + randPos;
        AG.SetDestination(dest);
    }

    void FSM() {
        if (curHP == 0) state = State.Dead;
        // Idle, Run, Dead, Wait 상태에서의 행동을 정의함.
        // Dead 상태인 경우, 무의미한 반복 코드 수행을 방지하기 위해 Dead 상태의 동작을 수행한 후
        // 즉시 Wait 상태로 넘어가 디스폰 혹은 상호작용을 위해 대기함.

        if (state == State.Idle)
        {
            // 플레이어가 인식되지 않은 일반적인 상태
            // 스폰 위치에서 Patrol 수행
            if (Vector3.Distance(transform.position, dest) < 0.2f)
            {
                SetDestinationForNav();
            }
        }

        else if (state == State.Run)
        {
            // 플레이어가 인식된 상태.
            // 플레이어 위치의 반대 방향으로 지속적으로 이동

            AG.SetDestination(2 * transform.position - player.transform.position);
        }
        else if (state == State.Dead)
        {

            // 몹이 사망한 상태.
            // 애니메이션을 변경시키고 제자리에 멈추도록 함.
            // 이후 리소스 상태로 전환 후 대기

            AM.SetBool("isMove", false);
            AM.SetBool("isDead", true);
            
            AG.SetDestination(transform.position);

            RH.enabled = true;

            state = State.Wait;
            gameObject.layer = 6;
            StartCoroutine(Despawn());
        }
        else {
            // Wait 상태일 경우엔 별다른 행동을 취하지 않음.
            // 코루틴에 의해 일정 시간 이후 비활성 상태로 전환.
        }
    }

    void CheckPlayer() {

        // 특정 범위 내 플레이어를 인식함
        // 특정 범위 내 플레이어가 없다면 Idle 상태.
        // 특정 범위 내 플레이어가 있다면 Run 상태.
        // 리소스 상태인 경우 플레이어 인식을 수행하지 않음.

        if (state != State.Wait && state != State.Dead) {
            var colliders = Physics.OverlapSphere(transform.position, DetectionRange, (1 << 3));

            if (colliders.Length != 0)
            {
                player = colliders[0].gameObject;
            }
            else
            {
                player = null;
            }

            if (player != null)
            {
                state = State.Run;
            }
            else
            {
                state = State.Idle;
                SetDestinationForNav();
            }
        }
    }

    IEnumerator Despawn() {

        // 만약 리소스 상태에서 플레이어와의 상호작용이 없다면 10초 이후
        // 비활성 상태로 변화. 이후 GameManager에 의해 리스폰 됨.

        yield return new WaitForSeconds(10f);
        gameObject.SetActive(false);
    }

    public void GetDamage() {
        curHP--;
    }
}
