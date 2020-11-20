using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ETouchMode
{
    Touch,
    TowerBuliding,
    ObstacleBuliding
}

public class TestInputManager : MonoBehaviour
{
    Camera _mainCamera;
    public static TestInputManager Instance { set; get; }
    public static ETouchMode TouchMode = ETouchMode.Touch;
    public static TestGhostTower BuildTower = null;
    public static TestTower SelectTower = null;
    [SerializeField] LayerMask _tileLayer = 0;

    private void Awake()
    {
        Instance = this;
        _mainCamera = GetComponent<Camera>();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(TouchMode == ETouchMode.TowerBuliding)
        {
            if (Input.GetMouseButton(1))
            {
                Destroy(BuildTower.gameObject);
                BuildTower = null;
                TouchMode = ETouchMode.Touch;
            }
            else
            {
                Ray ray = _mainCamera.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                int layerMask = 1 << LayerMask.NameToLayer("Tile");

                if (Physics.Raycast(ray, out hit, 100f, layerMask))
                {
                    Vector3 position = hit.point;
                    position.y -= 1;
                    BuildTower.transform.position = position;

                    TestNode node = hit.transform.GetComponent<TestNode>();

                    if(node._nodeType== ENodeType.TowerNode)
                    {
                        ETowerFitType towerFitType = node._parentTile.Fits(node._pos, BuildTower._demision);
                        BuildTower.FitMaterialCheck(towerFitType);
                        if(towerFitType == ETowerFitType.Fits)
                        {
                            Vector3 nodePosition = node._parentTile.NodeToPosition(node._pos,BuildTower._demision);
                            BuildTower.transform.position = nodePosition;
                            BuildTower.towerFitType = ETowerFitType.Fits;
                            BuildTower.fitPos = nodePosition;
                            if (Input.GetMouseButton(0))
                            {
                                node._parentTile.Occupy(node._pos, BuildTower._demision);
                                TestGameManager.Instance.TowerBulid(BuildTower);
                                Destroy(BuildTower.gameObject);
                                BuildTower = null;
                                TouchMode = ETouchMode.Touch;
                            }
                        }
                        else
                        {

                        }
                    }
                    else
                    {
                        BuildTower.NoneCheck();
                    }
                }
                else if (Physics.Raycast(ray, out hit, 100f))
                {
                    Vector3 position = hit.point;
                    BuildTower.transform.position = position;
                    BuildTower.NoneCheck();
                }
            }
        }
        else
        {
            Ray ray = _mainCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            int layerMask = 1 << LayerMask.NameToLayer("Tower");

            if(SelectTower != null)
            {
                if (Input.GetMouseButtonUp(1))
                {
                    SelectTower.TowerSelect(false);
                    SelectTower = null;
                }
            }

            if (Physics.Raycast(ray, out hit, 100f, layerMask))
            {
                if (Input.GetMouseButtonUp(0))
                {
                    TestTower tower = hit.transform.GetComponent<TestTower>();
                    if (tower._towerBulidSuccess)
                    {
                        if (SelectTower != null && SelectTower != tower)
                        {
                            SelectTower.TowerSelect(false);
                        }
                        tower.TowerSelect();
                        SelectTower = tower;
                    }
                }
            }
        }
    }

    public void InstallTower(ETowerType towerType,int installCost)
    {
        TouchMode = ETouchMode.TowerBuliding;
        TestGhostTower ghostTower = Instantiate(TestTowerDataManager.Instance.GetGhostTower(towerType));
        ghostTower.installCost = installCost;
        BuildTower = ghostTower;
    }

    public void TowerSelect()
    {

    }
}
