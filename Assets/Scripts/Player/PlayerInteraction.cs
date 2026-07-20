using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StoneSmashGames.Contact.Gameplay;

namespace StoneSmashGames.Contact.Player
{
    [RequireComponent(typeof(PlayerHUD))]
    public class PlayerInteraction : MonoBehaviour
    {
        public float interactionRange;
        public Transform playerCamera;
        PlayerHUD playerHUD;

        void Start()
        {
            playerHUD = GetComponent<PlayerHUD>();
        }


        void Update()
        {
            RaycastHit _hit;

            if (Physics.Raycast(playerCamera.position, playerCamera.forward, out _hit, interactionRange))
            {
                if (_hit.collider.TryGetComponent(out DoorController _door))
                {
                    playerHUD.interactionPrompt.text = "[E] Open Door";

                    if (Input.GetKeyDown(KeyCode.E))
                        _door.OpenDoor();
                }
                else
                {
                    playerHUD.interactionPrompt.text = "";
                }
            }
            else
            {
                playerHUD.interactionPrompt.text = "";
            }
        }
    }
}
