using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DamagePopUp : MonoBehaviour
{
    private float popUpTime = 2.0f;
    private TextMeshPro _textMesh;
    private GameObject parent;
    private GameObject offsetObj;

    private void Awake()
    {
        _textMesh = GetComponent<TextMeshPro>();
        parent = transform.parent.gameObject;
        parent.SetActive(false);
        
        //create parent to offset
        offsetObj = new GameObject("PopUpOffset");
        offsetObj.transform.position = parent.transform.position;
        offsetObj.transform.SetParent(parent.transform.parent);
        parent.transform.SetParent(offsetObj.transform);
        offsetObj.transform.localScale = new Vector3(1, 1, 1);

        //offset
        offsetObj.transform.localPosition -= new Vector3(2.0f, 0.0f, 0f);
    }

    public void SetTextColour(Color color) => _textMesh.color = color;
    public void ShowDamage(int damage) => StartCoroutine(DoPopUp(damage));

    private IEnumerator DoPopUp(int damage)
    {
        parent.SetActive(true);
        _textMesh.text = damage.ToString();
        yield return new WaitForSeconds(popUpTime);
        parent.SetActive(false);
    }

    private Transform GetUnit()
    {
        return transform.parent.parent.parent;
    }
}
