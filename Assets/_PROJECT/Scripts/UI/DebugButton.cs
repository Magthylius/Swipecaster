using UnityEngine;

public class DebugButton : MonoBehaviour
{
    public bool disable = false;

    public void Button_DebugOnClick()
    {
        if (disable) return;
        Debug.Log($"{gameObject.name} Clicked.");
    }
}
