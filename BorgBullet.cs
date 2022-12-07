using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BorgBullet : MonoBehaviour
{
    public GameObject Ruby;
    private Rigidbody2D rb;
    public float force;
    private float bulletTimer;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        Ruby = GameObject.FindGameObjectWithTag("RubyController");

        Vector3 direction = Ruby.transform.position - transform.position;
        rb.velocity = new Vector2(direction.x, direction.y).normalized * force;

        float rot = Mathf.Atan2(-direction.y, -direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, rot + 90);
    }

    // Update is called once per frame
    void Update()
    {
        if(transform.position.magnitude > 1000.0f){
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D other){


        if (other.gameObject.CompareTag("RubyController")){
            RubyController player = other.gameObject.GetComponent<RubyController>();
            player.ChangeHealth(-2);
            Destroy(gameObject);
        }
    }
}
