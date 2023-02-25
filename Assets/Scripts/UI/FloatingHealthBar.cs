using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingHealthBar : MonoBehaviour
{
    public IHasHealth parent;
    private Camera mainCam;
    public GameObject canvas;
    
    public RectTransform barTransform;
    private float maxWidth;
    
    void Start()
    {
        parent = GetComponent<IHasHealth>();
        maxWidth = barTransform.sizeDelta.x;
    }

    void Update()
    {
        Vector3 rotation = transform.rotation.eulerAngles;
        rotation.y = GameManager.Instance.mainCam.transform.rotation.eulerAngles.y + 180;
        canvas.transform.rotation = Quaternion.Euler(rotation);
        
        Vector2 newSize = barTransform.sizeDelta;
        newSize.x = (parent.health*maxWidth) / parent.maxHealth;
        barTransform.sizeDelta = newSize;
    }
}
