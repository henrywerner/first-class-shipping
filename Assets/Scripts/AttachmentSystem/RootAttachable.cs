using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RootAttachable : AbsAttachable
{
    public override void DetachAllConnected()
    {
        if (NextAttachment != null)
        {
            NextAttachment.DetachAllConnected();
            NextAttachment = null;
        }

        AttachmentsUpdate();
    }
}
