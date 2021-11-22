using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Animations;

public class Mixer : MonoBehaviour {
    public Animator animator;
    public AnimationClip clip0;
    public AnimationClip clip1;
    [Range(0, 1)]
    public float clip0Weight = .5f;

    PlayableGraph graph;
    AnimationPlayableOutput output;
    AnimationMixerPlayable mixerPlayable;

    void Start() {
        graph = PlayableGraph.Create("Mixer");
        output = AnimationPlayableOutput.Create(graph, "mixer", animator);

        mixerPlayable = AnimationMixerPlayable.Create(graph, 2);
        output.SetSourcePlayable(mixerPlayable);

        var clipPlayable0 = AnimationClipPlayable.Create(graph, clip0);
        var clipPlayable1 = AnimationClipPlayable.Create(graph, clip1);
        graph.Connect(clipPlayable0, 0, mixerPlayable, 0);
        graph.Connect(clipPlayable1, 0, mixerPlayable, 1);

        graph.Play();
    }

    void Update() {
        mixerPlayable.SetInputWeight(0, clip0Weight);
        mixerPlayable.SetInputWeight(1, 1 - clip0Weight);
    }

    void OnDestroy() {
        graph.Destroy();
    }
}
