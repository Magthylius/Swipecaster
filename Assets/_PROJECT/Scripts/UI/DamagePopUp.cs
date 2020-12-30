using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DamagePopUp : MonoBehaviour
{
    [SerializeField] private float popUpTime = 1.5f;
    private TextMeshPro _textMesh;

    private void Awake()
    {
        _textMesh = GetComponent<TextMeshPro>();
        _textMesh.enabled = false;
    }

    public void ShowDamage(int damage) => StartCoroutine(DoPopUp(damage));

    private IEnumerator DoPopUp(int damage)
    {
        _textMesh.enabled = true;
        _textMesh.text = damage.ToString();
        yield return new WaitForSeconds(popUpTime);
        _textMesh.enabled = false;
    }
}
