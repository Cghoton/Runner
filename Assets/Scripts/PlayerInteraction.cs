using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    [SerializeField]
    private PlayerInputController playerInputController;

    [SerializeField]
    private int playerHealth = 2;

    private bool invinsible = false;

    private void DamageToPlayer()
    {
        if (!invinsible)
        {
            playerHealth--;
            if(playerHealth <= 0)
            {
                LoseRound();
            }
            StartCoroutine(InvinsibilityTurnOn());
            StartCoroutine(RestoreHealth());
            playerInputController.StartCoroutine(playerInputController.SlowDown());
        }
    }

    private IEnumerator InvinsibilityTurnOn(float time = 3)
    {
        invinsible = true;
        yield return new WaitForSeconds(time);
        invinsible = false;
    }

    private IEnumerator RestoreHealth(float time = 8)
    {
        yield return new WaitForSeconds(time);
        playerHealth = 2;
    }

    private void LoseRound()
    {
        playerInputController.StopPlayerFromMoving();
    }

    public void Crash()
    {
        DamageToPlayer();
    }
}
