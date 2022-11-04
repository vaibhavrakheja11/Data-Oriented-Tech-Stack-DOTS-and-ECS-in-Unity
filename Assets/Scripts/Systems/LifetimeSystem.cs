using Unity.Entities;
using Unity.Jobs;

public class LifetimeSystem : SystemBase
{

    private EndSimulationEntityCommandBufferSystem endSimulationECBSystem;

    protected override void OnCreate()
    {
        endSimulationECBSystem = World.GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();
        base.OnCreate();
    }
    protected override void OnUpdate()
    {
        float deltaTime = Time.DeltaTime;
        var ecb = endSimulationECBSystem.CreateCommandBuffer().AsParallelWriter();

        Entities.ForEach((Entity entity, int entityInQueryIndex, ref Lifetime lifetime) => {
            lifetime.Value -= deltaTime;

            if(lifetime.Value <= 0)
            {
                ecb.DestroyEntity(entityInQueryIndex, entity);
                // can instantiate here as well
            }
           
        }).ScheduleParallel();

        endSimulationECBSystem.AddJobHandleForProducer(Dependency);
    }
}
