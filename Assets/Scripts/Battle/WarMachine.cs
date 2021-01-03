using UnityEngine;

public class WarMachine : MonoBehaviour {
    public bool isDead => isDead;

    [SerializeField] int _maxEnergy = 100;
    [SerializeField] int _maxShield = 75;
    [SerializeField] int _power = 12;
    [SerializeField] [Range(1, 10)] int _energyPenalty = 2;
    [SerializeField] StatsBar _statsBar = default;

    int _currentShield, _currentEnergy, _currentPower;
    bool _isDead = false;

    void Awake() {
        _statsBar.SetupEnergy(_maxEnergy, 0, 50);
        _statsBar.SetupShield(_maxShield, 0, 25);
        _statsBar.Power = _power;

        HideStats();
    }

    public void ShowStats() {
        _statsBar.gameObject.SetActive(true);
    }

    public void HideStats() {
        _statsBar.gameObject.SetActive(false);
    }

    public void TakeDamageToEnergy(int damage) {
        _currentEnergy -= damage;
        if (_currentEnergy < 1) {
            _isDead = true;
        }
    }

    public void TakeDamageToShield(int damage) {
        var diff = _currentShield - damage;
        if (diff > 0) {
            _currentShield -= damage;
        }
        else {
            _currentShield = 0;
            TakeDamageToEnergy(diff * _energyPenalty);
        }
    }
}
