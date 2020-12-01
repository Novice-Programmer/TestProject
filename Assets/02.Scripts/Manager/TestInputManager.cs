using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ETouchMode
{
    Touch,
    TowerBuilding,
    ObstacleBuilding
}

public class TestInputManager : MonoBehaviour
{
    Camera _mainCamera;
    public static TestInputManager Instance { set; get; }
    public static ETouchMode TouchMode = ETouchMode.Touch;
    public static TestGhost BuildGhost = null;
    public static TestTower SelectTower = null;

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
    [SerializeField] float _doubleTouchTime = 0.5f;

    Vector3 _touchPos = Vector3.zero;
    int _touchX = 0;
    int _touchY = 0;
    bool _doubleTouchCheck = false;

    float _timeCheck = 0;

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
        _timeCheck += Time.deltaTime;
        if (_doubleTouchCheck && _timeCheck >= _doubleTouchTime)
        {
            _doubleTouchCheck = false;
        }
        if (TouchMode == ETouchMode.TowerBuilding || TouchMode == ETouchMode.ObstacleBuilding)
        {
            if (Input.GetMouseButton(1))
            {
                Destroy(BuildGhost.gameObject);
                BuildGhost = null;
                TouchMode = ETouchMode.Touch;
            }
            else
            {
                Ray ray = _mainCamera.ScreenPointToRay(Input.mousePosition);
                int layerMask = 1 << LayerMask.NameToLayer("Tile");

                if (Physics.Raycast(ray, out RaycastHit hit, 100f, layerMask))
                {
                    Vector3 position = hit.point;
                    position.y -= 1;
                    BuildGhost.transform.position = position;

                    TestNode node = hit.transform.GetComponent<TestNode>();

                    if (TouchMode == ETouchMode.TowerBuilding && node._nodeType == ENodeType.TowerNode)
                    {
                        EFitType fitType = node._parentTile.Fits(node._pos, BuildGhost._demision);
                        BuildGhost.FitMaterialCheck(fitType);
                        if (fitType != EFitType.OutOfBounds && fitType != EFitType.Overlaps)
                        {
                            Vector3 nodePosition = node._parentTile.NodeToPosition(node._pos, BuildGhost._demision, fitType);
                            BuildGhost.transform.position = nodePosition;
                            BuildGhost._fitType = fitType;
                            BuildGhost._fitPos = nodePosition;
                            if (Input.GetMouseButton(0))
                            {
                                node._parentTile.Occupy(node._pos, BuildGhost._demision, fitType);
                                BuildGhost._parentTile = node._parentTile;
                                BuildGhost._gridPos = node._pos;
                                TestGameManager.Instance.TowerBuild(BuildGhost);
                                Destroy(BuildGhost.gameObject);
                                BuildGhost = null;
                                TouchMode = ETouchMode.Touch;
                            }
                        }
                        else
                        {
                            BuildGhost.NoneCheck();
                        }
                    }
                    else if (TouchMode == ETouchMode.ObstacleBuilding && node._nodeType == ENodeType.ObstacleNode)
                    {
                        EFitType fitType = node._parentTile.Fits(node._pos, BuildGhost._demision);
                        BuildGhost.FitMaterialCheck(fitType);
                        if (fitType != EFitType.OutOfBounds && fitType != EFitType.Overlaps)
                        {
                            Vector3 nodePosition = node._parentTile.NodeToPosition(node._pos, BuildGhost._demision, fitType);
                            BuildGhost.transform.position = nodePosition;
                            BuildGhost._fitType = fitType;
                            BuildGhost._fitPos = nodePosition;
                            if (Input.GetMouseButton(0))
                            {
                                node._parentTile.Occupy(node._pos, BuildGhost._demision, fitType);
                                BuildGhost._parentTile = node._parentTile;
                                BuildGhost._gridPos = node._pos;
                                TestGameManager.Instance.ObstacleBuild(BuildGhost);
                                Destroy(BuildGhost.gameObject);
                                BuildGhost = null;
                                TouchMode = ETouchMode.Touch;
                            }
                        }
                        else
                        {
                            BuildGhost.NoneCheck();
                        }
                    }
                    else
                    {
                        BuildGhost.NoneCheck();
                    }
                }
                else if (Physics.Raycast(ray, out hit, 100f))
                {
                    Vector3 position = hit.point;
                    BuildGhost.transform.position = position;
                    BuildGhost.NoneCheck();
                }

                if (Input.GetKeyUp(KeyCode.R))
                {
                    BuildGhost.RotateObject();
                }

                if (Input.mousePosition.x >= Screen.width - _panBorderThicknessX)
                {
                    if (transform.position.x + _panSpeed * Time.deltaTime <= _maxX)
                        transform.Translate(Vector3.right * _panSpeed * Time.deltaTime, Space.World);
                    else
                        transform.position = new Vector3(_maxX, transform.position.y, transform.position.z);
                }

                if (Input.mousePosition.x <= _panBorderThicknessX)
                {
                    if (transform.position.x - _panSpeed * Time.deltaTime >= _minX)
                        transform.Translate(Vector3.left * _panSpeed * Time.deltaTime, Space.World);
                    else
                        transform.position = new Vector3(_minX, transform.position.y, transform.position.z);
                }

                if (Input.mousePosition.y >= Screen.height - _panBorderThicknessY)
                {
                    if (transform.position.z + _panSpeed * Time.deltaTime <= _maxY)
                        transform.Translate(Vector3.forward * _panSpeed * Time.deltaTime, Space.World);
                    else
                        transform.position = new Vector3(transform.position.x, transform.position.y, _maxY);
                }
                if (Input.mousePosition.y <= _panBorderThicknessY)
                {
                    if (transform.position.z - _panSpeed * Time.deltaTime >= _minY)
                        transform.Translate(Vector3.back * _panSpeed * Time.deltaTime, Space.World);
                    else
                        transform.position = new Vector3(transform.position.x, transform.position.y, _minY);
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
                    if (tower._towerBuildSuccess)
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
                            if (transform.position.x > _maxX)
                                transform.position = new Vector3(_maxX, transform.position.y, transform.position.z);
                            if (transform.position.x < _minX)
                                transform.position = new Vector3(_minX, transform.position.y, transform.position.z);
                            if (transform.position.z > _maxY)
                                transform.position = new Vector3(transform.position.x, transform.position.y, _maxY);
                            if (transform.position.z < _minY)
                                transform.position = new Vector3(transform.position.x, transform.position.y, _minY);
                            return;
                        }
                    }

                    _touchPos = Input.mousePosition;
                    _doubleTouchCheck = _touchX != 0 || _touchY != 0;

                    if (_doubleTouchCheck)
                        _timeCheck = 0;
                }
            }
        }
    }

    public void Install(EObjectType objectType, EObjectName objectName, int installCost)
    {
        if (objectType == EObjectType.Tower)
            TouchMode = ETouchMode.TowerBuilding;
        else if (objectType == EObjectType.Obstacle)
            TouchMode = ETouchMode.ObstacleBuilding;
        TestGhost ghost = Instantiate(ObjectDataManager.Instance.GetBuildGhost(objectName));
        ghost._installCost = installCost;
        BuildGhost = ghost;
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
