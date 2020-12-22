// using UnityEngine;
// using UnityEngine.AI;

// [RequireComponent(typeof(NavMeshAgent))]
// public class Ship : MonoBehaviour
// {
//     [SerializeField]
//     private float _moveRange = 5f;

//     [SerializeField]
//     private ShipUI _uiPrefab = default;

//     private NavMeshAgent _agent;

//     private NavMeshObstacle _obstacle;

//     private ShipUI _ui;

//     public bool IsActive { get; private set; } = false;

//     private float _shield = 25f;

//     // private float _maxShield = 100f;

//     private float _energy = 25f;

//     private float Energy
//     {
//         get
//         {
//             return _energy;
//         }

//         set
//         {
//             _ui.UpdateEnergy(value);
//             _energy = value;
//         }

//     }

//     private float Shield
//     {
//         get
//         {
//             return _shield;
//         }

//         set
//         {
//             _ui.UpdateShield(value);
//             _shield = value;
//         }
//     }

//     // private float _maxEnergy = 100f;

//     private void Awake()
//     {
//         _agent = GetComponent<NavMeshAgent>();
//         _obstacle = GetComponent<NavMeshObstacle>();
//         _ui = Instantiate(_uiPrefab);
//     }

//     private void Update()
//     {
//         if (!IsActive)
//         {
//             return;
//         }

//         if (Input.GetMouseButtonDown(0))
//         {
//             OnClick();
//             Energy = 10f;
//         }
//     }

//     public Ship Respawn(Vector3 postition)
//     {
//         transform.position = postition;
//         _agent.enabled = false;
//         _obstacle.enabled = true;

//         return this;
//     }

//     public Ship Activate()
//     {
//         IsActive = true;
//         _obstacle.enabled = false;
//         _agent.enabled = true;

//         return this;
//     }

//     public Ship Deactivate()
//     {
//         IsActive = false;
//         _agent.enabled = false;
//         _obstacle.enabled = true;

//         return this;
//     }

//     private void OnClick()
//     {
//         RaycastHit hit;
//         Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

//         if (Physics.Raycast(ray, out hit))
//         {
//             float distance = Vector3.Distance(hit.point, transform.position);
//             if (distance <= _moveRange)
//             {
//                 MoveTo(hit.point);
//             }
//         }
//     }

//     private void MoveTo(Vector3 point)
//     {
//         _agent.SetDestination(point);
//         transform.LookAt(point);
//     }

//     private void OnDrawGizmos()
//     {
//         UnityEditor.Handles.color = Color.yellow;
//         UnityEditor.Handles.DrawWireDisc(
//             new Vector3(transform.position.x, 0, transform.position.z),
//             Vector3.down,
//             _moveRange
//         );
//         // Gizmos.DrawWireSphere(transform.position, _moveRange);
//     }
// }
