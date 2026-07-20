using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using StoneSmashGames.Contact.Player;
using StoneSmashGames.Contact.Gameplay;

namespace StoneSmashGames.Contact.Bugs
{
    [CreateAssetMenu(fileName = "New Bug", menuName = "Contact/Bug")]
    public class BugData : ScriptableObject
    {
        public int maxHealth;
        public float speed;

        public bool canStun;
        public float stunTime;

        public float attackRange;
        public float attackChargeTime;
        public float attackTime;
    }

    [RequireComponent(typeof(NavMeshAgent))]
    public class BugController : MonoBehaviour
    {
        public BugData bugData;
        public Transform target;
        public GameObject hitbox;

        public bool isStunned;
        public int health;

        bool isAttacking;
        NavMeshAgent agent;
        GameManager gameManager;

        void Start()
        {
            agent = GetComponent<NavMeshAgent>();
            target = FindObjectOfType<PlayerStats>().transform;
            gameManager = FindObjectOfType<GameManager>();

            InitializeBug();
        }
        
        void Update()
        {
            if (!target) { return; }

            float _distanceToTarget = Vector3.Distance(transform.position, target.position);

            if (_distanceToTarget > bugData.attackRange && !isAttacking && !isStunned)
                agent.SetDestination(target.position);
            else if (_distanceToTarget <= bugData.attackRange && !isAttacking && !isStunned)
                StartCoroutine(Attack());
        }

        void InitializeBug()
        {
            health = bugData.maxHealth;
            agent.speed = bugData.speed;
        }

        IEnumerator Attack()
        {
            isAttacking = true;
            agent.speed = 0;
            yield return new WaitForSeconds(bugData.attackChargeTime);
            hitbox.SetActive(true);
            yield return new WaitForSeconds(bugData.attackTime);
            hitbox.SetActive(false);
            agent.speed = bugData.speed;
            isAttacking = false;
        }

        public void TakeDamage(int _damage)
        {
            health -= _damage;

            if (!isStunned && bugData.canStun)
                StartCoroutine(Stun());

            if (health <= 0)
                Die();
        }

        public IEnumerator Stun()
        {
            isStunned = true;
            agent.speed = 0;
            yield return new WaitForSeconds(bugData.stunTime);
            agent.speed = bugData.speed;
            isStunned = false;
        }

        public void Die()
        {
            gameManager.spawnedBugs--;
            gameManager.remainingBugs--;
            Destroy(gameObject);
        }
    }
}
