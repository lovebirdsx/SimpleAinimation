using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Playables;

public class SimplePlay : MonoBehaviour {
    public Animator animator;
    public AnimationClip clip;

    PlayableGraph graph;
    AnimationPlayableOutput output;

    void Start() {
        // graph相当于动画播放的上下文,要播放动画,必须要有一个该对象
        // 构造一个动画播放器,必须包含Graph和Output两个对象
        // 且这两个对象是一一对应的关系,也就是说,1个Graph只能有1个Output
        // 为什么要这样设计呢?
        // 看起来没这个必要,Graph和Output完全可以合并到一起
        graph = PlayableGraph.Create("SimplePlay");
        graph.SetTimeUpdateMode(DirectorUpdateMode.GameTime);

        // 动画树和Animator一起构成一个Output
        // Animator表示动画播放作用的对象,也就是Animator所在的GameObject
        output = AnimationPlayableOutput.Create(graph, "Animation1", animator);
        var clipPlayable = AnimationClipPlayable.Create(graph, clip);
        output.SetSourcePlayable(clipPlayable);
        graph.Play();
    }

    void OnDestroy() {
        graph.Destroy();
    }
}
