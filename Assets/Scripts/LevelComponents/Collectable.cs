using UnityEngine;
using UnityEngine.SceneManagement;

// ================================================================
//  Collectable.cs  —  REPLACE your existing one
//  Original behavior intact — Destroy() still called as before.
//  Checks parent for remaining siblings BEFORE destroying.
// ================================================================

public class Collectable : MonoBehaviour
{
void OnTriggerEnter2D(Collider2D other)
{
    Debug.Log("[Collectable] Trigger hit by: " + other.tag);
    if (other.tag == "Player")
    {
            
            // Check siblings BEFORE destroying this object
            bool lastOne = false;
            Transform parent = transform.parent;
            if (parent != null)
            {
                int activeCount = 0;
                foreach (Transform sibling in parent)
                {
                    if (sibling.gameObject.activeSelf && sibling.gameObject != gameObject)
                        activeCount++;
                }
                // If no other active siblings, this is the last collectable
                if (activeCount == 0)
                    lastOne = true;
            }

            // Destroy as original
            Destroy(this.gameObject);

            // Save if this was the last one
            if (lastOne)
            {
                Debug.Log("[Collectable] Last one collected — saving progress.");

                if (Session.currentPlayer != null)
                {
                    string levelId = SceneManager.GetActiveScene().buildIndex.ToString();
                    Session.currentPlayer.MarkLevelComplete(levelId, stars: 1);
                    Session.currentPlayer.coins += 10;

                    if (LevelManager.Instance != null)
                        LevelManager.Instance.CompleteLevel(SceneManager.GetActiveScene().buildIndex);

                    FirebaseManager.Instance.Save(
                        Session.userId,
                        Session.currentPlayer,
                        onSuccess: () => Debug.Log("[Firebase] Level " + levelId + " saved!"),
                        onError:   (e) => Debug.LogWarning("[Firebase] Save failed: " + e)
                    );
                }
                else
                {
                    Debug.LogWarning("[Collectable] Session.currentPlayer is null — not saving.");
                }
            }
        }
    }
}
