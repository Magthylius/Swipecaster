using UnityEngine;

public class UnitAudio : MonoBehaviour
{
    [SerializeField] private AudioData audioData;
    [SerializeField] private string hurtAudioNameKey;
    [SerializeField] private string attackAudioNameKey;
    [SerializeField] private string healAudioNameKey;
    private AudioManager _audioManager;
    private Unit _unit;
    private bool _isPlayer;

    #region Management Methods

    private void Start()
    {
        _audioManager = AudioManager.instance;
        SubscribeAudioEvents();
    }
    private void OnDestroy() => UnsubscribeAudioEvents();

    public void InjectUnit(Unit unit) => _unit = unit;
    public AudioData GetAudioData() => audioData;
    public void SetAudioData(AudioData data) => audioData = data;
    public void SetIsPlayer(bool isPlayer) => _isPlayer = isPlayer;

    #endregion

    #region Audio Methods

    private void PlayHurtAudio()
    {
        if (audioData == null) return;
        _audioManager.PlayRandomSFX(audioData, hurtAudioNameKey);
    }
    private void PlayAttackAudio()
    {
        if (audioData == null) return;
        if (_isPlayer) _audioManager.PlaySFX(audioData, attackAudioNameKey);
        else _audioManager.PlayRandomSFX(audioData, attackAudioNameKey);
    }
    private void PlayHealAudio()
    {
        if (audioData == null) return;
        _audioManager.PlaySFX(audioData, healAudioNameKey);
    }
    private void SubscribeAudioEvents()
    {
        _unit.SubscribeOnHitMomentEvent(PlayHurtAudio);
        _unit.SubscribeOnAttackEvent(PlayAttackAudio);
        _unit.SubscribeOnHealEvent(PlayHealAudio);
    }
    private void UnsubscribeAudioEvents()
    {
        _unit.UnsubscribeOnHitMomentEvent(PlayHurtAudio);
        _unit.UnsubscribeOnAttackEvent(PlayAttackAudio);
        _unit.UnsubscribeOnHealEvent(PlayHealAudio);
    }

    #endregion
}