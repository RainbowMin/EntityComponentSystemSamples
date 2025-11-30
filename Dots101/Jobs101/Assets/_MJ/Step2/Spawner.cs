using UnityEngine;

namespace MJ.Step2
{
    public class Spawner : MonoBehaviour
    {
        // The set of targets is fixed, so rather than 
        // retrieve the targets every frame, we'll cache 
        // their transforms in this field.
        public static Transform[] TargetTransforms;
        public static Transform[] SeekerTransforms; // 添加这个静态数组

        public GameObject SeekerPrefab;
        public GameObject TargetPrefab;
        public int NumSeekers;
        public int NumTargets;
        public Vector2 Bounds;

        public void Start()
        {
            Random.InitState(123);
            
            SeekerTransforms = new Transform[NumSeekers];
            for (int i = 0; i < NumSeekers; i++)
            {
                GameObject go = GameObject.Instantiate(SeekerPrefab);

                //seek通过自己的Update来在一个随机方向进行直线匀速运动
                var seeker = go.GetComponent<Tutorials.Jobs.Step1.Seeker>();
                Vector2 dir = Random.insideUnitCircle;
                seeker.Direction = new Vector3(dir.x, 0, dir.y);

                SeekerTransforms[i] = go.transform; // 保存搜索者的Transform
                go.transform.localPosition = new Vector3(
                    Random.Range(0, Bounds.x), 0, Random.Range(0, Bounds.y));
            }

            TargetTransforms = new Transform[NumTargets];
            for (int i = 0; i < NumTargets; i++)
            {
                GameObject go = GameObject.Instantiate(TargetPrefab);

                //target通过自己的Update来在一个随机方向进行直线匀速运动
                var target = go.GetComponent<Tutorials.Jobs.Step1.Target>();
                Vector2 dir = Random.insideUnitCircle;
                target.Direction = new Vector3(dir.x, 0, dir.y);

                TargetTransforms[i] = go.transform;
                go.transform.localPosition = new Vector3(
                    Random.Range(0, Bounds.x), 0, Random.Range(0, Bounds.y));
            }
        }
    }
}