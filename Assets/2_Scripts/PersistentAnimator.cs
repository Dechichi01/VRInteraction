using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersistentAnimator {

    private static PersistentAnimator _instance;
    public static PersistentAnimator instance
    {
        get
        {
            if (_instance == null)
                _instance = new PersistentAnimator();

            return _instance;
        }
    }

    public PersistentAnimator()
    {
        if (_instance == null)
            _instance = this;

    }

	public void ChangeAnimRunTime_SmoothTransition(Animator anim, AnimatorOverrideController animOverride, MonoBehaviour mono)
    {
        if (mono == null)
        {
            Debug.LogError("Can't start coroutine without a monobehaviour assined");
            return;
        }

        AnimatorClipInfo[] clipInfos = anim.GetCurrentAnimatorClipInfo(0);
        AnimationClip[] clipsBK = new AnimationClip[clipInfos.Length];

        for (int i = 0; i < clipInfos.Length; i++)
        {
            AnimationClip clip = clipInfos[i].clip;
            clipsBK[i] = animOverride[clip.name];
            animOverride[clip.name] = clip;
        }

        anim.runtimeAnimatorController = animOverride;

        mono.StartCoroutine(ChangeAnimAfterTransition(clipsBK, animOverride, anim, anim.GetCurrentAnimatorStateInfo(0).fullPathHash));
    }

    private IEnumerator ChangeAnimAfterTransition(AnimationClip[] clipsBK, AnimatorOverrideController animOverride, Animator anim,
        int initialStateNameHash)
    {
        yield return new WaitForSecondsRealtime(0.05f);

        while (anim.IsInTransition(0)) yield return null;

        for (int i = 0; i < clipsBK.Length; i++)
        {
            if (clipsBK[i] != null)
                animOverride[clipsBK[i].name] = clipsBK[i];
        }

        if (initialStateNameHash == anim.GetCurrentAnimatorStateInfo(0).fullPathHash)
            anim.CrossFade(initialStateNameHash, 5f, 0, 0);
    }
}

public class AnimatorSnapShot
{
    private int layer;
    private int stateHash;

    private AnimParam<bool>[] boolParams;
    private AnimParam<float>[] floatParams;
    private AnimParam<int>[] intParams;

    public AnimatorSnapShot(Animator anim)
    {
        AnimatorStateInfo info = anim.GetCurrentAnimatorStateInfo(0);
        layer = 0;
        stateHash = info.fullPathHash;

        SaveParams(anim);
    }

    private void SaveParams(Animator anim)
    {
        List<AnimParam<bool>> boolList = new List<AnimParam<bool>>();
        List<AnimParam<float>> floatList = new List<AnimParam<float>>();
        List<AnimParam<int>> intList = new List<AnimParam<int>>();

        AnimatorControllerParameter[] animParams = anim.parameters;
        int count = anim.parameterCount;

        for (int i = 0; i < count; i++)
        {
            switch (animParams[i].type)
            {
                case AnimatorControllerParameterType.Bool:
                    boolList.Add(new AnimParam<bool>(animParams[i].nameHash, anim.GetBool(animParams[i].name)));
                    break;
                case AnimatorControllerParameterType.Float:
                    floatList.Add(new AnimParam<float>(animParams[i].nameHash, anim.GetFloat(animParams[i].name)));
                    break;
                case AnimatorControllerParameterType.Int:
                    intList.Add(new AnimParam<int>(animParams[i].nameHash, anim.GetInteger(animParams[i].name)));
                    break;
            }
        }

        boolParams = boolList.ToArray();
        floatParams = floatList.ToArray();
        intParams = intList.ToArray();
    }

    public void RestoreAnimator(Animator anim)
    {
        for (int i = 0; i < boolParams.Length; i++)
            anim.SetBool(boolParams[i].nameHash, boolParams[i].value);

        for (int i = 0; i < floatParams.Length; i++)
            anim.SetFloat(floatParams[i].nameHash, floatParams[i].value);

        for (int i = 0; i < intParams.Length; i++)
            anim.SetInteger(intParams[i].nameHash, intParams[i].value);

        anim.Play(stateHash, layer);
    }
}

public class AnimParam<T>
{
    public int nameHash;
    public T value;

    public AnimParam(int nameHash, T value)
    {
        this.nameHash = nameHash;
        this.value = value;
    }

}

