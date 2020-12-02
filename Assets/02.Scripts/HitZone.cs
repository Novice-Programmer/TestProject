using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitZone : MonoBehaviour
{
    int _damage = 0;
    string _targetTag;

    public void HitZoneSetting(int damage,string targetTag)
    {
        _damage = damage;
        _targetTag = targetTag;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(_targetTag))
        {
            ObjectGame objectHit = other.GetComponent<ObjectGame>();
            objectHit.Hit(_damage,EWeakType.None);
            gameObject.SetActive(false);
        }
    }
}
