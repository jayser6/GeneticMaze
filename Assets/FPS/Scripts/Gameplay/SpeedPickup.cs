using UnityEngine;
using Unity.FPS.Game;
using System;
using System.Collections;

namespace Unity.FPS.Gameplay
{
    public class SpeedPickup : Pickup
    {
        protected override void OnPicked(PlayerCharacterController player)
        {
            StartCoroutine(waiter(player));
        }

        IEnumerator waiter(PlayerCharacterController player)
        {
            player.GetComponent<PlayerCharacterController>().MaxSpeedOnGround = 15f;
            player.GetComponent<PlayerCharacterController>().JumpForce = 13f;
            player.GetComponent<PlayerCharacterController>().MaxSpeedInAir = 15f;

            Debug.Log("before wait");

            PlayPickupFeedback();

            yield return new WaitForSecondsRealtime(5);

            player.GetComponent<PlayerCharacterController>().MaxSpeedOnGround = 10f;
            player.GetComponent<PlayerCharacterController>().JumpForce = 9f;
            player.GetComponent<PlayerCharacterController>().MaxSpeedInAir = 10f;

            Debug.Log("after wait");

            //PlayPickupFeedback();
            Destroy(gameObject);
        }
    }
}