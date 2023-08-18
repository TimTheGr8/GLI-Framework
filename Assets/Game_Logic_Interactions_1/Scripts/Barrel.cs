using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barrel : MonoBehaviour
{
    [SerializeField]
    private float _explosionRadius = 2.5f;
    [SerializeField]
    private GameObject _explosionPrefab;
    
    void Start()
    {
        
    }
    
    public void Explode()
    {
        Vector3 location = this.transform.position;
        Instantiate(_explosionPrefab, new Vector3(location.x, location.y + 1.5f, location.z), Quaternion.identity);
        Collider[] hitColliders = Physics.OverlapSphere(this.transform.position, _explosionRadius);
        foreach (var collider in hitColliders)
        {
            if(collider.tag == "Clown")
            {
                AI clown = collider.GetComponent<AI>();
                clown.StartDeath();
            }
        }
        Destroy(this.gameObject);
    }
}
