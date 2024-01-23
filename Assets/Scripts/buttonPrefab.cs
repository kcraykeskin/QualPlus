using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class buttonPrefab : MonoBehaviour
{
    public GameManagerSS gameManager;



    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public int id;
    public void ChangeSprite(int newid, Sprite image)
    {
        GetComponent<SpriteRenderer>().sprite = image;
        id= newid;
    }

    public void OnMouseDown()
    {
        gameManager.checkTrue(this);
    }


}
