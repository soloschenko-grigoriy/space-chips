using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour {
    public delegate void SkipClick();
    public static event SkipClick OnSkipClick;
    public delegate void MeleeAttackSelected(bool value);

    public static event MeleeAttackSelected OnMeleeAttackSelected;

    [SerializeField] Toggle _meleeAttackToggle = default;

    void Awake() {
        DeactivateMeleeAttackButton();
    }

    public void ActivateMeleeAttackButton() {
        _meleeAttackToggle.interactable = true;
    }

    public void DeactivateMeleeAttackButton() {
        _meleeAttackToggle.interactable = false;
        _meleeAttackToggle.isOn = false;
    }

    public void Skip() {
        if (OnSkipClick != null) {
            OnSkipClick();
        }
    }

    public void MeleeToggleChangedValue() {
        if (!_meleeAttackToggle.interactable) {
            return;
        }

        if (OnMeleeAttackSelected == null) {
            return;
        }

        OnMeleeAttackSelected(_meleeAttackToggle.isOn);
    }
}
