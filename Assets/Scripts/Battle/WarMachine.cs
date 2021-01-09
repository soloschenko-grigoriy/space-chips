using UnityEngine;
using UnityEngine.UI;

public class WarMachine : MonoBehaviour {
    public bool isDead => isDead;
    public int Power => _power;

    [SerializeField] int _maxEnergy = 100;
    [SerializeField] int _maxShield = 75;
    [SerializeField] int _power = 12;
    [SerializeField] [Range(1, 10)] int _energyPenalty = 2;
    [SerializeField] StatsBar _statsBar = default;
    [SerializeField] AttackMethodSelection _attackMehtodSelection = default;

    int _currentShield, _currentEnergy, _currentPower;
    bool _isDead = false;

    void Awake() {
        _currentShield = _maxShield;
        _currentEnergy = _maxEnergy;
        _currentPower = _power;

        _statsBar.SetupEnergy(_maxEnergy, 0, _currentEnergy);
        _statsBar.SetupShield(_maxShield, 0, _currentShield);
        _statsBar.Power = _currentPower;

        HideStats();
        HideMethodSelection();
    }

    public void ShowStats() {
        _statsBar.gameObject.SetActive(true);
    }

    public void HideStats() {
        _statsBar.gameObject.SetActive(false);
    }

    public void ShowMethodSelection() {
        _attackMehtodSelection.Activate();
    }

    public void HideMethodSelection() {
        _attackMehtodSelection.Deactivate();
    }

    public void TakeDamage(AttackMethod method, int damage) {
        var stats = CalculateDamage(method, damage);

        _currentEnergy = stats.Energy;
        _currentShield = stats.Shield;
        _isDead = stats.IsDead;

        RefreshStats();
    }

    public void PreviewDamage(AttackMethod method, int damage) {
        var stats = CalculateDamage(method, damage);

        _statsBar.ShowPreview(stats.Energy, stats.Shield);
    }

    public void ClearPreviewDamage() {
        _statsBar.HidePreview();
        RefreshStats();
    }

    void RefreshStats() {
        _statsBar.Energy = _currentEnergy;
        _statsBar.Shield = _currentShield;
    }

    (int Energy, int Shield, bool IsDead) CalculateDamage(AttackMethod method, int damage) {
        (int Energy, int Shield, bool IsDead) result;
        switch (method) {
            case AttackMethod.Energy:
                result = CalculateDamageToEnergy(damage);
                break;
            case AttackMethod.Shield:
                result = CalculateDamageToShield(damage);
                break;
            default:
                Debug.LogError($"TakeDamageToShield {method} not supported");
                return (_currentEnergy, _currentShield, _isDead);
        }

        return result;
    }

    (int Energy, int Shield, bool IsDead) CalculateDamageToEnergy(int damage) {
        var energy = _currentEnergy - damage;
        var isDead = _isDead;
        if (energy < 1) {
            energy = 0;
            isDead = true;
        }

        return (energy, _currentShield, isDead);
    }

    (int Energy, int Shield, bool IsDead) CalculateDamageToShield(int damage) {
        var diff = _currentShield - damage;
        var shield = _currentShield;
        var energy = _currentEnergy;
        var isDead = _isDead;

        if (diff > 0) {
            shield -= damage;
        }
        else {
            shield = 0;
            var result = CalculateDamageToEnergy(-diff * _energyPenalty);
            energy = result.Energy;
            isDead = result.IsDead;
        }

        return (energy, shield, isDead);
    }
}
