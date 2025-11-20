using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;

namespace MJ.Step2
{
    public class FindNearest : MonoBehaviour
    {
        NativeArray<float3> SeekerPositions;
        NativeArray<float3> TargetPositions;
        NativeArray<float3> NearestTargetPositions;

        private Spawner spawner;

        public void Start()
        {
            spawner = Object.FindFirstObjectByType<Spawner>();
            if (spawner == null) return;

            // 使用Persistent分配器创建NativeArray，因为它们需要在整个程序运行期间存在
            TargetPositions = new NativeArray<float3>(spawner.NumTargets, Allocator.Persistent);
            SeekerPositions = new NativeArray<float3>(spawner.NumSeekers, Allocator.Persistent);
            NearestTargetPositions = new NativeArray<float3>(spawner.NumSeekers, Allocator.Persistent);
        }        

        // 重要：必须在对象销毁时释放NativeArray的内存
        public void OnDestroy()
        {
            if (TargetPositions.IsCreated) TargetPositions.Dispose();
            if (SeekerPositions.IsCreated) SeekerPositions.Dispose();
            if (NearestTargetPositions.IsCreated) NearestTargetPositions.Dispose();
        }

        public void Update()
        {
            if (spawner == null) return;

            // 将目标位置复制到NativeArray
            for (int i = 0; i < TargetPositions.Length; i++)
            {
                TargetPositions[i] = Spawner.TargetTransforms[i].localPosition;
            }

            // 将搜索者位置复制到NativeArray
            for (int i = 0; i < SeekerPositions.Length; i++)
            {
                SeekerPositions[i] = Spawner.SeekerTransforms[i].localPosition;
            }

            var job = new FindNearestJob
            {
                SeekerPositions = SeekerPositions,
                TargetPositions = TargetPositions,
                NearestTargetPositions = NearestTargetPositions
            };
            var jobHandle = job.Schedule();
            jobHandle.Complete();
            // 绘制调试线显示每个搜索者到最近目标的连线
            for (int i = 0; i < SeekerPositions.Length; i++)
            {
                Debug.DrawLine(SeekerPositions[i], NearestTargetPositions[i]);
            }
        }
    }
}