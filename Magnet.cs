using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Magnet : MonoBehaviour
{
    private void OnTriggerStay2D(Collider2D collision){
        if(collision.gameObject.TryGetComponent<HealthCollectible>(out HealthCollectible HealthCollectible)){
            HealthCollectible.SetTarget(transform.parent.position);
        }
    }
}
