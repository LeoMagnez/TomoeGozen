using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntitySpawner : MonoBehaviour
{
      EnemyHandler enemies;
      AllyHandler allies;


    [HideInInspector]
    public int xPos;
    [HideInInspector]
    public int zPos;

    [Header("Light Units")]
    public int lightUnitsCount;
    public int lightUnitsCap;
    public GameObject lightUnits;

    [Header("Medium Units")]
    public int mediumUnitsCount;
    public int mediumUnitsCap;
    public GameObject mediumUnits;

    [Header("Total")]
    public int totalCount;

    // Start is called before the first frame update
    void Start()
    {
        enemies = gameObject.GetComponent<EnemyHandler>();
        allies = gameObject.GetComponent<AllyHandler>();
    }

    // Update is called once per frame
    void Update()
    {
        enemies.Spawn();
        allies.Spawn();
    }


    public virtual void Spawn()
    {

    }

    public class Allies : EntitySpawner
    {
        public override void Spawn()
        {
            //base.Spawn();
        }
    }

}


