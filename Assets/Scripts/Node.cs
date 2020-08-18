using UnityEngine;

public class Node : MonoBehaviour
{
    private bool _clicked = false;

    public void Click()
    {
        _clicked = true;
    }

    void OnDrawGizmos()
    {
        if (!_clicked)
        {
            return;
        }

        Gizmos.DrawWireCube(transform.position, new Vector3(1, 0.001f, 1));
    }
}
