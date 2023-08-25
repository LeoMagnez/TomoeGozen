using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackHandler : MonoBehaviour
{
    [Header("Attack Power")]
    public int damage;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }



    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.TryGetComponent<EnemiesHP>(out EnemiesHP enemyComponent))
        {
            Debug.Log(enemyComponent);
            enemyComponent.TakeDamage(damage);
        }
    }
}
