using UnityEngine;


//
//  Currently a transparent stub — does nothing yet.
//  When your teammate finishes the server, uncomment
//  SaveToFirebase() and fill in your response parsing.
//  Nothing else in the project needs to change at that point.
//
//  Attach to the same GameObject as FirebaseManager.

public class JsonRelay : MonoBehaviour
{
    public static JsonRelay Instance { get; private set; }

    void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    /// <summary>
    /// Call this with the JSON string your server returns.
    /// Currently passes it straight to the game.
    /// Uncomment SaveToFirebase() below when server is ready.
    /// </summary>
    public void HandleServerResponse(string jsonFromServer)
    {
        DeliverToGame(jsonFromServer);
        // SaveToFirebase(jsonFromServer); // uncomment when server is ready
    }

    void DeliverToGame(string json)
    {
        // Replace this with however your game handles server responses
        Debug.Log("[JsonRelay] Delivered to game: " + json);
    }

    // ── Uncomment when server is ready ───────────────────────
    /*
    void SaveToFirebase(string json)
    {
        if (Session.currentPlayer == null) return;

        // Parse the server's JSON and update Session.currentPlayer fields
        // Example:
        // ServerResponse resp = JsonUtility.FromJson<ServerResponse>(json);
        // Session.currentPlayer.someField = resp.someField;

        FirebaseManager.Instance.Save(
            Session.userId,
            Session.currentPlayer,
            onSuccess: () => Debug.Log("[JsonRelay] Server response saved to Firebase.")
        );
    }
    */
}
