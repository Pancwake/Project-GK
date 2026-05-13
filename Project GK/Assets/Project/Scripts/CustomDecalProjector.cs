using UnityEngine;

public class CustomDecalProjector : MonoBehaviour
{
    public GameObject decalPrefab;
    GameObject decal;

    [SerializeField] LayerMask groundMask;

    public float surfaceOffset = 0.001f;

    private void Start()
    {
        decal = Instantiate(decalPrefab, transform.position, Quaternion.identity);

        if (Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit, 100f, groundMask))
        {
            MoveDecal(hit);
        }
    }

    private void Update()
    {
        ShootDecal();
    }

    void ShootDecal()
    {
        if (Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit, 100f, groundMask))
        {
            MoveDecal(hit);
        }
        else
        {

        }
    }

    void MoveDecal(RaycastHit hit)
    {
        Vector3 position = hit.point + Vector3.up * surfaceOffset;
        decal.transform.position = position;
    }

    private void OnDestroy()
    {
        Destroy(decal);
    }
}
