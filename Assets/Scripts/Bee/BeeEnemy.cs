using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeeEnemy : MonoBehaviour
{

    public Animator animator;
    private Rigidbody2D rb;
    public SpriteRenderer spriteRenderer;

    public float speed = 0.5f;
    private float rotateSpeed = 150f;
    private float waitTime;
    public float startWaitTime = 2f;
    public float waitTimeFirstMeleeAttack = 2f;
    private int i = 0;
    public Transform[] moveSpots;
    public GameObject meleeAttackSpot;

    private float cooldownBulletAttack = 1.5f;
    private float actualCooldownBulletAttack;
    private float bulletsShot;
    private float maxBulletshoots = 2f;

    private float cooldownMeleeAttack = 4f;
    private float actualCooldownMeleeAttack;
    private float meleeAttackTime;
    private GameObject target;

    public GameObject spawnPoint;
    public GameObject beeBullet;

    // Start is called before the first frame update
    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player");
        rb = GetComponent<Rigidbody2D>();
        waitTime = startWaitTime;
        actualCooldownBulletAttack = 0;
        actualCooldownMeleeAttack = 0;
        bulletsShot = 0;
        meleeAttackTime = 4f;
    }

    // Update is called once per frame
    private void Update()
    {
        actualCooldownMeleeAttack -= Time.deltaTime;
        actualCooldownBulletAttack -= Time.deltaTime;
    }
    void FixedUpdate()
    { 
        if (bulletsShot < maxBulletshoots) //Si la abeja no ha disparado el máximo de aguijones, mantiene la patrulla mientras comprueba que el jugador pase por debajo suya
        {
            Patrol();
            RaycastHit2D hit2D = Physics2D.Raycast(spawnPoint.transform.position, Vector2.down);
            if (hit2D.collider != null)
            {
                if (hit2D.collider.CompareTag("Player"))
                {
                    if (actualCooldownBulletAttack < 0)
                    {
                        ShootHomingBullet();
                    }
                }
            }
        }
        else //Una vez disparado el máximo de aguijones entra en el bucle de atacar al personaje
        {
            waitTimeFirstMeleeAttack -= Time.deltaTime;
            meleeAttackTime -= Time.deltaTime;
            //Acabada la espera, la abeja checkea si tiene el ataque en cooldown
            if (waitTime < 0)
            {
                //Si el ataque está en cooldown va a la posicion de espera
                if (waitTimeFirstMeleeAttack < 0 && actualCooldownMeleeAttack > 0)
                {
                    ReturnToMeleeAttackSpot();
                }
                //Si puede atacar, persigue al jugador durante meleeAttackTime e intenta picarle
                else if (actualCooldownMeleeAttack < 0 && waitTimeFirstMeleeAttack < 0)
                {
                    if (meleeAttackTime > 0)
                    {
                        Chase();
                        if (Vector2.Distance(transform.position, target.transform.position) < 0.3f)
                        {
                            AttackMelee();
                        }
                    }
                    else //Acabado el tiempo de ataque vuelve a la espera
                    {
                        waitTime = startWaitTime;
                    }
                }
            }
            else //Si la abeja acaba de atacar --> se queda quieta un waitTime(cansada) para que el jugador tenga oportunidad de matarla
            {
                Tired();
            }
        }
    }

    void Patrol()
    {
        transform.position = Vector2.MoveTowards(transform.position, moveSpots[i].transform.position, speed * Time.deltaTime);
        if (Vector2.Distance(transform.position, moveSpots[i].transform.position) < 0.1f)
        {
            if (waitTime <= 0)
            {
                if (moveSpots[i] != moveSpots[moveSpots.Length - 1])
                {
                    i++;
                }
                else
                {
                    i = 0;
                }
                waitTime = startWaitTime;
            }
            else
            {
                waitTime -= Time.deltaTime;
            }
        }
    }

    //
    void ShootHomingBullet()
    {
        Invoke("LaunchBullet", 0.5f);
        animator.Play("Attack");
        actualCooldownBulletAttack = cooldownBulletAttack;
        bulletsShot++;
    }

    void AttackMelee()
    {
        animator.Play("Attack");
    }

    void ReturnToMeleeAttackSpot()
    {
        if (transform.position == meleeAttackSpot.transform.position)
        {
            rb.angularVelocity = 0;
            rb.velocity = transform.up * 0;
        }
        else
        {
            Vector2 direction = (Vector2)transform.position - (Vector2)meleeAttackSpot.transform.position;
            direction.Normalize();
            float rotateAmount = Vector3.Cross(direction, transform.up).z;
            if (rotateAmount > 0)
            {
                rb.angularVelocity = rotateSpeed;
            }
            else if (rotateAmount < 0)
            {
                rb.angularVelocity = -rotateSpeed;
            }
            else
            {
                rotateSpeed = 0;
            }
            rb.velocity = transform.up * speed;
            meleeAttackTime = 5f;
        }
    }

    void Chase()
    {
        Vector2 direction = (Vector2)transform.position - (Vector2)target.transform.position;
        direction.Normalize();
        float rotateAmount = Vector3.Cross(direction, -transform.up).z;
        if (rotateAmount > 0)
        {
            rb.angularVelocity = rotateSpeed;
        }
        else if (rotateAmount < 0)
        {
            rb.angularVelocity = -rotateSpeed;
        }
        else
        {
            rotateSpeed = 0;
        }
        rb.velocity = -transform.up * speed;
    }

    void Tired()
    {
        transform.rotation = Quaternion.Euler(0, 0, 0);
        rb.velocity = transform.up * 0;
        actualCooldownMeleeAttack = cooldownMeleeAttack;
        waitTime -= Time.deltaTime;
    }

    void LaunchBullet()
    {
        GameObject newBullet;
        newBullet = Instantiate(beeBullet, spawnPoint.transform.position, spawnPoint.transform.rotation);
    }
}
