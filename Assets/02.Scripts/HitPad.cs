using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitPad : MonoBehaviour
{
    [SerializeField] EBadBuff _badBuffType = EBadBuff.None;
    TestBadBuff _badBuff = new TestBadBuff();
    string _targetTag;
    float _continueTime;
    float[] _values;

    float _timeCheck = 0.0f;

    private void Update()
    {
        _timeCheck += Time.deltaTime;
        if (_timeCheck >= _continueTime)
        {
            Destroy(gameObject);
        }
    }

    public void HitPadSetting(string targetTag, float[] values)
    {
        _targetTag = targetTag;
        _values = values;
        _continueTime = _values[2];
        switch (_badBuffType)
        {
            case EBadBuff.Radioactivity:
                _badBuff.remainTime = (int)_values[3];
                _badBuff.dotTime = 1.0f;
                _badBuff.hp = (int)_values[4];
                break;
        }
        _badBuff.type = _badBuffType;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(_targetTag))
        {
            ObjectHit objectHit = other.GetComponent<ObjectHit>();
            objectHit.BadBuff(_badBuff);
        }
    }
}
