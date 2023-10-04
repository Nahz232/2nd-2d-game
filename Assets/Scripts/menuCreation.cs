using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class menuCreation : MonoBehaviour
{
    private SpriteRenderer bgspriteRender;
    private SpriteRenderer buttonspriteRender;
   
        private void Start()
    {
        Sprite newbgSprite = Resources.Load<Sprite>("bg6");
        GameObject bg = new GameObject();
        bg.name = "Background";
        bg.transform.SetParent(transform);
        bg.AddComponent<SpriteRenderer>();
        bgspriteRender = bg.GetComponent<SpriteRenderer>();
        bgspriteRender.sprite = newbgSprite;
        Camera mainCamera = Camera.main;
        float cameraHeight = 2f * mainCamera.orthographicSize;
        float cameraWidth = cameraHeight * mainCamera.aspect;
        float spriteWidth = bgspriteRender.bounds.size.x;
        float spriteHeight = bgspriteRender.bounds.size.y;
        float scaleX = cameraWidth / spriteWidth;
        float scaleY = cameraHeight / spriteHeight;
        bg.transform.localScale = new Vector3(scaleX, scaleY, 1f);

        /*
        Sprite newButtonSprite = Resources.Load<Sprite>("Button_1");
        
        newCanvas.transform.SetParent(transform);
        newCanvas.transform.localScale = new Vector3(scaleX, scaleY, 1f);
        GameObject startButton = new GameObject();
        startButton.name = "StartButton";
        startButton.transform.SetParent(transform);
        startButton.AddComponent<SpriteRenderer>();
        buttonspriteRender = startButton.GetComponent<SpriteRenderer>();
        buttonspriteRender.sprite = newButtonSprite;
        startButton.transform.localScale = new Vector3(scaleX, scaleY, 1f);
        */
    }
}
