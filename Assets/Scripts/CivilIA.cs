using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Random = UnityEngine.Random;

namespace Civil
{
    public class CivilIA : MonoBehaviour
    {

        public GameObject bloodParticle;
        public UICounter counter;

        Rigidbody2D rb;

        [SerializeField] float speed = 7;
        [SerializeField] float maxTimeDestroy = 7;/*if stuck somewhere, destroy*/
        float currentTimeDes;

        [HideInInspector] public Vector3 destination;
        [HideInInspector] public bool isWaiting;

        private void Start()
        {
            rb = GetComponent<Rigidbody2D>();
            WaitXSeconds(ChooseNewLocation);
            currentTimeDes = maxTimeDestroy;
        }

        private void Update()
        {//if it is to close, search new location
            if (Vector2.Distance(transform.position, destination) <= 1)
            {
                WaitXSeconds(ChooseNewLocation);

            }
        }

        private void FixedUpdate()
        {
            if (!isWaiting && currentTimeDes > 0)
            {//if isnt in cooldown, move towards
                rb.MovePosition(transform.position + transform.right * speed * Time.fixedDeltaTime);
            }
        }

        void ChooseNewLocation()
        {//random coords
            destination = transform.position + (Vector3)Random.insideUnitCircle * Random.Range(20, 151);
        }

        void WaitXSeconds(Action onwaited, float random = -1)
        {
            if (isWaiting)
                return;
            StartCoroutine(_WaitXSeconds(onwaited, random));
        }

        IEnumerator _WaitXSeconds(Action onwaited, float random = -1)
        {//wait seconds and restart functions

            if (isWaiting)
                yield break;

            isWaiting = true;

            yield return new WaitForSeconds(random <= -1 ? Random.Range(1f, 2f) : random);

            if (!isWaiting)
                yield break;

            onwaited?.Invoke();

            Vector3 dir = (destination - transform.position).normalized;
            transform.rotation = Quaternion.Euler(dir);

            isWaiting = false;
        }
        void OnAvoidObstacle()
        {
            destination = transform.position+(-transform.right).normalized *150;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.tag == "Player1"|| other.tag =="Player2")
            {
                Instantiate(bloodParticle, transform.position, transform.rotation);
                //counter.SubstractSecs(5);
                CivilSpawner.civilSpawner.spawn--;
                CivilSpawner.civilSpawner.peopleDied++;
                Destroy(this.gameObject);
            }
            else if ((other.tag == "Wall")&&!isWaiting)
            {
                WaitXSeconds(OnAvoidObstacle);
                currentTimeDes = maxTimeDestroy;
            }
        }
        private void OnTriggerStay2D(Collider2D other)
        {
            if((other.tag == "Wall"))
            {/*in case it get stuck, destroy*/
                currentTimeDes -= Time.deltaTime;
                if (currentTimeDes <= 0)
                {
                    Destroy(this.gameObject);
                    CivilSpawner.civilSpawner.spawn--;
                }
            }
        }
    }
}
