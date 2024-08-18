using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RootAttachable : AbsAttachable
{
    public override void Detach()
    {
        if (NextAttachment != null)
        {
            NextAttachment.Detach();
            NextAttachment = null;
        }

        AttachmentsUpdate();
    }
}
