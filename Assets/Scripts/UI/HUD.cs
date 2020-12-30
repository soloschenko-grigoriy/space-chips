using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour {
    public delegate void SkipClick();
    public delegate void MeleeAttackClick();
    public static event SkipClick OnSkipClick;
    public static event MeleeAttackClick OnMeleeAttackClick;

    [SerializeField] Button _meleeAttackButton = default;

    void Awake() {
        DeactivateMeleeAttackButton();
    }

    public void ActivateMeleeAttackButton() {
        _meleeAttackButton.interactable = true;
    }

    public void DeactivateMeleeAttackButton() {
        _meleeAttackButton.interactable = false;
    }

    public void Skip() {
        if (OnSkipClick != null) {
            OnSkipClick();
        }
    }

    public void MeleeAttack() {
        if (OnMeleeAttackClick != null) {
            OnMeleeAttackClick();
        }
    }
}
