using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sozluk : MonoBehaviour
{
    // Start is called before the first frame update


    [SerializeField] TextMesh say�;
    [SerializeField] SpriteRenderer sembol;


    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void InitializeElement(int say�m, Sprite sprite�m)
    {
        say�.GetComponent<TextMesh>().text= say�m.ToString();
        sembol.GetComponent<SpriteRenderer>().sprite = sprite�m;

    }
}
