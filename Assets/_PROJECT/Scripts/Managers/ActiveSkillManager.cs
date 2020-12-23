using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActiveSkillManager : MonoBehaviour
{
    public static ActiveSkillManager Instance;
    private TurnBaseManager _turnBaseManager;

    [Header("UI")]
    [SerializeField] private Image healthBar;
    [SerializeField] private Image skillChargeBar;
    [SerializeField] private Image casterPortrait;
    [SerializeField] private GameObject skillActivator;

    private void UpdateHealthBar()
    {

    }

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        _turnBaseManager = TurnBaseManager.instance;
    }
}