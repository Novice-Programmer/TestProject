using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ERotateType
{
    degree0,
    degree90,
    degree180,
    degree270
}

public class TestGhost : MonoBehaviour
{
    public EObjectType _objectType = EObjectType.None;
    public EObjectName _objectName = EObjectName.None;
    public int _installCost;
    public TestIntVector2 _demision;
    public TestIntVector2 _saveDemision;
    public TestIntVector2 _gridPos;
    public EFitType _fitType = EFitType.Overlaps;
    public Vector3 _fitPos;
    public TestTile _parentTile;
    public ERotateType _rotateType = ERotateType.degree0;
    [SerializeField] MeshRenderer[] _materialRenders = null;
    [SerializeField] Material _fitMaterial = null;
    [SerializeField] Material _outMaterial = null;
    [SerializeField] GameObject _rangeObject = null;

    private void Start()
    {
        _saveDemision = _demision;
        if (_objectType == EObjectType.Tower)
            _rangeObject.transform.localScale *= ObjectDataManager.Instance.GetTowerData(_objectName).atkRange;
    }

    public void FitMaterialCheck(EFitType towerFitType)
    {
        switch (towerFitType)
        {
            case EFitType.PXPYFits:
            case EFitType.MXPYFits:
            case EFitType.PXMYFits:
            case EFitType.MXMYFits:
            case EFitType.EXPYFits:
            case EFitType.EXMYFits:
            case EFitType.PXEYFits:
            case EFitType.MXEYFits:
            case EFitType.EXEYFits:
                MaterialApply(_fitMaterial);
                break;
            case EFitType.Overlaps:
                MaterialApply(_outMaterial);
                break;
            case EFitType.OutOfBounds:
                MaterialApply(_outMaterial);
                break;
        }
    }

    public void NoneCheck()
    {
        MaterialApply(_outMaterial);
    }

    void MaterialApply(Material material)
    {
        for (int i = 0; i < _materialRenders.Length; i++)
        {
            _materialRenders[i].material = material;
        }
    }

    public void RotateObject()
    {
        if (_rotateType == ERotateType.degree270)
        {
            _rotateType = ERotateType.degree0;
        }

        else
        {
            _rotateType++;
        }

        switch (_rotateType)
        {
            case ERotateType.degree0:
                transform.rotation = Quaternion.Euler(Vector3.zero);
                _demision = _saveDemision;
                break;
            case ERotateType.degree90:
                transform.rotation = Quaternion.Euler(new Vector3(0, 90, 0));
                _demision = new TestIntVector2(_saveDemision.y, _saveDemision.x);
                break;
            case ERotateType.degree180:
                transform.rotation = Quaternion.Euler(new Vector3(0, 180, 0));
                _demision = _saveDemision;
                break;
            case ERotateType.degree270:
                transform.rotation = Quaternion.Euler(new Vector3(0, -90, 0));
                _demision = new TestIntVector2(_saveDemision.y, _saveDemision.x);
                break;
        }
    }
}
