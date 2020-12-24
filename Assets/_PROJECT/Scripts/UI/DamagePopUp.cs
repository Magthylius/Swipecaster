using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DamagePopUp : MonoBehaviour
{
    private void Start()
    {
        gameObject.transform.position += new Vector3(0.0f, 0.5f, 0.0f);
    }

    public void showDamage(int damage)
    {
        gameObject.GetComponent<TextMeshPro>().text = damage.ToString();
        gameObject.SetActive(true);
        StartCoroutine(popUpTime());
    }

    private IEnumerator popUpTime()
    {
        yield return new WaitForSeconds(1f);
        gameObject.SetActive(false);
    }
}
