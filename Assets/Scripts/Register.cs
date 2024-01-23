using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using System.Collections;
using TMPro;


public class Register : MonoBehaviour
{
    public TMP_InputField usernameInput;
    public TMP_InputField passwordInput;
    public Button registerButton;
    public Button goToLoginButton;

    private string registerUrl = "http://localhost/Unity/Register.php"; // Replace with your API endpoint

    void Start()
    {
        
        registerButton.onClick.AddListener(RegisterUser);
        goToLoginButton.onClick.AddListener(goToLoginScene);
    }

    void goToLoginScene()
    {
        SceneManager.LoadScene("Login");
    }

    void RegisterUser()
    {
        StartCoroutine(RegisterRoutine());
    }

    IEnumerator RegisterRoutine()
    {
        WWWForm form = new WWWForm();
        form.AddField("username", usernameInput.text);
        form.AddField("password", passwordInput.text);

        UnityWebRequest www = UnityWebRequest.Post(registerUrl, form);
        yield return www.SendWebRequest();

        if (www.result == UnityWebRequest.Result.ConnectionError || www.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.LogError("Error: " + www.error);
        }
        else
        {
            Debug.Log("User registered: " + www.downloadHandler.text);
        }
    }
}
