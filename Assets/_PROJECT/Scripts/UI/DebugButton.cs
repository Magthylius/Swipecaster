using UnityEngine;

public class DebugButton : MonoBehaviour
{
    public void Button_DebugOnClick()
    {
        Debug.Log($"{gameObject.name} Clicked.");
    }
}
