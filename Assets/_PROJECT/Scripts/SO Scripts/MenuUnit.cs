using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(menuName = "Menu/Menu Unit")]
public class MenuUnit : ScriptableObject
{
    [Header("General")]
    public string ID;
    public int level;
}
