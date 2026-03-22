using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// ================================================================
//  EnemyDeclan.cs  —  REPLACE your existing one
//  Your original code is fully intact.
//  Firebase save added when enemy is defeated.
// ================================================================

public class EnemyDeclan : MonoBehaviour
{
    public shipMovement shipMovement;
    public GameObject winPanel;

    void Start() { }

    void Update() { }

    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("[EnemyDeclan] Hit — Session.currentPlayer: " + (Session.currentPlayer == null ? "NULL" : Session.currentPlayer.userId));
        if (other.gameObject.tag == "Projectile")
        {
            if (shipMovement.power == 10)
            {
                winPanel.SetActive(true);
                Destroy(other.gameObject);

                // Save progress to Firebase on enemy defeat
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
                        onSuccess: () => Debug.Log("[Firebase] Enemy level " + levelId + " saved!"),
                        onError:   (e) => Debug.LogWarning("[Firebase] Save failed: " + e)
                    );
                }
                else
                {
                    Debug.LogWarning("[EnemyDeclan] Session.currentPlayer is null — not saving.");
                }

                Destroy(this.gameObject);
            }
            else
            {
                Destroy(other.gameObject);
            }
        }
    }
}
