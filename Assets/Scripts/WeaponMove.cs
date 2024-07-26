using UnityEngine;

public class WeaponMove : MonoBehaviour
{
    private Rigidbody RG;

    public Vector3 dest;
    public float speed;

    // Start is called once before the first execution of Update after the MonoBehaviour is created

    private void Start()
    {
        RG = GetComponent<Rigidbody>();
        gameObject.SetActive(false);
    }

    public void SetVelocity() {
        RG.linearVelocity = (dest - transform.position).normalized * speed;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == 8) {

            var colliders = Physics.OverlapSphere(transform.position, 5f, (1 << 9));

            if (colliders.Length > 0) {
                for (int i = 0; i < colliders.Length; i++)
                {
                    if (colliders[i].tag == "Rabbit") {
                        colliders[i].GetComponent<MobAI>().GetDamage();
                    }
                }
            }
        }

        gameObject.SetActive(false);
    }
}
