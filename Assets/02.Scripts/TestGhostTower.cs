using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestGhostTower : MonoBehaviour
{
    public ETowerType _towerType = ETowerType.None;
    public int installCost;
    public TestIntVector2 _demision;
    public ETowerFitType towerFitType = ETowerFitType.Overlaps;
    public Vector3 fitPos;
    [SerializeField] MeshRenderer[] _materialRenders = null;
    [SerializeField] Material _fitMaterial = null;
    [SerializeField] Material _outMaterial = null;
    [SerializeField] GameObject _rangeObject = null;

    private void Start()
    {
        _rangeObject.transform.localScale *= TestTowerDataManager.Instance.GetTowerData(_towerType).atkRange;
    }

    public void FitMaterialCheck(ETowerFitType towerFitType)
    {
        switch (towerFitType)
        {
            case ETowerFitType.Fits:
                MaterialApply(_fitMaterial);
                break;
            case ETowerFitType.Overlaps:
                MaterialApply(_outMaterial);
                break;
            case ETowerFitType.OutOfBounds:
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
