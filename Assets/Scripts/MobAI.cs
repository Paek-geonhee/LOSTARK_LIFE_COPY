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

        // ���� �ٽ��� Idle, Run �����϶� ���ҽ��� �ƴϸ�,
        // Dead(Wait) �����϶� ���ҽ��� �����ϴ� ����.
        // ���� ���� ������� ��� �ʱ� ���·� ���ư��� ��.
        gameObject.layer = 9;
        curHP = HP;
        transform.position = spawnPosition;
        RH.enabled = false;
        state = State.Idle;
    }

    // Update is called once per frame
    void Update()
    {
        // ���ҽ� ���°� �ƴ� ��� ��� �÷��̾ üũ�ϰ� ���� ��ȭ�� ������.

        CheckPlayer();
        FSM();
    }

    void SetDestinationForNav() {

        // ������Ʈ�� �߽����� ���� ������ ���� ��ġ�� �̵���.

        Vector3 randPos = Random.insideUnitSphere * 15f;
        randPos.y = 0;
        dest = spawnPosition + randPos;
        AG.SetDestination(dest);
    }

    void FSM() {
        if (curHP == 0) state = State.Dead;
        // Idle, Run, Dead, Wait ���¿����� �ൿ�� ������.
        // Dead ������ ���, ���ǹ��� �ݺ� �ڵ� ������ �����ϱ� ���� Dead ������ ������ ������ ��
        // ��� Wait ���·� �Ѿ ���� Ȥ�� ��ȣ�ۿ��� ���� �����.

        if (state == State.Idle)
        {
            // �÷��̾ �νĵ��� ���� �Ϲ����� ����
            // ���� ��ġ���� Patrol ����
            if (Vector3.Distance(transform.position, dest) < 0.2f)
            {
                SetDestinationForNav();
            }
        }

        else if (state == State.Run)
        {
            // �÷��̾ �νĵ� ����.
            // �÷��̾� ��ġ�� �ݴ� �������� ���������� �̵�

            AG.SetDestination(2 * transform.position - player.transform.position);
        }
        else if (state == State.Dead)
        {

            // ���� ����� ����.
            // �ִϸ��̼��� �����Ű�� ���ڸ��� ���ߵ��� ��.
            // ���� ���ҽ� ���·� ��ȯ �� ���

            AM.SetBool("isMove", false);
            AM.SetBool("isDead", true);
            
            AG.SetDestination(transform.position);

            RH.enabled = true;

            state = State.Wait;
            gameObject.layer = 6;
            StartCoroutine(Despawn());
        }
        else {
            // Wait ������ ��쿣 ���ٸ� �ൿ�� ������ ����.
            // �ڷ�ƾ�� ���� ���� �ð� ���� ��Ȱ�� ���·� ��ȯ.
        }
    }

    void CheckPlayer() {

        // Ư�� ���� �� �÷��̾ �ν���
        // Ư�� ���� �� �÷��̾ ���ٸ� Idle ����.
        // Ư�� ���� �� �÷��̾ �ִٸ� Run ����.
        // ���ҽ� ������ ��� �÷��̾� �ν��� �������� ����.

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

        // ���� ���ҽ� ���¿��� �÷��̾���� ��ȣ�ۿ��� ���ٸ� 10�� ����
        // ��Ȱ�� ���·� ��ȭ. ���� GameManager�� ���� ������ ��.

        yield return new WaitForSeconds(10f);
        gameObject.SetActive(false);
    }

    public void GetDamage() {
        curHP--;
    }
}
