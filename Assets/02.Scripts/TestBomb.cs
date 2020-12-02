using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestBomb : MonoBehaviour
{
    [SerializeField] EWeakType _weakType = EWeakType.None;
    [SerializeField] float _bombTime = 0.5f;
    [SerializeField] HitPad _hitPad = null;
    [SerializeField] bool _oneTarget = false;

    [SerializeField] string _targetTag;
    float[] _valeus;
    float _timeCheck = 0.0f;
    bool _dealCheck = true;

    private void Start()
    {
        if (_hitPad != null)
        {
            HitPad hitPad = Instantiate(_hitPad, transform.position, Quaternion.identity);
            hitPad.HitPadSetting(_targetTag, _valeus);
        }
    }

    private void Update()
    {
        _timeCheck += Time.deltaTime;
        if (_timeCheck >= _bombTime)
        {
            _dealCheck = false;
        }
    }

    public void BombSetting(string targetTag, params float[] values)
    {
        _targetTag = targetTag;
        _valeus = values;
    }

    void Bomb()
    {
        GameObject[] hitObjects = GameObject.FindGameObjectsWithTag(_targetTag);
        foreach(GameObject hitObject in hitObjects)
        {
            if (Vector3.Distance(transform.position, hitObject.transform.position) < _valeus[1])
            {
                if (_dealCheck)
                {
                    ObjectGame objectHit = hitObject.GetComponent<ObjectGame>();
                    objectHit.Hit((int)_valeus[0], _weakType);
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(_targetTag))
        {
            if (_oneTarget)
            {
                ObjectGame objectHit = other.GetComponent<ObjectGame>();
                objectHit.Hit((int)_valeus[0], _weakType);
                GetComponent<BoxCollider>().enabled = false;
            }
            else
            {
                GetComponent<BoxCollider>().enabled = false;
                Bomb();
            }
        }
    }
}
