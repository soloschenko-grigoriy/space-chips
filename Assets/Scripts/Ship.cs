using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class Ship : MonoBehaviour
{
    [SerializeField]
    float _moveRange = 5f;
    NavMeshAgent _agent = default;

    void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            OnClick();
        }
    }

    private void OnClick()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit))
        {
            float distance = Vector3.Distance(hit.point, transform.position);
            if (distance <= _moveRange)
            {
                MoveTo(hit.point);
            }
        }
    }

    private void MoveTo(Vector3 point)
    {
        _agent.SetDestination(point);
        transform.LookAt(point);
    }

    private void OnDrawGizmos()
    {
        UnityEditor.Handles.color = Color.yellow;
        UnityEditor.Handles.DrawWireDisc(
            new Vector3(transform.position.x, 0, transform.position.z),
            Vector3.down,
            _moveRange
        );
        // Gizmos.DrawWireSphere(transform.position, _moveRange);
    }
}
