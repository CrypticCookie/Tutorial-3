using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public float speed;
    public bool vertical;
    public float changeTime = 3.0f;
    public ParticleSystem smokeEffect;

    Rigidbody2D rigidbody2D;
    float timer;
    int direction = 1;
    bool broken = true;
    Animator animator;

    private RubyController rubyController;

    // Start is called before the first frame update
    void Start()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();   
        timer = changeTime;
        animator = GetComponent<Animator>(); 

        GameObject rubyControllerObject = GameObject.FindWithTag("RubyController"); //this line of code finds the RubyController script by looking for a "RubyController" tag on Ruby

        if (rubyControllerObject != null)

        {

            rubyController = rubyControllerObject.GetComponent<RubyController>(); //and this line of code finds the rubyController and then stores it in a variable

            print ("Found the RubyController Script!");

        }

        if (rubyController == null)

        {

            print ("Cannot find GameController Script!");

        }
    }

    void Update(){
        timer -= Time.deltaTime;

        if (timer < 0){
            direction = -direction;
            timer = changeTime;
        }
        //remember ! inverse the test, so if broken is true !broken will be false and return won’t be executed.
if(!broken)
{
    return;
}
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector2 position = rigidbody2D.position;

        if (vertical){
            position.y = position.y + Time.deltaTime * speed * direction;;
            animator.SetFloat("Move X", 0);
            animator.SetFloat("Move Y", direction);
        }
        else{
            position.x = position.x + Time.deltaTime * speed * direction;;
            animator.SetFloat("Move X", direction);
            animator.SetFloat("Move Y", 0);
        }

        rigidbody2D.MovePosition(position);
        if(!broken)
  {
      return;
  }
    }

    void OnCollisionEnter2D(Collision2D other)
{
    RubyController player = other.gameObject.GetComponent<RubyController>();

    if (player != null)
    {
        player.ChangeHealth(-1);
    }
}

//Public because we want to call it from elsewhere like the projectile script
public void Fix()
{
    broken = false;
    rigidbody2D.simulated = false;
    smokeEffect.Stop();
    animator.SetInteger("State", 1);
    if (rubyController != null){
        rubyController.RobotNum(1);
    }    
}

}
