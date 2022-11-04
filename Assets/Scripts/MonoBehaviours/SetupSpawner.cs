using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

public class SetupSpawner : MonoBehaviour
{
    [SerializeField] private GameObject _personPrefab;
    [SerializeField] private int _gridSize;
    [SerializeField] private int _spread;
    [SerializeField] private Vector2 _speedRange = new Vector2(1,5);
    [SerializeField] private Vector2 _lifetimeRange = new Vector2(10, 120);

    private BlobAssetStore _blob;

    private void Start()
    {
        _blob = new BlobAssetStore();
        GameObjectConversionSettings settings = GameObjectConversionSettings.FromWorld(World.DefaultGameObjectInjectionWorld, _blob);
        Entity entity = GameObjectConversionUtility.ConvertGameObjectHierarchy(_personPrefab, settings);
        EntityManager entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
        

        for(int i = 0; i< _gridSize; i++)
        {
            for(int z = 0; z < _gridSize; z++)
            {
                float speed = UnityEngine.Random.Range(_speedRange.x, _speedRange.y);
                float lifetime = UnityEngine.Random.Range(_lifetimeRange.x, _lifetimeRange.y);
                var instance = entityManager.Instantiate(entity);

                float3 position = new float3(i, 0, z);
                float3 destinationposition = new float3(i * _spread, 0, z * _spread);
                entityManager.SetComponentData(instance, new Translation { Value = position });
                entityManager.SetComponentData(instance, new Destination { Value = position });
                entityManager.SetComponentData(instance, new MovementSpeed { Value = speed });
                entityManager.SetComponentData(instance, new Lifetime { Value = lifetime });

            }
        }
        
       
    }

    private void OnDestroy()
    {
        _blob.Dispose();
    }
}
