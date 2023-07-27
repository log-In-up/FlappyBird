using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LevelGenerator
{
    [DisallowMultipleComponent]
    public sealed class Generator : MonoBehaviour
    {
        #region Editor fields
        [SerializeField] private Camera _camera = null;

        [SerializeField] private GameObject _bottomPipe = null;
        [SerializeField] private GameObject _topPipe = null;
        [SerializeField] private Transform _pipeParent = null;
        [SerializeField, Min(0.0f)] private float _spawnDelay = 1.0f;
        [SerializeField, Min(0.0f)] private float _gateDelta = 1.0f;

        [SerializeField] private GameObject _ground = null;
        [SerializeField] private Transform _groundParent = null;
        [SerializeField, Min(0.0f)] private float _groundScaler = 1.571428571428571f;

        [SerializeField, Min(0.0f)] private float _levelObjectsSpeed = 2.5f;
        #endregion

        #region Fields
        private List<GameObject> _groundTiles = null, _pipes = null;

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
            _pipes = new List<GameObject>();

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
            _groundSpawnTime = _worldWidth / _levelObjectsSpeed;
            _groundSpawnTimer = _groundSpawnTime;

            _pipeSpawnTimer = _spawnDelay;
            _gateHeight = 0.0f;
        }

        private void Update()
        {
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

                _pipeSpawnTimer = _spawnDelay;
            }
        }
        #endregion

        #region Methods
        private void SpawnPipeTile()
        {
            //top pipe
            float y = _camera.orthographicSize + (_gateHeight + (_gateDelta / 2));

            Vector2 position = new Vector2(_worldWidth, y / 2);
            GameObject topPipe = Instantiate(_topPipe, position, Quaternion.identity, _pipeParent);
            _pipes.Add(topPipe);

            float size = _camera.orthographicSize - (_gateHeight + (_gateDelta / 2));
            NewScale(topPipe, size);

            //bottom pipe
            float bottomPoint = -_camera.orthographicSize + GroundHeight;
            y = bottomPoint + (_gateHeight - (_gateDelta / 2));

            position = new Vector2(_worldWidth, y / 2);
            GameObject bottomPipe = Instantiate(_bottomPipe, position, Quaternion.identity, _pipeParent);
            _pipes.Add(bottomPipe);

            size = Mathf.Abs(bottomPoint - (_gateHeight - (_gateDelta / 2)));
            NewScale(bottomPipe, size);
        }

        private void UpdateGateHeight()
        {

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
                item.transform.Translate(_levelObjectsSpeed * Time.deltaTime * Vector2.left);
            }

            foreach (GameObject item in _pipes)
            {
                item.transform.Translate(_levelObjectsSpeed * Time.deltaTime * Vector2.left);
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
        internal void DestroyPipe(GameObject gameObject)
        {
            _pipes.Remove(gameObject);

            Destroy(gameObject);
        }

        internal void DestroyGroundTile(GameObject gameObject)
        {
            _groundTiles.Remove(gameObject);

            Destroy(gameObject);
        }
        #endregion
    }
}