using UnityEngine;
using UnityEngine.UI;

public class AttackMethodSelection : MonoBehaviour {
    public delegate void AttackMethodSelected(AttackMethod type);
    public static event AttackMethodSelected OnAttackMethodSelected;

    public delegate void AttackConfirmed();
    public static event AttackConfirmed OnAttackConfirmed;

    [SerializeField] Toggle _energyAttack = default;
    [SerializeField] Toggle _shieldAttack = default;
    [SerializeField] RectTransform _confirmContainer = default;

    void Awake() {
        _energyAttack.onValueChanged.AddListener(delegate {
            OnValueChanged(AttackMethod.Energy);
        });

        _shieldAttack.onValueChanged.AddListener(delegate {
            OnValueChanged(AttackMethod.Shield);
        });
    }

    void OnValueChanged(AttackMethod method) {
        var value = method == AttackMethod.Energy ? _energyAttack.isOn : _shieldAttack.isOn;
        _confirmContainer.gameObject.SetActive(value);

        if (OnAttackMethodSelected == null) {
            return;
        }

        if (value) {
            OnAttackMethodSelected(method);
        }
        else {
            OnAttackMethodSelected(AttackMethod.None);
        }
    }

    public void OnConfirm() {
        if (OnAttackConfirmed != null) {
            OnAttackConfirmed();
        }
    }

    public void Activate() {
        gameObject.SetActive(true);

    }

    public void Deactivate() {
        _energyAttack.SetIsOnWithoutNotify(false);
        _shieldAttack.SetIsOnWithoutNotify(false);

        gameObject.SetActive(false);
        _confirmContainer.gameObject.SetActive(false);
    }

}
