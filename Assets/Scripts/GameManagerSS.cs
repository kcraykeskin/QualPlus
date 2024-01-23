using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class GameManagerSS : MonoBehaviour
{
    [SerializeField] private Transform buttonPrefab;
    [SerializeField] private Transform sozlukPrefab;
    private List<buttonPrefab> buttons = new();
    private List<sozluk> sozlukk = new();
    [SerializeField] private Sprite[] images;
    [SerializeField] private TextMesh centerObj;
    [SerializeField] private TextMesh timerText;
    [SerializeField] private TextMesh results;
    private float currentTime;

    bool isCountingDown = false;
    int[] sozluksýrasý = { 0, 1, 2, 3, 4, 5, 6, 7, 8 };
    int correctNumber = 0;
    int correct = 0;
    int wrong = 0;
    float gameTimeInSeconds = 30.0f; 


    void Start()
    {

        ShuffleArrayElements(sozluksýrasý);
        createButtons();
        centerSprite();
        currentTime = gameTimeInSeconds;
        timerText.text = "Time: " + Mathf.CeilToInt(currentTime).ToString();
        isCountingDown=true;
        StartCoroutine(Countdown());
    }
    private IEnumerator Countdown()
    {
        while (isCountingDown)
        {
            currentTime -= 1.0f;
            timerText.text = "Time: " + Mathf.CeilToInt(currentTime).ToString();

            if (currentTime <= 0.0f)
            {
                GameOver();
                isCountingDown = false; // Stop the countdown
            }

            yield return new WaitForSeconds(1.0f); // Wait for 1 second before the next iteration
        }
    }


    private void centerSprite()
    {

        int randomNumber = Random.Range(0, 9);
        centerObj.text = (randomNumber+1).ToString();
        correctNumber = sozluksýrasý[randomNumber];

    }
    private void createButtons()
    {
        for (int col = 0; col < 9; col++)
        {
            Transform buttonTransform = Instantiate(buttonPrefab);
            Transform sozlukTransform = Instantiate(sozlukPrefab);
            float xIndex = buttonTransform.position.x + (2.3f * col);
            float yIndex = buttonTransform.position.y;
            buttonTransform.localPosition = new Vector2(xIndex, yIndex);
            xIndex = sozlukTransform.position.x + (2.3f * col);
            yIndex = sozlukTransform.position.y;
            sozlukTransform.localPosition = new Vector2(xIndex, yIndex);

            buttonPrefab thisbutton = buttonTransform.GetComponent<buttonPrefab>();
            thisbutton.ChangeSprite(col, images[col]);
            buttons.Add(thisbutton);
            thisbutton.gameManager = this;

            sozluk thissozluk = sozlukTransform.GetComponent<sozluk>();
            thissozluk.InitializeElement(col + 1, images[sozluksýrasý[col]]);
            sozlukk.Add(thissozluk);


            Destroy(sozlukPrefab.gameObject);
            
        }
    }


    public void checkTrue(buttonPrefab sentButton)
    {

        Debug.Log(sentButton.id);
        
        if (sentButton.id == correctNumber)
        {
            correct++;
            centerSprite();
        }
        else {
            wrong++;
                }
    }

    void ShuffleArrayElements<T>(T[] array)
    {
        int n = array.Length;
        for (int i = n - 1; i > 0; i--)
        {
            int randomIndex = Random.Range(0, i + 1);
            T temp = array[i];
            array[i] = array[randomIndex];
            array[randomIndex] = temp;
        }
    }
    private void GameOver()
    {
        foreach (buttonPrefab buttonn in buttons)
        {
            Destroy(buttonn.gameObject);

        }

        foreach (sozluk sozluuk in sozlukk)
        {
            Destroy(sozluuk.gameObject);

        }

        Destroy(buttonPrefab.gameObject);

        timerText.gameObject.SetActive(false);
        centerObj.gameObject.SetActive(false);
        results.text = "Total Points: " + Mathf.FloorToInt(correct).ToString() + " Wrong Clicks: " + Mathf.FloorToInt(wrong).ToString();
        results.gameObject.SetActive(true);
        StartCoroutine(SendResultsToServer());
    }

    IEnumerator SendResultsToServer()
    {
        string username = UserManager.Instance.Username; // Get the logged-in username

        WWWForm form = new WWWForm();
        form.AddField("username", username);
        form.AddField("time", gameTimeInSeconds.ToString());
        form.AddField("initialTries", correct);
        form.AddField("wrongClicks", wrong);

        using (UnityWebRequest www = UnityWebRequest.Post("http://localhost/Unity/SSsubmit_results.php", form))
        {
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.ConnectionError || www.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.Log("Error: " + www.error);
            }
            else
            {
                Debug.Log("Results submitted successfully.");
            }
        }
    }
}
