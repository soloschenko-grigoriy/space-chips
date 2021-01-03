using UnityEngine;
using UnityEngine.UI;

public class StatsBar : MonoBehaviour {
    public float Energy {
        set {
            _energyFill.value = value;
            _energyLabel.text = value.ToString();
        }
    }

    public float Shield {
        set {
            _shieldFill.value = value;
            _shieldLabel.text = value.ToString();
        }
    }

    public int Power {
        set => _powerLabel.text = value.ToString();
    }

    [SerializeField] Slider _energyFill = default;
    [SerializeField] Text _energyLabel = default;
    [SerializeField] Slider _shieldFill = default;
    [SerializeField] Text _shieldLabel = default;
    [SerializeField] Text _powerLabel = default;


    public void SetupEnergy(int max, int min, int value) {
        _energyFill.maxValue = max;
        _energyFill.minValue = min;
        Energy = value;
    }

    public void SetupShield(int max, int min, int value) {
        _shieldFill.maxValue = max;
        _shieldFill.minValue = min;
        Shield = value;
    }
}
