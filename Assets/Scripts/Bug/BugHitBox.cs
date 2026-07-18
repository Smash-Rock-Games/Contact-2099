using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StoneSmashGames.Contact.Player;

namespace StoneSmashGames.Contact.Bugs
{
    public class BugHitBox : MonoBehaviour
    {
        public int damage;

        private void OnTriggerEnter(Collider _other)
        {
            if (_other.TryGetComponent(out PlayerStats _player))
                _player.TakeDamage(damage);
        }
    }
}
