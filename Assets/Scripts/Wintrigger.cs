// ===== WinTrigger.cs =====
// Ataseaza pe un GameObject cu Collider (trigger) la finalul hartii
// MazeGenerator il va crea si pozitiona automat

using UnityEngine;

public class WinTrigger : MonoBehaviour
{
    private bool hasTriggered = false;

    private void OnTriggerEnter(Collider other)
    {
        if (hasTriggered) return;

        // Acceptam playerul dupa componenta de miscare sau, fallback, dupa Rigidbody.
        bool isPlayer = other.GetComponent<BallMovement>() != null || other.GetComponent<Rigidbody>() != null;
        if (!isPlayer) return;

        WinMenu winMenu = FindFirstObjectByType<WinMenu>();
        if (winMenu == null) return;

        hasTriggered = true;
        winMenu.ShowWinScreen();
    }
}