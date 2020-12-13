using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombEffect : MonoBehaviour
{
    [SerializeField] EWeakType _weakType = EWeakType.None;
    [SerializeField] float _bombTime = 0.5f;
    [SerializeField] HitField _hitField = null;
    [SerializeField] bool _oneTarget = false;
    [SerializeField] bool _oneShot = false;
    [SerializeField] bool _delayShot = false;
    [SerializeField] float _delayTime = 1;

    string _targetTag;
    float[] _valeus;
    float _timeCheck = 0.0f;
    bool _dealCheck = true;

    private void Start()
    {
        if (_hitField != null)
        {
            HitField hitPad = Instantiate(_hitField, transform.position, Quaternion.identity);
            hitPad.HitPadSetting(_targetTag, _valeus);
        }
    }

    private void Update()
    {
        _timeCheck += Time.deltaTime;
        if (_delayShot)
        {
            if (_timeCheck >= _delayTime)
            {
                _delayShot = false;
                GetComponent<BoxCollider>().enabled = true;
            }
        }
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
        if (string.IsNullOrEmpty(_targetTag))
        {
            GetComponent<BoxCollider>().enabled = false;
        }
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
                if (_oneShot)
                {
                    ObjectGame objectHit = other.GetComponent<ObjectGame>();
                    objectHit.Hit((int)_valeus[0], _weakType);
                }
                else
                {
                    GetComponent<BoxCollider>().enabled = false;
                    Bomb();
                }
            }
        }
    }
}
