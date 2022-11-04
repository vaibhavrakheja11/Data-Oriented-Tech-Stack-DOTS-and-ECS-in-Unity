using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Jobs.LowLevel.Unsafe;
using Unity.Mathematics;
using Unity.Transforms;

[UpdateInGroup(typeof(InitializationSystemGroup))]
public class RandomSystem : SystemBase
{
    public NativeArray<Unity.Mathematics.Random> RandomArray { get; private set; }

    protected override void OnCreate()
    {
        
        var threadCount = JobsUtility.MaxJobThreadCount;
        var randomArray = new Unity.Mathematics.Random[threadCount];
        var seed = new System.Random();


        /*Entities.ForEach((ref Translation translation, in Rotation rotation) => {

        }).Schedule();*/

        for (int i = 0; i < threadCount; i++)
        {
            randomArray[i] = new Unity.Mathematics.Random((uint)seed.Next());
        }

        RandomArray = new NativeArray<Unity.Mathematics.Random>(randomArray, Allocator.Persistent);

        base.OnCreate();
    }


    protected override void OnDestroy()
    {
        RandomArray.Dispose();
        base.OnDestroy();
    }
    protected override void OnUpdate()
    {
        
    }
}
