using Application;
using DifficultySystem;
using System.Collections.Generic;
using UnityEngine;

namespace Level
{
    [DisallowMultipleComponent]
    public sealed class Generator : MonoBehaviour
    {
        #region Editor fields
        [SerializeField] private Camera _camera = null;
        [SerializeField] private Controller _controller = null;

        [SerializeField] private GameObject _gate = null;
        [SerializeField] private Transform _gateParent = null;
        [SerializeField] private Transform _pipeParent = null;
        [SerializeField] private GameObject _ground = null;
        [SerializeField] private Transform _groundParent = null;

        [SerializeField, Min(0.0f)] private float _groundScaler = 1.571428571428571f;

        [SerializeField] private float _minimumHeight = -3.0f;
        [SerializeField] private float _maximumHeight = 4.0f;
        #endregion

        #region Fields
        private DifficultyData _difficultyData;
        private List<GameObject> _groundTiles = null, _pipes = null;
        private List<Gate> _gates = null;

        private bool _isStopped;
        private float _worldHeight, _worldWidth;
        private float _groundSpawnTime, _groundSpawnTimer, _pipeSpawnTimer;
        private float _gateHeight;

        private const int InitialCountOfGround = 2;
        private const float GroundHeight = 1.0f;
        #endregion

        #region MonoBehaviour API
        private void Awake()
        {
            float aspect = (float)Screen.width / Screen.height;
            _worldHeight = _camera.orthographicSize * 2;
            _worldWidth = _worldHeight * aspect;

            _groundTiles = new List<GameObject>();

            Vector2 spawnPosition = new Vector2(0.0f, -(_camera.orthographicSize - (GroundHeight / 2)));

            for (int index = 0; index < InitialCountOfGround; index++)
            {
                spawnPosition.x = _worldWidth * index;

                GameObject ground = Instantiate(_ground, spawnPosition, Quaternion.identity, _groundParent);
                _groundTiles.Add(ground);

                NewScale(ground, GroundHeight, _groundScaler);
            }
        }

        private void Start()
        {
            _pipes = new List<GameObject>();
            _gates = new List<Gate>();

            _isStopped = true;
        }

        private void Update()
        {
            if (_isStopped) return;

            UpdateObjectsPlace();

            _groundSpawnTimer -= Time.deltaTime;
            if (_groundSpawnTimer <= 0.0f)
            {
                SpawnGroundTile();

                _groundSpawnTimer = _groundSpawnTime;
            }

            _pipeSpawnTimer -= Time.deltaTime;
            if (_pipeSpawnTimer <= 0.0f)
            {
                SpawnPipeTile();

                UpdateGateHeight();

                _pipeSpawnTimer = _difficultyData.PipeSpawnDelay;
            }
        }
        #endregion

        #region Methods
        private void SpawnPipeTile()
        {
            //top pipe
            float y = _camera.orthographicSize + (_gateHeight + (_difficultyData.GateHeight / 2));

            Vector2 position = new Vector2(_worldWidth, y / 2);
            GameObject topPipe = Instantiate(_difficultyData.TopPipe, position, Quaternion.identity, _pipeParent);
            _pipes.Add(topPipe);

            float size = _camera.orthographicSize - (_gateHeight + (_difficultyData.GateHeight / 2));
            NewScale(topPipe, size);

            //bottom pipe
            float bottomPoint = -_camera.orthographicSize + GroundHeight;
            y = bottomPoint + (_gateHeight - (_difficultyData.GateHeight / 2));

            position = new Vector2(_worldWidth, y / 2);
            GameObject bottomPipe = Instantiate(_difficultyData.BottomPipe, position, Quaternion.identity, _pipeParent);
            _pipes.Add(bottomPipe);

            size = Mathf.Abs(bottomPoint - (_gateHeight - (_difficultyData.GateHeight / 2)));
            NewScale(bottomPipe, size);

            //gates
            position = new Vector2(_worldWidth, _gateHeight);
            GameObject gate = Instantiate(_gate, position, Quaternion.identity, _gateParent);

            if (gate.TryGetComponent(out BoxCollider2D boxCollider2D))
            {
                boxCollider2D.size = new Vector2(boxCollider2D.size.x, _difficultyData.GateHeight);
            }

            if (gate.TryGetComponent(out Gate scoreGate))
            {
                scoreGate.Init(_controller);
                _gates.Add(scoreGate);
            }
        }

