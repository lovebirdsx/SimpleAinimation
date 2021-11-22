using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Animations;

public class CustomPlayableBehaviour : PlayableBehaviour {
    int currentClipIndex;
    float timeToNextClip;
    Playable mixer;

    public void Initialize(AnimationClip[] clips, Playable owner, PlayableGraph graph) {
        owner.SetInputCount(1);
        mixer = AnimationMixerPlayable.Create(graph, clips.Length);
        graph.Connect(mixer, 0, owner, 0);
        owner.SetInputWeight(0, 1);
        for (int i = 0; i < clips.Length; i++) {
            var clip = clips[i];
            var clipPlayable = AnimationClipPlayable.Create(graph, clip);
            graph.Connect(clipPlayable, 0, mixer, i);
            mixer.SetInputWeight(i, 1);
        }

        SetInputWeightToClipIndex(0);
    }

    void SetInputWeightToClipIndex(int clipIndex) {
        for (int i = 0; i < mixer.GetInputCount(); i++) {
            mixer.SetInputWeight(i, currentClipIndex == i ? 1 : 0);
        }
    }

    public override void PrepareFrame(Playable playable, FrameData info) {
        if (mixer.GetInputCount() <= 0) {
            return;
        }

        timeToNextClip -= (float)info.deltaTime;
        if (timeToNextClip <= 0f) {
            currentClipIndex++;
            if (currentClipIndex >= mixer.GetInputCount()) {
                currentClipIndex = 0;
            }

            var currentClip = (AnimationClipPlayable)mixer.GetInput(currentClipIndex);
            currentClip.SetTime(0);
            timeToNextClip = currentClip.GetAnimationClip().length;
            SetInputWeightToClipIndex(currentClipIndex);
        }
    }
}
