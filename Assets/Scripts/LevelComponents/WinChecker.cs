using UnityEngine;
using System.Collections;

public class WinChecker : MonoBehaviour
{
    [Header("UI Panel to Enable When All Children Are Inactive")]
    public GameObject winPanel;

    [Header("Player Object To Reset")]
    public Transform player;

    [Header("Where the player should respawn")]
    public Vector2 respawnPoint;

    [Header("Time before showing win panel (seconds)")]
    public float winDelay = 1f;

    [Header("How often to check (seconds)")]
    public float checkInterval = 0.2f;

    private Coroutine winRoutine;
    public GameObject[] collectables;

    public ObjectiveTracker objTracker;
    private void Start()
    {
        InvokeRepeating(nameof(CheckChildren), 0f, checkInterval);
        respawnPoint = player.position;
        if(GameObject.Find("Objective Manager") != null)
            objTracker = GameObject.Find("Objective Manager").GetComponent<ObjectiveTracker>();
        int counter = 0;
        collectables = new GameObject[transform.childCount];
        foreach (Transform child in transform)
        {
            collectables[counter] = child.gameObject;
            counter++;
        }
    }

    private void CheckChildren()
    {
        // Already won?
        if (winPanel != null && winPanel.activeSelf)
            return;

        // Check every child
        foreach (Transform child in transform)
        {
            if (child.gameObject.activeSelf)
            {
                // A child is active → cancel win timer if running
                if (winRoutine != null)
                {
                    StopCoroutine(winRoutine);
                    winRoutine = null;
                }
                return;
            }
        }

        // All children are inactive → start win delay if not already started
        if (winRoutine == null)
            if(objTracker != null)
            {
                if(objTracker.levelWon == true)
                {
                    winRoutine = StartCoroutine(WinAfterDelay());
                }
            }
            else
            {
                winRoutine = StartCoroutine(WinAfterDelay());
            }
                
    }

    private IEnumerator WinAfterDelay()
    {
        float timer = 0f;

        while (timer < winDelay)
        {
            timer += Time.deltaTime;

            // If ANY child reactivates during delay → cancel win
            foreach (Transform child in transform)
            {
                if (child.gameObject.activeSelf)
                {
                    winRoutine = null;
                    yield break;
                }
            }

            yield return null;
        }

        // After delay → WIN
        if (winPanel != null)
            winPanel.SetActive(true);

        winRoutine = null;
    }


    public void ResetLevel()
    {
        // Cancel win timer
        if (winRoutine != null)
        {
            StopCoroutine(winRoutine);
            winRoutine = null;
        }

        // Reactivate all children
        foreach(GameObject collectable in collectables)
        {
            if(collectable != null)
                collectable.SetActive(true);
        }

        // Hide win panel
        if (winPanel != null)
            winPanel.SetActive(false);

        // Reset player position
        if (player != null && respawnPoint != null)
            player.position = respawnPoint;
    }
}
