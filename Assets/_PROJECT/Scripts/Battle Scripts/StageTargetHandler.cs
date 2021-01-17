using ConversionFunctions;
using System.Collections.Generic;
using UnityEngine;

public class StageTargetHandler : MonoBehaviour
{
    [SerializeField] private Vector2 positionOffset = Vector2.zero;
    [SerializeField] private Vector3 scaleOffset = Vector3.one;
    [SerializeField] private string handlerName = string.Empty;
    [SerializeField] private Color handlerColour = Color.green;
    private InformationManager _infomation;
    private Sprite _prioritySprite;
    private GameObject _spriteHolder;
    private SpriteRenderer _spriteRenderer;

    private void Start()
    {
        _infomation = InformationManager.instance;
        InjectBattleStage();
        LoadPrioritySprite();
        LoadSpriteHolder();
    }
    
    public void UpdateHandler(in GameObject target)
    {
        if (target == null) { DeactivateSpriteHolderAndReset(); return; }

        var targetParent = target.transform.parent;
        if (targetParent == null) { DeactivateSpriteHolderAndReset(); return; }

        ActivateSpriteHolder(targetParent);
    }

    private void InjectBattleStage() => BattlestageManager.instance.SetStageTargetHandler(this);
    private void LoadPrioritySprite() => _prioritySprite = _infomation.GetPrioritySprite();
    private void LoadSpriteHolder()
    {
        _spriteHolder = _infomation.InstantiateSpriteHolder(handlerName, null, scaleOffset, _prioritySprite);
        _spriteRenderer = _spriteHolder.GetComponent<SpriteRenderer>();
        _spriteRenderer.color = handlerColour;
    }
    private void ActivateSpriteHolder(Transform transform)
    {
        _spriteHolder.transform.position = transform.position.AsVector2() + positionOffset;
        _spriteHolder.transform.SetParent(transform);
        ActivateSpriteHolder();
    }
    private void DeactivateSpriteHolder(Transform transform)
    {
        _spriteHolder.transform.SetParent(transform);
        DeactivateSpriteHolder();
    }
    public void ActivateSpriteHolder()
    {
        _spriteHolder.SetActive(true);
        _spriteRenderer.enabled = true;
    }
    public void DeactivateSpriteHolder()
    {
        _spriteHolder.SetActive(false);
        _spriteRenderer.enabled = false;
    }
    public void DeactivateSpriteHolderAndReset()
    {
        DeactivateSpriteHolder(null);
    }
}
