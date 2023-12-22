using UnityEngine;

public class Laser : MonoBehaviour
{
    [SerializeField] private float _laserMaxLenght;

    private LineRenderer _lineRenderer;

    private void Start()
    {
        _lineRenderer = GetComponent<LineRenderer>();
    }

    private void Update()
    {
        if (Physics.Raycast(transform.position, Vector3.left, out RaycastHit hit, _laserMaxLenght))
        {
            _lineRenderer.SetPosition(0, transform.position);
            _lineRenderer.SetPosition(1, hit.point);
        }
        else
        {
            _lineRenderer.SetPosition(0, transform.position);
            _lineRenderer.SetPosition(1, transform.position + Vector3.left * _laserMaxLenght);
        }
    }
}
