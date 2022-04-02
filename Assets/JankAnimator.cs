using System;
using UnityEngine;

[Serializable]
public class JankKeyFrame
{
    public GameObject nextLoc;
    public float lerpTime;
    
}

public class JankAnimator : MonoBehaviour
{
    public GameObject ObjectToJanklyAnimate;

    public JankKeyFrame[] KeyFrames;
    public bool animateOnStart = true;

    public bool currentlyAnimation = false;
    private int currentJankFrameIndex = 0;
    private float frameStartTime = 0;
    private float frameEndTime = 0;

    private Vector3 OriginalLocation;
    private Quaternion OriginalRotation;

    private Vector3 CurrentFrameLocation;
    private Quaternion CurrentFrameRotation;

    private Vector3 NextFrameLocation;
    private Quaternion NextFrameRotation;


    private bool lastFrame = false;
    private bool executeCallaback = false;
    private Action callbackMethod;

    // Update is called once per frame
    void Update()
    {
        if ((!currentlyAnimation) && (animateOnStart))
        {
            animateOnStart = false;
            StartJankAnimation();
        }

        if (currentlyAnimation)
        {
            Animate();
        }
    }

    public float GetPercentThroughAnim()
    {
        return (frameEndTime - Time.time) / (frameEndTime - frameStartTime);
    }

    private void Animate()
    {
        if (Time.time < frameEndTime)
        {
            ObjectToJanklyAnimate.transform.localPosition = Vector3.Lerp(NextFrameLocation, CurrentFrameLocation, GetPercentThroughAnim());
            ObjectToJanklyAnimate.transform.localRotation = Quaternion.Lerp(NextFrameRotation, CurrentFrameRotation, GetPercentThroughAnim());
        }
        else
        {
            NextKeyFrame();
        }
    }

    private void NextKeyFrame()
    {
        if (lastFrame)
        {
            lastFrame = false;
            currentlyAnimation = false;

            ObjectToJanklyAnimate.transform.localPosition = OriginalLocation;
            ObjectToJanklyAnimate.transform.localRotation = OriginalRotation;

            if (executeCallaback)
                callbackMethod();
        }
        else
        {
            frameStartTime = Time.time;

            if (currentJankFrameIndex < KeyFrames.Length - 1)
            {
                currentJankFrameIndex++;
                frameEndTime = frameStartTime + KeyFrames[currentJankFrameIndex].lerpTime;
                CurrentFrameLocation = KeyFrames[currentJankFrameIndex - 1].nextLoc.transform.localPosition;
                CurrentFrameRotation = KeyFrames[currentJankFrameIndex - 1].nextLoc.transform.localRotation;
                NextFrameLocation = KeyFrames[currentJankFrameIndex].nextLoc.transform.localPosition;
                NextFrameRotation = KeyFrames[currentJankFrameIndex].nextLoc.transform.localRotation;
            }
            else
            {
                lastFrame = true;
                CurrentFrameLocation = KeyFrames[KeyFrames.Length - 1].nextLoc.transform.localPosition;
                CurrentFrameRotation = KeyFrames[KeyFrames.Length - 1].nextLoc.transform.localRotation;
                NextFrameLocation = OriginalLocation;
                NextFrameRotation = OriginalRotation;
                frameEndTime = frameStartTime + KeyFrames[KeyFrames.Length - 1].lerpTime;
            }
        }
    }

    public void StartJankAnimation()
    {
        currentlyAnimation = true;
        currentJankFrameIndex = 0;
        frameStartTime = Time.time;
        frameEndTime = frameStartTime + KeyFrames[currentJankFrameIndex].lerpTime;
        executeCallaback = false;

        OriginalLocation = ObjectToJanklyAnimate.transform.localPosition;
        OriginalRotation = ObjectToJanklyAnimate.transform.rotation;
        CurrentFrameLocation = OriginalLocation;
        CurrentFrameRotation = OriginalRotation;
        NextFrameLocation = KeyFrames[currentJankFrameIndex].nextLoc.transform.localPosition;
        NextFrameRotation = KeyFrames[currentJankFrameIndex].nextLoc.transform.localRotation;
    }

    public void StartJankAnimation(Action endAnimationCallback)
    {
        callbackMethod = endAnimationCallback;
        StartJankAnimation();
        executeCallaback = true;
    }

    public void CancelAnimation()
    {
        lastFrame = false;
        currentlyAnimation = false;
        ObjectToJanklyAnimate.transform.localPosition = OriginalLocation;
        ObjectToJanklyAnimate.transform.localRotation = OriginalRotation;
    }
}
