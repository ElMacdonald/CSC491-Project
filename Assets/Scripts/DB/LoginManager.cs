using UnityEngine;
using TMPro;

// Login uses joinCode + studentId as the Firestore document key.
// e.g. userId = "MATH101_john123"

public class LoginManager : MonoBehaviour
{
    [Header("UI References")]
    public GameObject      loginPanel;
    public TMP_InputField  studentIdInput;
    public TMP_InputField  joinCodeInput;
    public TMP_InputField  displayNameInput;
    public TextMeshProUGUI statusText;

    private const float SESSION_DURATION = 28800f; // 8 hours

    void Start()
    {
        if (PlayerPrefs.HasKey("sessionUserId") && PlayerPrefs.HasKey("sessionTimeEpoch"))
        {
            long savedEpoch = long.Parse(PlayerPrefs.GetString("sessionTimeEpoch", "0"));
            long elapsed    = System.DateTimeOffset.UtcNow.ToUnixTimeSeconds() - savedEpoch;

            if (elapsed < (long)SESSION_DURATION)
            {
                AutoLogin(PlayerPrefs.GetString("sessionUserId"), PlayerPrefs.GetString("sessionCode", ""));
                return;
            }
            else
            {
                ClearSession();
            }
        }

        loginPanel.SetActive(true);
    }

    // Strips non-alphanumeric characters to prevent malformed Firestore paths
    private static string SanitiseId(string raw)
    {
        if (string.IsNullOrEmpty(raw)) return raw;
        var sb = new System.Text.StringBuilder(raw.Length);
        foreach (char c in raw)
            if (char.IsLetterOrDigit(c) || c == '_')
                sb.Append(c);
        return sb.ToString();
    }

    public void OnLoginButtonPressed()
    {
        string studentId = SanitiseId(studentIdInput.text.Trim());
        string joinCode  = SanitiseId(joinCodeInput.text.Trim().ToUpper());
        string name      = studentId;

        if (string.IsNullOrEmpty(studentId)) { SetStatus("Please enter your student ID."); return; }
        if (string.IsNullOrEmpty(joinCode))  { SetStatus("Please enter your classroom join code."); return; }

        SetStatus("Loading...");

        string userId = joinCode + "_" + studentId;
        Session.userId        = userId;
        Session.classroomCode = joinCode;

        FirebaseManager.Instance.Load(userId, (loadedData) =>
        {
            if (loadedData != null)
            {
                Session.currentPlayer = loadedData;
#if UNITY_EDITOR
                Debug.Log("[Login] Loaded existing save for: " + userId);
#endif
            }
            else
            {
                Session.currentPlayer = new PlayerData
                {
                    userId        = userId,
                    displayName   = name,
                    classroomCode = joinCode
                };
                FirebaseManager.Instance.Save(userId, Session.currentPlayer,
#if UNITY_EDITOR
                    onSuccess: () => Debug.Log("[Login] New save created for: " + userId));
#else
                    onSuccess: null);
#endif
            }

            Session.StartSession();
            SaveSessionCookie(userId, joinCode);
            loginPanel.SetActive(false);
            SetStatus("");
        },
        (error) =>
        {
#if UNITY_EDITOR
            Debug.LogWarning("[Login] Firebase unavailable, offline mode: " + error);
#endif
            Session.currentPlayer = new PlayerData
            {
                userId        = userId,
                displayName   = name,
                classroomCode = joinCode
            };
            Session.StartSession();
            loginPanel.SetActive(false);
            SetStatus("(Offline mode)");
        });
    }

    void AutoLogin(string userId, string code)
    {
        Session.userId        = userId;
        Session.classroomCode = code;
        SetStatus("Loading...");

        FirebaseManager.Instance.Load(userId, (loadedData) =>
        {
            Session.currentPlayer = loadedData ?? new PlayerData { userId = userId, classroomCode = code };
            Session.StartSession();
            loginPanel.SetActive(false);
            SetStatus("");
#if UNITY_EDITOR
            Debug.Log("[Login] Auto-login OK: " + userId);
#endif
        },
        (error) =>
        {
            Session.currentPlayer = new PlayerData { userId = userId, classroomCode = code };
            Session.StartSession();
            loginPanel.SetActive(false);
            SetStatus("(Offline mode)");
        });
    }

    void SaveSessionCookie(string userId, string code)
    {
        PlayerPrefs.SetString("sessionUserId",    userId);
        PlayerPrefs.SetString("sessionCode",      code);
        PlayerPrefs.SetString("sessionTimeEpoch", System.DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString());
        PlayerPrefs.Save();
    }

    void ClearSession()
    {
        PlayerPrefs.DeleteKey("sessionUserId");
        PlayerPrefs.DeleteKey("sessionCode");
        PlayerPrefs.DeleteKey("sessionTime");
        PlayerPrefs.DeleteKey("sessionTimeEpoch");
        PlayerPrefs.Save();
    }

    void SetStatus(string msg)
    {
        if (statusText != null) statusText.text = msg;
    }

    public void Logout()
    {
        ClearSession();
        Session.userId        = "";
        Session.classroomCode = "";
        Session.currentPlayer = null;
        loginPanel.SetActive(true);
    }
}
