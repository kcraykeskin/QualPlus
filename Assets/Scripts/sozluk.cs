using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sozluk : MonoBehaviour
{
    // Start is called before the first frame update


    [SerializeField] TextMesh sayý;
    [SerializeField] SpriteRenderer sembol;


    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void InitializeElement(int sayým, Sprite spriteým)
    {
        sayý.GetComponent<TextMesh>().text= sayým.ToString();
        sembol.GetComponent<SpriteRenderer>().sprite = spriteým;

    }
}
