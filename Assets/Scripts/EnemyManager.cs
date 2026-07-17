using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [SerializeField] Collider _playerCollider;
    [SerializeField] Enemy[] _enemies;
    private void OnEnable()
    {
        foreach(var enemy in _enemies)
        {
            enemy._playerCollider = _playerCollider;
        }
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
