using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace StoneSmashGames.Contact.Player
{
    [RequireComponent(typeof(PlayerController))]
    public class PlayerStats : MonoBehaviour
    {
        public int maxHealth = 100;
        public int health = 100;
        public int scrap = 0;
        public bool isDead;

        public Image healthOverlay;

        public List<PlayerWeapon> weapons;
        public bool canShoot = true;

        PlayerController playerController;

        void Start()
        {
            playerController = GetComponent<PlayerController>();
        }

        
        void Update()
        {
            float _healthPercent = (float)health / (float)maxHealth;
            float _overlayAlpha = (float)(1f - _healthPercent) * 0.8f;

            Color _overlayColor = healthOverlay.color;
            _overlayColor.a = _overlayAlpha;

            healthOverlay.color = _overlayColor;

            if(isDead)
                playerController.cameraHandle.position = Vector3.Lerp(
                    playerController.cameraHandle.position, 
                    playerController.transform.position + Vector3.up * 0.5f, 
                    Time.deltaTime * 5);
        }

        public void TakeDamage(int _damage)
        {
            health -= _damage;

            if (health <= 0 && !isDead)
                StartCoroutine(Die());
        }

        public IEnumerator Die()
        {
            playerController.canMove = false;
            playerController.canLook = false;
            isDead = true;
            canShoot = false;

            yield return new WaitForSeconds(5);
        }
    }
}
