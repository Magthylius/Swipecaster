using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DamagePopUp : MonoBehaviour
{
    private float popUpTime = 2.0f;
    private TextMeshPro _textMesh;
    private GameObject _parent;
    private GameObject offsetObj;

    private void Awake()
    {
        _textMesh = GetComponent<TextMeshPro>();
        _parent = transform.parent.gameObject;
        _parent.SetActive(false);
        
        //create parent to offset
        offsetObj = new GameObject("PopUpOffset");
        offsetObj.transform.position = _parent.transform.position;
        offsetObj.transform.SetParent(_parent.transform.parent);
        _parent.transform.SetParent(offsetObj.transform);
        offsetObj.transform.localScale = new Vector3(1, 1, 1);

        //offset
        offsetObj.transform.localPosition -= new Vector3(2.0f, 0.0f, 0f);
    }

    public void ShowPopUp(int damage, bool isDamage, bool isMitigated)
    {
        Color textColour = isDamage ? Color.red : Color.green;
        if (isMitigated) textColour = Color.gray;
        if(!_parent.activeInHierarchy)
        {
            _parent.SetActive(true);
            SetTextColour(textColour);
            ShowDamage(damage);
        }
        else
        {
            int cummulatedDamage = int.Parse(_textMesh.text);
            _textMesh.text = (cummulatedDamage + damage).ToString();
        }
    }

    public void SetTextColour(Color color) => _textMesh.color = color;
    public void ShowDamage(int damage) => StartCoroutine(DoPopUp(damage));

    private IEnumerator DoPopUp(int damage)
    {
        _parent.SetActive(true);
        _textMesh.text = damage.ToString();
        yield return new WaitForSeconds(popUpTime);
        _parent.SetActive(false);
    }

    private Transform GetUnit()
    {
        return transform.parent.parent.parent;
    }
}
