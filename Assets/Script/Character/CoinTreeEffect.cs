using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinTreeEffect : MonoBehaviour
{
    public SkeletonAnimation skeletonAnimation;

    public void Setting(string aniName)
    {
        skeletonAnimation.AnimationState.SetAnimation(0, aniName, false);
        Invoke("D", 0.93f);
    }

    void D()
    {
        Destroy(this.gameObject);
    }
}
