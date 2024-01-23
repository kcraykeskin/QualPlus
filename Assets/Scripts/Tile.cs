using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Tile : MonoBehaviour
{
    [Header("Tile Sprites")]
    [SerializeField] private Sprite unclickedTile;
    [SerializeField] private Sprite clickedTile;
    [SerializeField] private Sprite wrongTile;

    [Header("Game Manager")]
    public GameManagerGM gameManager;
    private SpriteRenderer spriteRenderer;
    
    private void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (gameManager.rightTile(this))
            {
                spriteRenderer.sprite = clickedTile;
            }
            else
            {
                StartCoroutine(clickedWrong());
            }

        }
        
    }
    
    public IEnumerator clickedWrong()
    {
        spriteRenderer.sprite = wrongTile;
        yield return new WaitForSeconds(1);
        spriteRenderer.sprite = unclickedTile;
    }

    public void cover()
    {
        Debug.Log("ivearrived");
        spriteRenderer.sprite = unclickedTile;
    }



    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

    }
   
}
