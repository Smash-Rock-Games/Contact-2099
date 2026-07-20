using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace StoneSmashGames.Contact.Player
{
    public class PlayerHUD : MonoBehaviour
    {
        public TMP_Text interactionPrompt;

        public Image crosshair;
        public TMP_Text weaponName;
        public TMP_Text magazineAmmo;
        public TMP_Text storedAmmo;

        void Start()
        {

        }


        void Update()
        {

        }

        public void UpdateWeaponUI(PlayerWeapon _playerWeapon)
        {
            if (crosshair.sprite != _playerWeapon.weapon.crosshair)
                crosshair.sprite = _playerWeapon.weapon.crosshair;

            weaponName.text = _playerWeapon.weapon.name;
            magazineAmmo.text = _playerWeapon.ammoInMagazine + "/" + _playerWeapon.weapon.magazineSize;
            storedAmmo.text = _playerWeapon.storedAmmo + "";
        }
    }
}
