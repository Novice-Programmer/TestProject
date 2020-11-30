using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestGhost : MonoBehaviour
{
    public EObjectType _objectType = EObjectType.None;
    public EObjectName _objectName = EObjectName.None;
    public int _installCost;
    public TestIntVector2 _demision;
    public TestIntVector2 _gridPos;
    public EFitType _towerFitType = EFitType.Overlaps;
    public Vector3 _fitPos;
    public TestTile _parentTile;
    [SerializeField] MeshRenderer[] _materialRenders = null;
    [SerializeField] Material _fitMaterial = null;
    [SerializeField] Material _outMaterial = null;
    [SerializeField] GameObject _rangeObject = null;

    private void Start()
    {
        if(_objectType== EObjectType.Tower)
        _rangeObject.transform.localScale *= ObjectDataManager.Instance.GetTowerData(_objectName).atkRange;
    }

    public void FitMaterialCheck(EFitType towerFitType)
    {
        switch (towerFitType)
        {
            case EFitType.Fits:
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
        for(int i = 0; i < _materialRenders.Length; i++)
        {
            _materialRenders[i].material = material;
        }
    }


}
