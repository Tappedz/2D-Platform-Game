using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeeBullet : MonoBehaviour
{

    private float speed = 1.2f;
    private float rotateSpeed = 100f;
    private float lifeTime = 2f;
    
    private GameObject target;
    private Rigidbody2D rb;
    // Start is called before the first frame update
    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player");
        rb = GetComponent<Rigidbody2D>();
        Destroy(gameObject,lifeTime);

    }

    // Update is called once per frame
    void Update()
    {
        //transform.Translate(Vector2.down * speed * Time.deltaTime);
    }

    private void FixedUpdate()
    {
        Vector2 direction = (Vector2)transform.position - (Vector2)target.transform.position;
        direction.Normalize();
        float rotateAmount = Vector3.Cross(direction, -transform.up).z;
        if(rotateAmount > 0)
        {
            rb.angularVelocity = rotateSpeed;
        }
        else if(rotateAmount < 0){
            rb.angularVelocity = -rotateSpeed;
        }
        else
        {
            rotateSpeed = 0;
        }
        rb.velocity = -transform.up * speed;
    }
}
