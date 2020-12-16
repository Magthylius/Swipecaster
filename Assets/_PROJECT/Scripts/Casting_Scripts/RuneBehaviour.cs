using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RuneBehaviour : MonoBehaviour
{
    CastingManager castingManager;
    
    public float deactivateLevel = -2500.0f;
    
    float maxVelocity;
    
    Rigidbody2D rb;
    
    void Awake() => rb = GetComponent<Rigidbody2D>();
    
    void Start()
    {
        castingManager = CastingManager.instance;
        GetMaxVelocity();
    }
    
    void Update()
    {
        SelfDeactivate();
    }
    
    void FixedUpdate()
    {
        rb.velocity = Vector2.ClampMagnitude(rb.velocity, maxVelocity);
    }

    void SelfDeactivate()
    {
        if (GetComponent<RectTransform>().anchoredPosition.y <= deactivateLevel)
        {
            castingManager.runeList.Remove(this.gameObject);
            rb.velocity = new Vector2(0, -10);
            gameObject.SetActive(false);
        }
    }

    #region Global Accessors
    void GetMaxVelocity() => maxVelocity = castingManager.maxVel;
    #endregion

}
