using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Playables;

public class AnimatiorController1 : MonoBehaviour {
    public Animator animator;
    public AnimationClip[] clips;

    PlayableGraph graph;
    AnimationPlayableOutput output;
    int currentClipIndex;
    float timeToNextClip;
    Playable mixer;

    void Awake() {
        graph = PlayableGraph.Create("CustomPlayableBehaviour");
        graph.SetTimeUpdateMode(DirectorUpdateMode.Manual);
        var customPlayable = ScriptPlayable<CustomPlayableBehaviour>.Create(graph);
        var playQueue = customPlayable.GetBehaviour();
        playQueue.Initialize(clips, customPlayable, graph);

        output = AnimationPlayableOutput.Create(graph, "output", animator);
        output.SetSourcePlayable(customPlayable);
        graph.Play();
    }

    void OnDestroy() {
        graph.Destroy();
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.Space)) {
            if (graph.IsPlaying()) {
                graph.Stop();
            } else {
                graph.Play();
            }
        }

        if (graph.IsPlaying()) {
            graph.Evaluate(Time.deltaTime);
        }
    }
}
