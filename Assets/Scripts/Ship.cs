using UnityEngine;

public class Ship : MonoBehaviour
{
    [SerializeField, Range(1f, 100f)]
    float _speed = 5f;
    Vector3 _targetPosition;
    float _timeElapsed = 0;

    void Awake()
    {
        _targetPosition = transform.localPosition;
    }

    void Update()
    {
        if (Vector3.Distance(transform.localPosition, _targetPosition) > 0.001)
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition, _targetPosition, _timeElapsed);
            _timeElapsed += Time.deltaTime * _speed / 1000;

            Vector3 relativePos = _targetPosition - transform.position;
            transform.rotation = Quaternion.LookRotation(relativePos, Vector3.up);
        }
        else
        {
            _targetPosition = transform.localPosition;
        }
    }

    public void MoveTo(Vector3 position)
    {
        _timeElapsed = 0;
        _targetPosition = position;
    }
}
