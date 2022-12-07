using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BorgAttack : MonoBehaviour
{
    public float speed;
    public bool vertical;
    public float changeTime = 3.0f;
    public GameObject bullet;
    public Transform bulletPos;
        Rigidbody2D rigidbody2D;
    bool broken = true;
    private float timer;
    private GameObject Ruby;
    int direction = 1;
    //bool broken = true;
    Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>(); 
        rigidbody2D = GetComponent<Rigidbody2D>();
        Ruby = GameObject.FindGameObjectWithTag("RubyController");
        animator.SetInteger("State", 3);
    }

    // Update is called once per frame
    void Update()
    {

         
    

        float distance = Vector2.Distance(transform.position, Ruby.transform.position);
        Debug.Log(distance);

        if(distance < 4){
            
            timer += Time.deltaTime;

            if (timer > 2 && broken == true){
            timer = 0;
            shoot();
        }
        if (broken == false){
            animator.SetInteger("State", 1);
        }
        if (timer < 0){
            direction = -direction;
            timer = changeTime;
        }
        
    }
    //else{
      //      animator.SetInteger("State", 3);
        //}
        if(!broken)
{
    return;
     animator.SetInteger("State", 1);
}
}

void FixedUpdate()
    {
        Vector2 position = rigidbody2D.position;

        /*if (vertical){
            position.y = position.y + Time.deltaTime * speed * direction;;
            animator.SetFloat("Move X", 0);
            animator.SetFloat("Move Y", direction);
        }
        else{
            position.x = position.x + Time.deltaTime * speed * direction;;
            animator.SetFloat("Move X", direction);
            animator.SetFloat("Move Y", 0);
        }*/
    //animator.SetInteger("State", 3);
        rigidbody2D.MovePosition(position);
        if(!broken)
  {
      return;
       animator.SetInteger("State", 1);
  }
    }

    void shoot(){
        Instantiate(bullet, bulletPos.position, Quaternion.identity);
        animator.SetInteger("State", 2);
    }

void OnCollisionEnter2D(Collision2D other)
{
    RubyController player = other.gameObject.GetComponent<RubyController>();

    if (player != null)
    {
        player.ChangeHealth(-1);
    }
}

    public void Destroy()
{
    broken = false;
    rigidbody2D.simulated = false;
    animator.SetInteger("State", 1);
}
}
