using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;

namespace MJ.Step3
{
    [BurstCompile]
    public struct FindNearestJob : IJobParallelFor
    {
        // 只读字段，标记为ReadOnly以允许潜在的并行优化
        [ReadOnly] public NativeArray<float3> TargetPositions;
        [ReadOnly] public NativeArray<float3> SeekerPositions;

        // 输出结果数组
        public NativeArray<float3> NearestTargetPositions;

        public void Execute(int index)
        {
            float3 seekerPos = SeekerPositions[index];
            float nearestDist = float.MaxValue;
            float3 nearestTargetPos = float3.zero;

            // 遍历所有目标，计算距离并更新最近的目标
            for(int j = 0; j < TargetPositions.Length; j++)
            {
                float3 targetPos = TargetPositions[j];
                float dist = math.distance(seekerPos, targetPos);
                if(dist < nearestDist)
                {
                    nearestDist = dist;
                    nearestTargetPos = targetPos;
                }
            }

            //将最近的目标位置写入输出数组
            NearestTargetPositions[index] = nearestTargetPos;
        }
    }

}