using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Civil
{
    public class CivilSpawner : MonoBehaviour
    {
        public GameObject civil;
        public UICounter counter;

        public static CivilSpawner civilSpawner;

        public float spawn;
        public float peopleDied;

        public float spawnCooldown;

        float cooldown;
        Transform[] spawnsPositions;

        private void Awake()
        {//Saves all the positions where civilians can spawn

            if (civilSpawner == null)
                civilSpawner = this;
            else
                Destroy(this.gameObject);

            spawnsPositions = new Transform[transform.childCount];

            for (int i = 0; i < transform.childCount; i++)
            {
                spawnsPositions[i] = this.transform.GetChild(i);
            }
        }

        void Update()
        {
            if (cooldown <= 0&&spawn<=120)
            {
                cooldown = spawnCooldown;
                InstantiateNewCivilian();
                spawn++;
            }
            else
                cooldown -= Time.deltaTime;
        }

        void InstantiateNewCivilian()
        {
            //choose a spawnpoint
            Vector3 spawnPosition = spawnsPositions[Random.Range(0, this.transform.childCount)].position;

            var _civil = Instantiate<GameObject>(civil, spawnPosition, Quaternion.Euler(Vector3.zero));
            _civil.GetComponent<CivilIA>().counter = counter;
        }
    }
}
