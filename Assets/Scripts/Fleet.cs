using UnityEngine;

public class Fleet : MonoBehaviour
{
    [SerializeField, Range(1, 5)]
    private int _size = 1;

    [SerializeField]
    private Ship _shipPrefab = default;

    [SerializeField]
    private Transform[] _respawnPoints = default;

    private Ship[] _ships;

    private void Awake()
    {
        _ships = new Ship[_size];

        for (int i = 0; i < _size; i++)
        {
            _ships[i] =
                Instantiate(_shipPrefab)
                .Respawn(_respawnPoints[i].position);
        }

        _ships[0].Activate();
    }
}
