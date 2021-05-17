using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerObstacle : MonoBehaviour
{
    public Vector3 _centerSizeSpawner;
    public Vector3 _sizeSpawner;

    public GameObject _PrefabPipe;

    private float _timerSpawn;
    public float _timeSpawnResetValue;
    public float _timeFirstSpawnValue;

    public List<GameObject> pipes = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        _timerSpawn = _timeFirstSpawnValue;
    }

    // Update is called once per frame
    void Update()
    {
        _timerSpawn -= Time.deltaTime;
        if (_timerSpawn <= 0)
        {
            SpawnItem();
            _timerSpawn = _timeSpawnResetValue;
        }
    }

    public void SpawnItem()
    {
        Vector3 pos = _centerSizeSpawner + new Vector3(Random.Range(-_sizeSpawner.x / 2, _sizeSpawner.x / 2), Random.Range(-_sizeSpawner.y / 2, _sizeSpawner.y / 2), Random.Range(-_sizeSpawner.z / 2, _sizeSpawner.z / 2));

        GameObject go = Instantiate(_PrefabPipe, pos, Quaternion.identity);

        pipes.Add(go);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(1, 0, 0, 0.5f);
        Gizmos.DrawCube(_centerSizeSpawner, _sizeSpawner);
    }
}
