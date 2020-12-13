using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] float _maxTime = 30.0f;
    [Header("Status")]
    [SerializeField] BombEffect _bombObject = null;
    [SerializeField] ESoundName _launchSound = ESoundName.Rocket;
    [SerializeField] ESoundName _boomSound = ESoundName.RocketBomb;

    [SerializeField] float[] _values = null;

    [Header("TransValue")]
    [SerializeField] float _speed = 10f;
    [SerializeField] float _rotateSpeed = 15f;
    [SerializeField] bool _parabola = false;
    [SerializeField] float _upSpeed = 10f;
    [SerializeField] float _maxUp = 10.0f;
    [SerializeField] float _minY = 0.2f;

    Transform _target;
    Vector3 _targetPos;
    string _tagetTag;
    bool _guidance;

    float _timeCheck = 0.0f;

    private void Start()
    {
        SoundManager.Instance.PlayEffectSound(_launchSound, transform);
    }

    private void Update()
    {
        _timeCheck += Time.deltaTime;
        if (_timeCheck >= _maxTime)
        {
            BombEffect bomb = Instantiate(_bombObject, transform.position, transform.rotation);
            bomb.BombSetting(_tagetTag, _values);
            Destroy(gameObject);
        }
        if (_parabola)
        {
            if (transform.position.y > _maxUp)
            {
                _parabola = false;
                return;
            }
            transform.Translate(Vector3.up * _upSpeed * Time.deltaTime, Space.World);
        }
        else
        {
            Vector3 dir;
            if (_guidance)
            {
                if (_target == null)
                {
                    Bomb();
                    return;
                }
                dir = _target.position - transform.position;
                if (dir.y > -_minY)
                {
                    dir.y = 0;
                }
            }
            else
            {
                dir = _targetPos - transform.position;
            }

            transform.Translate(dir.normalized * _speed * Time.deltaTime, Space.World);
            transform.forward = Vector3.Lerp(transform.forward, dir.normalized, _rotateSpeed * Time.deltaTime);
        }
    }

    public void ProjectileSetting(Transform target, string targetTag, params float[] values)
    {
        _target = target;
        _guidance = true;
        _tagetTag = targetTag;
        _values = values;
    }

    public void ProjectileSetting(Vector3 targetPos, string targetTag, params float[] values)
    {
        _targetPos = targetPos;
        _guidance = false;
        _tagetTag = targetTag;
        _values = values;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Field"))
        {
            Bomb();
        }

        else if (string.IsNullOrEmpty(_tagetTag))
        {
            Bomb();
        }

        else if (other.CompareTag(_tagetTag))
        {
            Bomb();
        }
    }

    void Bomb()
    {
        SoundManager.Instance.PlayEffectSound(_boomSound, transform);
        BombEffect bomb = Instantiate(_bombObject, transform.position, transform.rotation);
        bomb.BombSetting(_tagetTag, _values);
        Destroy(gameObject);
    }
}
