using UnityEngine;

public class User : MonoBehaviour
{
    public static User instance;
    
    private string _nickname = "Player 1";

    public string Nickname
    {
        get
        {
            return _nickname;
        }
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this);
        }
    }

    [SerializeField]
    private Rect _nicnameLabelPosition = new Rect(200, 20, 100, 20);

    [SerializeField]
    private Rect _nicknameFieldPosition = new Rect(300, 20, 100, 20);

    private void OnGUI()
    {
        GUI.Label(_nicnameLabelPosition, "Nickname");

        if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == "Start")
        {
            _nickname = GUI.TextField(_nicknameFieldPosition, _nickname);
        }
        else
        {
            GUI.Label(_nicknameFieldPosition, _nickname);
        }
    }
}
