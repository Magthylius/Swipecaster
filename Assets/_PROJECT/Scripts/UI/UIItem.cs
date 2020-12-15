using System;
using UnityEngine;
using UnityEngine.UI;

public class UIItem : MonoBehaviour
{
    [SerializeField] private bool _disableButtonDebugs = false;
    [SerializeField] private string _title;
    [SerializeField] private Button _button;
    [SerializeField] private DebugButton _debugButton;
    [SerializeField] private Image _image;
    [SerializeField] private Color _imageColour;

    public string Title => _title;
    public Button UIButton => _button;
    public Image UIImage { get => _image; set => _image = value; }
    public Color UIColour => _imageColour;

    private void Awake()
    {
        CheckForMissingReferences();
        OnValidate();
    }

    private void OnValidate()
    {
        string upTitle = UppercaseFirstLetter(_title.Trim());
        gameObject.name = "UI Item - " + upTitle;
        if (_button != null) _button.gameObject.name = "Button - " + upTitle;
        if (_image != null) _image.color = _imageColour;
        if (_debugButton != null) _debugButton.disable = _disableButtonDebugs;
    }

    private void CheckForMissingReferences()
    {
        if (_button == null) _button = transform.GetChild(0).GetComponent<Button>();
        if (_image == null) _image = transform.GetChild(1).GetComponent<Image>();
    }

    private static string UppercaseFirstLetter(string word)
    {
        if (string.IsNullOrEmpty(word)) return string.Empty;
        char[] chars = word.ToCharArray();

        chars[0] = char.ToUpper(chars[0]);
        return new string(chars);
    }
}
