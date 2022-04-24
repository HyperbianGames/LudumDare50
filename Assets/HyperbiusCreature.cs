using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public enum AttachmentPoints
{
    List,
    All,
    The,
    Attachment,
    Points,
}

[Serializable]
public class AttachmentPoint
{
    public AttachmentPoints AttacmnetType;
    public GameObject GO;
}

public class HyperbiusCreature : MonoBehaviour
{
    public AttachmentPoint[] AttachmentPoints;

    // Name
    // Description

    // Coalesce Questions
    // How do we preview??
    // 
}
