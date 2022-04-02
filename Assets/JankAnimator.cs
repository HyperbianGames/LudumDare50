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
            ObjectToJanklyAnimate.transform.position = Vector3.Lerp(NextFrameLocation, CurrentFrameLocation, GetPercentThroughAnim());
            ObjectToJanklyAnimate.transform.rotation = Quaternion.Lerp(NextFrameRotation, CurrentFrameRotation, GetPercentThroughAnim());
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

            ObjectToJanklyAnimate.transform.position = OriginalLocation;
            ObjectToJanklyAnimate.transform.rotation = OriginalRotation;
        }
        else
        {
            frameStartTime = Time.time;

            if (currentJankFrameIndex < KeyFrames.Length - 1)
            {
                currentJankFrameIndex++;
                frameEndTime = frameStartTime + KeyFrames[currentJankFrameIndex].lerpTime;
                CurrentFrameLocation = KeyFrames[currentJankFrameIndex-1].nextLoc.transform.position;
                CurrentFrameRotation = KeyFrames[currentJankFrameIndex-1].nextLoc.transform.rotation;
                NextFrameLocation = KeyFrames[currentJankFrameIndex].nextLoc.transform.position;
                NextFrameRotation = KeyFrames[currentJankFrameIndex].nextLoc.transform.rotation;
            }
            else
            {
                lastFrame = true;
                CurrentFrameLocation = KeyFrames[KeyFrames.Length - 1].nextLoc.transform.position;
                CurrentFrameRotation = KeyFrames[KeyFrames.Length - 1].nextLoc.transform.rotation;
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

        OriginalLocation = ObjectToJanklyAnimate.transform.position;
        OriginalRotation = ObjectToJanklyAnimate.transform.rotation;
        CurrentFrameLocation = OriginalLocation;
        CurrentFrameRotation = OriginalRotation;
        NextFrameLocation = KeyFrames[currentJankFrameIndex].nextLoc.transform.position;
        NextFrameRotation = KeyFrames[currentJankFrameIndex].nextLoc.transform.rotation;
    }
}
