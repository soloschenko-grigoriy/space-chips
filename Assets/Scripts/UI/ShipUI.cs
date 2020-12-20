using UnityEngine;
using UnityEngine.UI;

public class ShipUI : MonoBehaviour
{
    [SerializeField]
    private Text _energyText = default;

    // [SerializeField]
    // private RectTransform _energyFill = default;

    [SerializeField]
    private Text _shieldText = default;

    [SerializeField]
    // private RectTransform _shieldFill = default;

    private void Awake()
    {
        transform.SetParent(GameObject.Find("MyChips").transform);
    }

    public void UpdateShield(float v)
    {
        this._shieldText.text = v.ToString();
    }

    public void UpdateEnergy(float v)
    {
        this._energyText.text = v.ToString();
    }
}
