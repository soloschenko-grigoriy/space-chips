using UnityEngine;
using UnityEngine.UI;

public class StatsBar : MonoBehaviour {
    public float Energy {
        get => _energyFill.value;
        set {
            _energyFill.value = value;
            _energyLabel.text = value.ToString();
        }
    }

    public float Shield {
        get => _shieldFill.value;
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

    [SerializeField] Slider _energyPreviewFill = default;
    [SerializeField] Slider _shieldPreviewFill = default;

    public void SetupEnergy(int max, int min, int value) {
        _energyFill.maxValue = max;
        _energyFill.minValue = min;

        _energyPreviewFill.maxValue = max;
        _energyPreviewFill.minValue = min;

        Energy = value;
    }

    public void SetupShield(int max, int min, int value) {
        _shieldFill.maxValue = max;
        _shieldFill.minValue = min;

        _shieldPreviewFill.maxValue = max;
        _shieldPreviewFill.minValue = min;

        Shield = value;
    }

    public void ShowPreview(int energy, int shield) {
        _energyPreviewFill.value = Energy;
        _shieldPreviewFill.value = Shield;

        Energy = energy;
        Shield = shield;

        _energyPreviewFill.gameObject.SetActive(true);
        _shieldPreviewFill.gameObject.SetActive(true);
    }

    public void HidePreview() {
        Energy = _energyPreviewFill.value;
        Shield = _shieldPreviewFill.value;

        _energyPreviewFill.gameObject.SetActive(false);
        _shieldPreviewFill.gameObject.SetActive(false);
    }
}
