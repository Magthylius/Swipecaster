using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DamagePopUp : MonoBehaviour
{
    private float popUpTime = 2.0f;
    private TextMeshPro _textMesh;
    private GameObject parent;

    private void Awake()
    {
        _textMesh = GetComponent<TextMeshPro>();
        parent = transform.parent.gameObject;
        parent.transform.position = new Vector3(0.0f, 0.0f, 0.0f);
        parent.SetActive(false);
    }

    public void ShowDamage(int damage) => StartCoroutine(DoPopUp(damage));

    private IEnumerator DoPopUp(int damage)
    {
        parent.SetActive(true);
        _textMesh.text = damage.ToString();
        yield return new WaitForSeconds(popUpTime);
        parent.SetActive(false);
    }
}
