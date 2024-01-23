using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class GameManagerGM : MonoBehaviour
{
    [SerializeField] private generator generator;
    [SerializeField] private Transform tilePrefab;
    [SerializeField] private Transform gameHolder;
    [SerializeField] private TextMesh text1;
    [SerializeField] private TextMesh text2;
    [SerializeField] private TextMesh lengthtext;
    [SerializeField] private TextMesh timers;

    int phase = 1;
    private readonly float tileSize = 0.5f;
    private List<Tile> tiles = new();
    int initialTries = 0;
    int wrongClicks = 0;
    public int[] path = { 90, 91, 92, 93, 94, 84, 74, 64, 63, 62, 52, 42, 32, 22, 23, 24, 14, 4, 5, 6, 16, 26, 36, 37, 38, 28, 18, 19, 9 };
    private float currentTime;
    private float phase1time;
    private float phase2time;
    public int currentTile=0;
    public int length = 0;

    // Start is called before the first frame update
    void Start()
    {
        path = generator.generatepath();
        Debug.Log(path);
        length = path.Count(value => value != 0);
        lengthtext.text = "Length: " + length.ToString();
        CreateGameBoard();
    }

    void Update()
    {
        currentTime += Time.deltaTime;
    }


    public void CreateGameBoard()
    {

        for(int row =0; row< 10; row++)
        {
            for(int col =0; col<10; col++)
            {
                Transform tileTransform = Instantiate(tilePrefab);
                tileTransform.parent = gameHolder;
                float xIndex = col -((10 - 1)/ 2.0f);
                float yIndex = row - ((10 - 1) / 2.0f);
                tileTransform.localPosition = new Vector2(xIndex*tileSize, yIndex* tileSize);

                Tile tile = tileTransform.GetComponent<Tile>();
                tiles.Add(tile);
                tile.gameManager = this;
            }
        }

    }

   
    public void phase2()
    {
        foreach(Tile tile in tiles) {
            tile.cover();
        
        }
    }




    public bool rightTile(Tile tile)
    {
        int location = tiles.IndexOf(tile);

        if (location == path[currentTile])
        {
            if(location == 9)
            {
                if (phase == 1)
                {
                    phase1time = currentTime;
                    currentTime = 0;
                    currentTile = 0;
                    phase = 2;
                    phase2();
                    return false;
                }
                else
                {
                    phase2time = currentTime;
                    Results();
                    return false;
                }
            }
            else
            {
                currentTile++;
                return true;
            }

        }

        else {
            goBack();
            if (phase == 1)
            {
                initialTries++;
            }
            else
            {
                wrongClicks++;
            }
            return false; 
        }
    }

    private void Results()
    {
        foreach (Tile tile in tiles)
        {
            Destroy(tile.gameObject);

        }

        text1.text = "Your Initial number of wrong clicks: " + initialTries.ToString();
        text2.text = "Your Wrong Clicks on second try: "+ wrongClicks.ToString();
        timers.text = "Phase 1:" + Mathf.FloorToInt(phase1time).ToString() + "  Phase 2:" + Mathf.FloorToInt(phase2time).ToString();
        text1.gameObject.SetActive(true);
        text2.gameObject.SetActive(true);
        timers.gameObject.SetActive(true);
        StartCoroutine(SendResultsToServer());

    }

    private void goBack()
    {
        if (currentTile != 0)
        {
            currentTile--;
            tiles[path[currentTile]].cover();

        }
    }



    IEnumerator SendResultsToServer()
    {
        string username = UserManager.Instance.Username; // Get the logged-in username

        WWWForm form = new WWWForm();
        form.AddField("username", username);
        form.AddField("length", length.ToString());
        form.AddField("initialTries", initialTries);
        form.AddField("wrongClicks", wrongClicks);
        form.AddField("phase1time", Mathf.FloorToInt(phase1time).ToString());
        form.AddField("phase2time", Mathf.FloorToInt(phase2time).ToString());

        using (UnityWebRequest www = UnityWebRequest.Post("http://localhost/Unity/GMsubmit_results.php", form))
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
