using UnityEngine;
using UnityEngine.SceneManagement;

public class Collectable : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag != "Player") return;

        // Count active siblings before destroying this object
        bool lastOne = false;
        Transform parent = transform.parent;
        if (parent != null)
        {
            int activeCount = 0;
            foreach (Transform sibling in parent)
                if (sibling.gameObject.activeSelf && sibling.gameObject != gameObject)
                    activeCount++;
            lastOne = activeCount == 0;
        }

        Destroy(this.gameObject);

        if (lastOne)
        {
            if (Session.currentPlayer != null)
                Session.currentPlayer.coins += 10;

            if (LevelManager.Instance != null)
                LevelManager.Instance.CompleteLevel(SceneManager.GetActiveScene().buildIndex);
        }
    }
}
