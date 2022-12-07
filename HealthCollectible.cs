using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthCollectible : MonoBehaviour
{
    public AudioClip collectedClip;
    Rigidbody2D rb;

    bool hasTarget;
    Vector3 targetPosition;
    float moveSpeed = 5f;

    private void Awake(){
        rb = GetComponent<Rigidbody2D>();
    }

    void OnTriggerEnter2D(Collider2D other){
        RubyController controller = other.GetComponent<RubyController>();

    if (controller != null)
    {
        if (controller.health < controller.maxHealth){
        controller.ChangeHealth(1);
        Destroy(gameObject);

        controller.PlaySound(collectedClip);
        }
    }
    }

    private void FixedUpdate(){
        if(hasTarget == true){
            Vector2 targetDirection = (targetPosition - transform.position).normalized;
            rb.velocity = new Vector2(targetDirection.x, targetDirection.y) * moveSpeed;
        }
    }

    public void SetTarget(Vector3 position){
        targetPosition = position;
     //   hasTarget = true;
    }

   /* public void Testing(bool yes){
        hasTarget = yes;
        Vector3 position = Vector3.zero;
        SetTarget(position);
    }*/
}