        private void UpdateGateHeight()
        {
            float min = _gateHeight - (_difficultyData.GateHeight / 2);
            float max = _gateHeight + (_difficultyData.GateHeight / 2);
            float temp = Random.Range(min, max);

            _gateHeight = Mathf.Clamp(temp, _minimumHeight, _maximumHeight);
        }

        private void SpawnGroundTile()
        {
            Vector2 spawnPosition = new Vector2(_worldWidth, -(_camera.orthographicSize - (GroundHeight / 2)));

            GameObject groundTile = Instantiate(_ground, spawnPosition, Quaternion.identity, _groundParent);
            NewScale(groundTile, GroundHeight, _groundScaler);
            _groundTiles.Add(groundTile);
        }

        private void UpdateObjectsPlace()
        {
            foreach (GameObject item in _groundTiles)
            {
                item.transform.Translate(_difficultyData.LevelObjectsSpeed * Time.deltaTime * Vector2.left);
            }

            foreach (GameObject item in _pipes)
            {
                item.transform.Translate(_difficultyData.LevelObjectsSpeed * Time.deltaTime * Vector2.left);
            }

            foreach (Gate item in _gates)
            {
                item.transform.Translate(_difficultyData.LevelObjectsSpeed * Time.deltaTime * Vector2.left);
            }
        }

        private void NewScale(GameObject theGameObject, float newSize)
        {
            float size = theGameObject.GetComponent<Renderer>().bounds.size.y;

            Vector3 rescale = theGameObject.transform.localScale;

            rescale.y = newSize * rescale.y / size;

            theGameObject.transform.localScale = rescale;
        }

        private void NewScale(GameObject theGameObject, float newSize, float scale)
        {
            float size = theGameObject.GetComponent<Renderer>().bounds.size.y;

            Vector3 rescale = theGameObject.transform.localScale;

            rescale.y = newSize * rescale.y / size;
            rescale.x = rescale.y * scale;

            theGameObject.transform.localScale = rescale;
        }
        #endregion

        #region Public API
        internal void RemoveGateTile(Gate gate) => _gates.Remove(gate);

        internal void RemoveGroundTile(GameObject gameObject) => _groundTiles.Remove(gameObject);

        internal void RemovePipe(GameObject gameObject) => _pipes.Remove(gameObject);

        internal void Launch()
        {
            _difficultyData = Bootstrapper.Instance.GameDifficultySystem.GetDifficultyData();

            _groundSpawnTime = _worldWidth / _difficultyData.LevelObjectsSpeed;
            _groundSpawnTimer = _groundSpawnTime;

            _pipeSpawnTimer = _difficultyData.PipeSpawnDelay;
            _gateHeight = 0.0f;

            _isStopped = false;
        }

        internal void Stop()
        {
            foreach (Gate gate in _gates)
            {
                gate.Stop();
            }

            _isStopped = true;
        }

        internal void ResetGenerator()
        {
            foreach(Gate gate in _gates)
            {
                Destroy(gate.gameObject);
            }
            _gates.Clear();

            foreach (GameObject item in _groundTiles)
            {
                Destroy(item);
            }
            _groundTiles.Clear();

            foreach (GameObject item in _pipes)
            {
                Destroy(item);
            }
            _pipes.Clear();

            Vector2 spawnPosition = new Vector2(0.0f, -(_camera.orthographicSize - (GroundHeight / 2)));

            for (int index = 0; index < InitialCountOfGround; index++)
            {
                spawnPosition.x = _worldWidth * index;

                GameObject ground = Instantiate(_ground, spawnPosition, Quaternion.identity, _groundParent);
                _groundTiles.Add(ground);

                NewScale(ground, GroundHeight, _groundScaler);
            }
        }
        #endregion
    }
}