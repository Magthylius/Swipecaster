using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DamagePopUp : MonoBehaviour
{
    private void Awake()
    {
       // gameObject.GetComponent<RectTransform>().position += new Vector3(0.0f, 5.0f, 0.0f);
       // Debug.Log(gameObject.GetComponent<RectTransform>().position.y);
      //  gameObject.transform.position += new Vector3(0.0f, 1.0f, 0.0f);
    }

    public void showDamage(int damage)
    {
        gameObject.GetComponent<TextMeshPro>().text = damage.ToString();
        gameObject.SetActive(true);
        StartCoroutine(popUpTime());
    }

    private IEnumerator popUpTime()
    {
        yield return new WaitForSeconds(1.5f);
        gameObject.SetActive(false);
    }
}
