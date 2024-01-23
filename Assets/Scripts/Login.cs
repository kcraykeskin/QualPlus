using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using TMPro;

public class Login : MonoBehaviour
{
    public TMP_InputField usernameInput;
    public TMP_InputField passwordInput;
    public Button loginButton;
    public Button goToRegisterButton;



    void Start()
    {
        loginButton.onClick.AddListener(LoginUser);
        goToRegisterButton.onClick.AddListener(MoveToRegister);
    }

    void LoginUser()
    {
        StartCoroutine(LoginRoutine());
    }

    IEnumerator LoginRoutine()
    {
        WWWForm form = new WWWForm();
        form.AddField("username", usernameInput.text);
        form.AddField("password", passwordInput.text);

        UnityWebRequest www = UnityWebRequest.Post("http://localhost/Unity/Login.php", form);
        yield return www.SendWebRequest();

        if (www.result == UnityWebRequest.Result.ConnectionError || www.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.Log("Error: " + www.error);
        }
        else
        {
            // Check the response text
            if (www.downloadHandler.text == "success")
            {
                UserManager.Instance.Username = usernameInput.text;

                Debug.Log($"Logging in '{usernameInput.text}'");
                SceneManager.LoadScene("GameSelect");
            }
            else
            {
                Debug.Log("Incorrect credentials");
            }
        }
    }

    void MoveToRegister()
    {
        Debug.Log("calýstýmaslýnda");
        SceneManager.LoadScene("Register");
    }
}
