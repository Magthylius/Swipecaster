using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DamagePopUp : MonoBehaviour
{
    [SerializeField] private float popUpTime = 1.5f;
    private TextMeshPro _textMesh;
    public GameObject parent;

    private void Awake()
    {
        _textMesh = GetComponent<TextMeshPro>();
        parent = transform.parent.gameObject;
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
