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

    [Header("CameraMove")]
    [SerializeField] float _minX = 0;
    [SerializeField] float _maxX = 0;
    [SerializeField] float _minY = 0;
    [SerializeField] float _maxY = 0;
    [SerializeField] float _panSpeed = 10.0f;
    [SerializeField] float _panBorderThicknessX = 100.0f;
    [SerializeField] float _panBorderThicknessY = 100.0f;
    [SerializeField] float _touchBorderThicknessX = 150.0f;
    [SerializeField] float _touchBorderThicknessY = 150.0f;
    [SerializeField] float _touchPanMoveLength = 5.0f;

    Vector3 _touchPos = Vector3.zero;
    int _touchX = 0;
    int _touchY = 0;
    bool _doubleTouchCheck = false;

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
        if (TouchMode == ETouchMode.TowerBuliding)
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

                    if (node._nodeType == ENodeType.TowerNode)
                    {
                        ETowerFitType towerFitType = node._parentTile.Fits(node._pos, BuildTower._demision);
                        BuildTower.FitMaterialCheck(towerFitType);
                        if (towerFitType == ETowerFitType.Fits)
                        {
                            Vector3 nodePosition = node._parentTile.NodeToPosition(node._pos, BuildTower._demision);
                            BuildTower.transform.position = nodePosition;
                            BuildTower.towerFitType = ETowerFitType.Fits;
                            BuildTower.fitPos = nodePosition;
                            if (Input.GetMouseButton(0))
                            {
                                node._parentTile.Occupy(node._pos, BuildTower._demision);
                                BuildTower.parentTile = node._parentTile;
                                BuildTower._gridPos = node._pos;
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

                if (Input.mousePosition.x >= Screen.width - _panBorderThicknessX)
                {
                    transform.Translate(Vector3.right * _panSpeed * Time.deltaTime, Space.World);
                }
                if (Input.mousePosition.x <= _panBorderThicknessX)
                {
                    transform.Translate(Vector3.left * _panSpeed * Time.deltaTime, Space.World);
                }

                if (Input.mousePosition.y >= Screen.height - _panBorderThicknessY)
                {
                    transform.Translate(Vector3.forward * _panSpeed * Time.deltaTime, Space.World);
                }
                if (Input.mousePosition.y <= _panBorderThicknessY)
                {
                    transform.Translate(Vector3.back * _panSpeed * Time.deltaTime, Space.World);
                }
            }
        }
        else
        {
            Ray ray = _mainCamera.ScreenPointToRay(Input.mousePosition);
            int towerLayerMask = 1 << LayerMask.NameToLayer("Tower");

            if (SelectTower != null)
            {
                if (Input.GetMouseButtonUp(1))
                {
                    SelectTower.TowerSelect(false);
                    SelectTower = null;
                }
            }

            if (Physics.Raycast(ray, out RaycastHit hit, 50f, towerLayerMask))
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

            else
            {
                if (Input.GetMouseButtonUp(0))
                {
                    if (Input.mousePosition.x >= Screen.width - _touchBorderThicknessX)
                    {
                        _touchX = 1;
                    }
                    else if (Input.mousePosition.x <= _touchBorderThicknessX)
                    {
                        _touchX = -1;
                    }
                    else
                    {
                        _touchX = 0;
                    }
                    if (Input.mousePosition.y >= Screen.height - _touchBorderThicknessY)
                    {
                        _touchY = 1;
                    }
                    else if (Input.mousePosition.y <= _touchBorderThicknessY)
                    {
                        _touchY = -1;
                    }
                    else
                    {
                        _touchY = 0;
                    }

                    if (_doubleTouchCheck)
                    {
                        Vector2 prevTouch = new Vector2(_touchPos.x, _touchPos.y);
                        Vector2 nowTouch = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
                        _doubleTouchCheck = false;
                        if (Vector2.Distance(prevTouch, nowTouch) < 10.0f)
                        {
                            transform.Translate((Vector3.forward * _touchY + Vector3.right * _touchX) * _touchPanMoveLength, Space.World);
                            return;
                        }
                    }

                    _touchPos = Input.mousePosition;
                    _doubleTouchCheck = _touchX != 0 || _touchY != 0;
                }
            }
        }
    }

    public void InstallTower(ETowerType towerType, int installCost)
    {
        TouchMode = ETouchMode.TowerBuliding;
        TestGhostTower ghostTower = Instantiate(TestTowerDataManager.Instance.GetGhostTower(towerType));
        ghostTower.installCost = installCost;
        BuildTower = ghostTower;
    }

    public void TowerSelectClose()
    {
        if (SelectTower != null)
        {
            SelectTower.TowerSelect(false);
            SelectTower = null;
        }
    }

    public void UITouch()
    {
        _doubleTouchCheck = false;
    }
}
