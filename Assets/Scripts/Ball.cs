using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    #region field;
    [SerializeField]
    ParticleSystem particle;
    [SerializeField]
    AudioSource audiosource;
    public float maxDamage = 100f;
    public float explosionForce = 1000f;
    public float lifeTime = 10f;
    public float explosionRadius = 20f;
    public LayerMask whatIsProp;

    #endregion
    void Start()
    {
        Destroy(gameObject, lifeTime);
    }
     
    private void OnTriggerEnter(Collider other)
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position,explosionRadius,whatIsProp);

        foreach (Collider collider in colliders)
        {
            Rigidbody targetRigidbody = collider.GetComponent<Rigidbody>();
            targetRigidbody.AddExplosionForce(explosionForce,transform.position,explosionRadius);
            targetRigidbody.GetComponent<Prop>().TakeDamage(CalculateDamage(collider.transform.position));
        }

        particle.transform.parent = null;
        particle.Play();
        audiosource.Play();
        Destroy(particle.gameObject, particle.duration);
        Destroy(gameObject);

    }

    /// <summary>
    /// ���� �߽����κ��� �Ÿ��� ���� ������ ��� 
    /// </summary>
    /// <param name="targetPosition"></param>
    /// <returns></returns>
    float CalculateDamage(Vector3 targetPosition)
    {
        float distance = Vector3.Distance(targetPosition,transform.position);
        distance = Mathf.Min(distance,explosionRadius); // �ݶ��̴��� �� ��ó�� ���� ���� ���
        float edgetToCenterDistance = explosionRadius - distance;
        float percentage = edgetToCenterDistance / explosionRadius;
        return (maxDamage * percentage);
    }


}
