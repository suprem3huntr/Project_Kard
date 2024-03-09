using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IMousable
{
    void onClick();
    void onHover();
    void onNonHover();

    public bool IsHovering
    {
        get;
        set;
    }
}
