using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RinoEnemy : MonoBehaviour
{

    public Animator animator;

    public SpriteRenderer rinoSpriteRenderer;

    public float calmSpeed = 0.5f;
    public float rageSpeed = 1f;
    private float waitTime;
    public float startWaitTime = 2;
    private int i = 0;
    private Vector2 actualPos;
    public Transform[] moveSpots;

    public GameObject target;
    public bool playerChased;
    public bool secondCharge;
    public float lookingForTime;
    private float tiredTime;

    private string colorAttack = "#BF5252";
    private string normalColor = "#FFFFFF";
    private string lookingForPlayerColor = "#51DED4";
    private string tiredColor = "#71E785";
    private Color newCol;

    public Transform rightVision;
    public Transform leftVision;
    public Vector2 visionPosition;

    // Start is called before the first frame update
    void Start()
    {
        waitTime = startWaitTime;
        tiredTime = 0;
        playerChased = false;
        secondCharge = false;
        lookingForTime = 2f;
}

    // Update is called once per frame
    void FixedUpdate()
    {
        StartCoroutine(CheckEnemyMoving()); //Corutina para girar el spriteRenderer y hacer las transiciones entre animaciones
        //Patrullar --> modo por defecto
        Patrol();
        //Comprobamos los raycast usados como vision del rinoceronte
        if (rinoSpriteRenderer.flipX == true)
        {
            RaycastHit2D rightHit2D = Physics2D.Raycast(rightVision.position, rightVision.right);
            if (rightHit2D.collider != null)
            {
                if (rightHit2D.collider.CompareTag("Player") && rightHit2D.distance < 0.8f)
                {
                    //Si el rinoceronte ve al jugador y el jugador esta cerca, hace la embestida
                    ChargeAttack();
                }
                else
                {
                    if (playerChased && secondCharge == false)
                    {
                        Tired();
                    }
                    else if (playerChased && secondCharge)
                    {
                        LookingForPlayer();
                    }
                }
            }
        }
        else if (rinoSpriteRenderer.flipX == false)
        {
            RaycastHit2D leftHit2D = Physics2D.Raycast(leftVision.position, -leftVision.right);
            if (leftHit2D.collider != null)
            {
                if (leftHit2D.collider.CompareTag("Player") && leftHit2D.distance < 0.8f)
                {
                    ChargeAttack();
                }
                else
                {
                    if (playerChased && secondCharge == false)
                    {
                        Tired();
                    }
                    else if(playerChased && secondCharge)
                    {
                        LookingForPlayer();
                    }
                }
            }
        }
    }


    void Patrol()
    {
        if (tiredTime <= 0 && playerChased == false)
        {
            lookingForTime = 2f;
            ColorUtility.TryParseHtmlString(normalColor, out newCol);
            rinoSpriteRenderer.color = newCol;
            Debug.Log("Patrullar");
            transform.position = Vector2.MoveTowards(transform.position, moveSpots[i].transform.position, calmSpeed * Time.deltaTime);
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
        else
        {
            tiredTime -= Time.deltaTime;
        }
    }

    void ChargeAttack()
    {
        if (tiredTime <= 0)
        {
            Debug.Log("Embestir");
            calmSpeed = rageSpeed;
            transform.position = Vector2.MoveTowards(transform.position, target.transform.position, rageSpeed * Time.deltaTime);
            ColorUtility.TryParseHtmlString(colorAttack, out newCol);
            rinoSpriteRenderer.color = newCol;
            playerChased = true;
            secondCharge = true;
        }
    }

    void LookingForPlayer()
    {
        Debug.Log("Buscando");
        ColorUtility.TryParseHtmlString(lookingForPlayerColor, out newCol);
        rinoSpriteRenderer.color = newCol;
        calmSpeed = 0.5f;
        if(lookingForTime < 0)
        {
            playerChased = true;
            secondCharge = false;
        }
        else
        {
            lookingForTime -= Time.deltaTime;
        }
    }

    void Tired()
    {
        Debug.Log("Cansado");
        ColorUtility.TryParseHtmlString(tiredColor, out newCol);
        rinoSpriteRenderer.color = newCol;
        calmSpeed = 0.5f;
        playerChased = false;
        tiredTime  = 2f;
    }

    IEnumerator CheckEnemyMoving()
    {
        actualPos = transform.position;
        yield return new WaitForSeconds(0.5f);
        if (transform.position.x > actualPos.x)
        {
            rinoSpriteRenderer.flipX = true;
            animator.SetBool("Idle", false);
        }
        else if (transform.position.x < actualPos.x)
        {
            rinoSpriteRenderer.flipX = false;
            animator.SetBool("Idle", false);
        }
        else if (transform.position.x == actualPos.x)
        {
            animator.SetBool("Idle", true);
        }
    }
}
