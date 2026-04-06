using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class WinChecker : MonoBehaviour
{
    [Header("Win Panel")]
    public GameObject winPanel;

    [Header("Player")]
    public Transform player;
    public Vector2 respawnPoint;

    [Header("Timing")]
    public float winDelay     = 1f;
    public float checkInterval = 0.2f;

    private Coroutine winRoutine;
    public GameObject[] collectables;
    public ObjectiveTracker objTracker;

    private void Start()
    {
        InvokeRepeating(nameof(CheckChildren), 0f, checkInterval);
        respawnPoint = player.position;

        if (GameObject.Find("Objective Manager") != null)
            objTracker = GameObject.Find("Objective Manager").GetComponent<ObjectiveTracker>();

        collectables = new GameObject[transform.childCount];
        int i = 0;
        foreach (Transform child in transform)
            collectables[i++] = child.gameObject;
    }

    private void CheckChildren()
    {
        if (winPanel != null && winPanel.activeSelf) return;

        foreach (Transform child in transform)
        {
            if (child.gameObject.activeSelf)
            {
                if (winRoutine != null) { StopCoroutine(winRoutine); winRoutine = null; }
                return;
            }
        }

        if (winRoutine == null)
        {
            if (objTracker != null)
            {
                if (objTracker.levelWon) winRoutine = StartCoroutine(WinAfterDelay());
            }
            else
            {
                winRoutine = StartCoroutine(WinAfterDelay());
            }
        }
    }

    private IEnumerator WinAfterDelay()
    {
        float timer = 0f;
        while (timer < winDelay)
        {
            timer += Time.deltaTime;
            foreach (Transform child in transform)
            {
                if (child.gameObject.activeSelf) { winRoutine = null; yield break; }
            }
            yield return null;
        }

        if (winPanel != null) winPanel.SetActive(true);

        if (LevelManager.Instance != null)
            LevelManager.Instance.CompleteLevel(SceneManager.GetActiveScene().buildIndex);

        winRoutine = null;
    }

    public void ResetLevel()
    {
        if (winRoutine != null) { StopCoroutine(winRoutine); winRoutine = null; }

        foreach (GameObject c in collectables)
            if (c != null) c.SetActive(true);

        if (winPanel != null) winPanel.SetActive(false);
        if (player != null)   player.position = respawnPoint;
    }
}
