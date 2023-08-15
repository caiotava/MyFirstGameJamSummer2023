using UnityEngine;
using UnityEngine.Serialization;

public class BulletTarget : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb;
    private Unit unitOwner;
    private Unit unitTarget;
    
    public void Initialize(Unit unit, Unit target)
    {
        unitOwner = unit;
        unitTarget = target;

        tag = unitOwner.tag;

        var distance = transform.position - unitTarget.transform.position;
        var rotation = Mathf.Atan2(distance.y, distance.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, rotation));
    }
    
    public void Update()
    {
        if (unitTarget is null || unitTarget.Equals(null))
        {
            Destroy(gameObject, 6.0f);
            return;
        }

        var direction = (unitTarget.transform.position - transform.position).normalized;
        rb.velocity = direction * (Time.deltaTime * unitOwner.unitStats.projectileSpeed);
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.transform.tag == tag)
        {
            return;
        }

        var unitCollider = other.transform.GetComponent<Unit>();
        if (unitCollider == null)
        {
            return;
        }

        unitCollider.GiveDamage(unitOwner.unitStats.Attack);
        Destroy(transform.gameObject);
    }
}