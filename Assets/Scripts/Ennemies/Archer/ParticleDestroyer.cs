using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleDestroyer : MonoBehaviour
{
    private float elapsedTime;
    // Start is called before the first frame update
    void Start()
    {
        elapsedTime = 5f;
    }

    // Update is called once per frame
    void Update()
    {
        if (elapsedTime > 0)
        {
            elapsedTime -= Time.deltaTime;
        }
        else
        {
            Destroy(gameObject);
        } 
    }
}
