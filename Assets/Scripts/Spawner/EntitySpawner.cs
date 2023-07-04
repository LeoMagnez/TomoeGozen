using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class EntitySpawner : MonoBehaviour
{

    public enum Faction
    {
        Ally,
        Enemy,
    }

    //à retirer à terme
    public bool isAlly, isEnemy;

    [Range(0f, 100f)]
    public float radius;

    [Header("Light Units")]
    public int lightUnitsCount;
    public int lightUnitsCap;

    //à simplifier à terme
    public GameObject allyLightUnits;
    public GameObject enemyLightUnits;

    [Header("Medium Units")]
    public int mediumUnitsCount;
    public int mediumUnitsCap;

    //à simplifier à terme
    public GameObject allyMediumUnits;
    public GameObject enemyMediumUnits;

    [Header("Total")]
    public int totalCount;


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        StartCoroutine(SpawnLightUnits());
        StartCoroutine(SpawnMediumUnits());
    }


    public void Spawn()
    {

    }

    private IEnumerator SpawnLightUnits()
    {
        while (lightUnitsCount < lightUnitsCap)
        {
            //à simplifier
            if (isAlly)
            {
                Instantiate(allyLightUnits, GetSpawningPos(), Quaternion.identity);
            }

            //à simplifier
            if (isEnemy)
            {
                Instantiate(enemyLightUnits, GetSpawningPos(), Quaternion.identity);
            }

            lightUnitsCount += 1;
            totalCount += 1;
        }
        yield return null;
    }

    private IEnumerator SpawnMediumUnits()
    {
        while (mediumUnitsCount < mediumUnitsCap)
        {
            //à simplifier
            if (isAlly)
            {
                Instantiate(allyMediumUnits, GetSpawningPos(), Quaternion.identity);
            }

            //à simplifier
            if (isEnemy)
            {
                Instantiate(enemyMediumUnits, GetSpawningPos(), Quaternion.identity);
            }

            totalCount += 1;
            mediumUnitsCount += 1;
        }
        yield return null;
    }

    public void OnDrawGizmosSelected()
    {
        //à simplifier
        if (isEnemy)
        {
            Handles.color = Color.red;
        }

        //à simplifier
        if (isAlly)
        {
            Handles.color = Color.cyan;
        }

        Handles.DrawWireDisc(transform.position, transform.up, radius);
    }

    protected Vector3 GetSpawningPos()
    {
        Vector3 spawnVector = transform.forward * Random.Range(0.5f, radius);
        spawnVector = Quaternion.Euler(0, Random.Range(0, 360), 0) * spawnVector;

        return transform.position + spawnVector;
    }

}


