using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class JankAnimationControlData
{
    public string AbilityName = string.Empty;
    public JankAnimator Animation;
    public bool PlayInstantly = true;
    public bool Loop = false;
    public bool Rare = false;
}

public static class ListGetRandom
{
    private static System.Random rand = null;
    public static T GetRandom<T>(this List<T> thisList)
    {
        if (thisList.Count == 1)
            return thisList[0];
        
        if (rand == null)
            rand = new System.Random();

        int index = rand.Next(thisList.Count);
        return thisList[index];
    }
}

public class JankAnimationController : MonoBehaviour
{
    public JankAnimationControlData IdleAnimation;
    public JankAnimationControlData[] AbilityAnimations;

    public JankAnimationControlData nextAnimation = null;
    private JankAnimator CurrentAnimator;
    private bool AnimationQueued = false;

    private Dictionary<string, Dictionary<bool, List<JankAnimationControlData>>> animationData = new Dictionary<string, Dictionary<bool, List<JankAnimationControlData>>>();

    public void ExecuteAbilityAnimation(string abilityName)
    {
        if (animationData.ContainsKey(abilityName))
        {
            if (animationData[abilityName][true].Count > 0)
            {
                // there are rare animations, do RNG
            }
            else
            {
                JankAnimationControlData asdf = animationData[abilityName][false].GetRandom();
                if (asdf.PlayInstantly)
                {
                    CurrentAnimator.CancelAnimation();
                    asdf.Animation.StartJankAnimation(endAnimationCallback);
                    CurrentAnimator = asdf.Animation;
                }
                else
                {
                    nextAnimation = asdf;
                }
            }
        }
    }

    private void endAnimationCallback()
    {
        if (AnimationQueued && nextAnimation != null)
        {            
            nextAnimation.Animation.StartJankAnimation(endAnimationCallback);
            CurrentAnimator = nextAnimation.Animation;
            nextAnimation = null;
        }
        else
        {
            IdleAnimation.Animation.StartJankAnimation(endAnimationCallback);
            CurrentAnimator = IdleAnimation.Animation;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        foreach (JankAnimationControlData anim in AbilityAnimations)
        {
            if (!animationData.ContainsKey(anim.AbilityName))
            {
                animationData.Add(anim.AbilityName, new Dictionary<bool, List<JankAnimationControlData>>());
                animationData[anim.AbilityName].Add(true, new List<JankAnimationControlData>());
                animationData[anim.AbilityName].Add(false, new List<JankAnimationControlData>());
            }

            if (anim.Rare)
            {
                animationData[anim.AbilityName][true].Add(anim);
            }
            else
            {
                animationData[anim.AbilityName][false].Add(anim);
            }
        }

        IdleAnimation.Animation.StartJankAnimation(endAnimationCallback);
        CurrentAnimator = IdleAnimation.Animation;
    }
}
