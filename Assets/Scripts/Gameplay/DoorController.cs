using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StoneSmashGames.Contact.Gameplay
{
    public class DoorController : MonoBehaviour
    {
        public int locationIndex;
        public BoxCollider collider;

        GameManager gameManager;

        void Start()
        {
            gameManager = FindObjectOfType<GameManager>();
        }

        public void OpenDoor()
        {
            collider.enabled = false;
            gameManager.AddLocationSpawns(locationIndex);
        }
    }
}
