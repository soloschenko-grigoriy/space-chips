using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour {
    public delegate void SkipClick();
    public static event SkipClick OnSkipClick;
    public delegate void AttackSelected(AttackType type, bool value);
    public static event AttackSelected OnAttackSelected;

    [SerializeField] Toggle _meleeAttackToggle = default;
    [SerializeField] Toggle _rangeAttackToggle = default;
    [SerializeField] Button _skipButton = default;

    void Awake() {
        DeactivateMeleeAttackButton();
        DeactivateRangeAttackButton();
        DeactivateSkipButton();
    }

    public void ActivateSkipButton() {
        _skipButton.interactable = true;
    }

    public void DeactivateSkipButton() {
        _skipButton.interactable = false;
    }

    public void ActivateMeleeAttackButton() {
        _meleeAttackToggle.interactable = true;
    }

    public void ActivateRangeAttackButton() {
        _rangeAttackToggle.interactable = true;
    }

    public void DeactivateMeleeAttackButton() {
        _meleeAttackToggle.interactable = false;
        _meleeAttackToggle.isOn = false;
    }

    public void DeactivateRangeAttackButton() {
        _rangeAttackToggle.interactable = false;
        _rangeAttackToggle.isOn = false;
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

        if (OnAttackSelected == null) {
            return;
        }

        OnAttackSelected(AttackType.Melee, _meleeAttackToggle.isOn);
    }

    public void RangeToggleChangedValue() {
        if (!_rangeAttackToggle.interactable) {
            return;
        }

        if (OnAttackSelected == null) {
            return;
        }

        OnAttackSelected(AttackType.Range, _rangeAttackToggle.isOn);
    }
}
