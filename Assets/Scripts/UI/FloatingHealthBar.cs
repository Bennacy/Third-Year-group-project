using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FloatingHealthBar : MonoBehaviour
{
    public IHasHealth parent;
    private Camera mainCam;
    public GameObject canvas;
    public Color backColor;
    public Color frontColor;
    public Color flashColor;
    public Image backImage;
    public Image frontImage;
    
    public RectTransform barTransform;
    private float maxWidth;
    private float targetWidth;
    private int previousHealth;
    
    void Start()
    {
        parent = GetComponentInParent<IHasHealth>();
        targetWidth = maxWidth = barTransform.sizeDelta.x;
    }

    void Update()
    {
        Vector3 rotation = transform.rotation.eulerAngles;
        rotation.y = GameManager.Instance.mainCam.transform.rotation.eulerAngles.y + 180;
        canvas.transform.rotation = Quaternion.Euler(rotation);
        
        if(parent.health < previousHealth){
            StartCoroutine(UpdateBarWidth());
        }
        
        Vector2 newSize = barTransform.sizeDelta;
        newSize.x = Mathf.Lerp(newSize.x, targetWidth, 5*Time.deltaTime);
        barTransform.sizeDelta = newSize;

        previousHealth = parent.health;
    }

    IEnumerator UpdateBarWidth(){
        targetWidth = (parent.health*maxWidth) / parent.maxHealth;
        for(int i = 0; i < 4; i++){
            frontImage.color = (i % 2 == 0) ? flashColor : frontColor;
            yield return new WaitForSeconds(.05f);
        }
    }
}
