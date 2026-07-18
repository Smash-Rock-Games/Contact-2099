using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StoneSmashGames.Contact.Player
{
    public enum WeaponType
    {
        Semi,
        Auto,
        Burst
    }

    [CreateAssetMenu(fileName = "New Weapon", menuName = "Contact/Weapon")]
    public class WeaponData : ScriptableObject
    {
        public Sprite crosshair;
        public WeaponType weaponType;

        public float fireRate;
        public float range;
        public float spread;

        public float reloadTime;
        public int numberOfShots;
        public int magazineSize;

        public int burstAmount;
        public float timeBetweenBursts;
    }

    public class PlayerWeapon : MonoBehaviour
    {
        public Transform playerCamera;
        public Transform firePoint;
        public WeaponData weapon;
        public PlayerHUD playerHUD;
        public PlayerStats playerStats;

        public int ammoInMagazine;
        public int storedAmmo;

        bool isBurstFiring;
        bool isReloading;
        float nextTimeToFire;
        Vector3 aimTarget;

        void Start()
        {

        }

        void Update()
        {
            if (!weapon) { return; }

            playerHUD.UpdateWeaponUI(this);

            SetAimTarget();

            if (!isReloading && playerStats.canShoot)
            {
                if (weapon.weaponType == WeaponType.Auto && Time.time >= nextTimeToFire)
                    if (Input.GetMouseButton(0))
                        InitializeShooting();

                if (weapon.weaponType != WeaponType.Auto && Time.time >= nextTimeToFire)
                    if (Input.GetMouseButtonDown(0))
                        InitializeShooting();

                if (Input.GetKeyDown(KeyCode.R) && storedAmmo > 0 && ammoInMagazine < weapon.magazineSize)
                    StartCoroutine(Reload());
            }
        }

        void SetAimTarget()
        {
            RaycastHit _hit;

            if (Physics.Raycast(playerCamera.position, playerCamera.forward, out _hit, weapon.range))
            {
                aimTarget = _hit.point;
            }
            else
            {
                aimTarget = playerCamera.position + playerCamera.forward * weapon.range;
            }
        }

        void InitializeShooting()
        {
            nextTimeToFire = Time.time + 1 / weapon.fireRate;

            if (weapon.weaponType == WeaponType.Burst && !isBurstFiring)
                StartCoroutine(BurstShoot());
            else
                Shoot();
        }

        IEnumerator BurstShoot()
        {
            isBurstFiring = true;

            for (int i = 0; i < weapon.burstAmount; i++)
            {
                Shoot();
                yield return new WaitForSeconds(weapon.timeBetweenBursts);
            }

            isBurstFiring = false;
        }

        void Shoot()
        {
            if(ammoInMagazine > 0)
            {
                ammoInMagazine--;

                for (int i = 0; i < weapon.numberOfShots; i++)
                {
                    RaycastHit hit;

                    firePoint.LookAt(aimTarget);
                    firePoint.localRotation *= Quaternion.Euler(
                        Random.Range(-weapon.spread, weapon.spread),
                        Random.Range(-weapon.spread, weapon.spread),
                        Random.Range(-weapon.spread, weapon.spread));

                    if (Physics.Raycast(firePoint.position, firePoint.forward, out hit, weapon.range))

                    {
                        Debug.DrawRay(firePoint.position, firePoint.forward * hit.distance, Color.yellow, 1f);
                        Debug.Log("Did Hit");
                    }
                    else
                    {
                        Debug.DrawRay(firePoint.position, firePoint.forward * weapon.range, Color.white, 1f);
                        Debug.Log("Did not Hit");
                    }

                    firePoint.localRotation = Quaternion.identity;
                }
            }
        }

        IEnumerator Reload()
        {
            isReloading = true;

            yield return new WaitForSeconds(weapon.reloadTime);

            int _neededAmmo = weapon.magazineSize - ammoInMagazine;
            int _ammoToLoad = Mathf.Min(_neededAmmo, storedAmmo);

            ammoInMagazine += _ammoToLoad;
            storedAmmo -= _ammoToLoad;

            isReloading = false;
        }
    }
}
