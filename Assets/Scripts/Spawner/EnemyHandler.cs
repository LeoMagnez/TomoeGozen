using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EnemyHandler : EntitySpawner
{
        public override void Spawn()
        {
            StartCoroutine(SpawnLightUnits());
            StartCoroutine(SpawnMediumUnits());
        }

        private IEnumerator SpawnLightUnits()
        {
            while (lightUnitsCount < lightUnitsCap)
            {
                xPos = Random.Range(-160, 130);
                zPos = Random.Range(-200, 10);
                Instantiate(lightUnits, new Vector3(xPos, 1, zPos), Quaternion.identity);
                lightUnitsCount += 1;
                totalCount += 1;
            }
            yield return null;
        }

        private IEnumerator SpawnMediumUnits()
        {
            while (mediumUnitsCount < mediumUnitsCap)
            {
                xPos = Random.Range(-160, 130);
                zPos = Random.Range(-200, 10);
                Instantiate(mediumUnits, new Vector3(xPos, 1, zPos), Quaternion.identity);
                totalCount += 1;
                mediumUnitsCount += 1;
            }
            yield return null;
        }
}

