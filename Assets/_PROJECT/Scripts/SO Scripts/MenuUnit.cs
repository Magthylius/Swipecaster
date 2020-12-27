using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(menuName = "Unit/Menu Unit")]
public class MenuUnit : ScriptableObject
{
    [Header("General")]
    public string ID;
    public int level;
}
