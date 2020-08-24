/* ================================================================
   ---------------------------------------------------
   Project   :    Aurora FPS
   Publisher :    Infinite Dawn
   Author    :    Tamerlan Favilevich
   ---------------------------------------------------
   Copyright © Tamerlan Favilevich 2017-2020 All rights reserved.
   ================================================================ */

using System;

[AttributeUsage(AttributeTargets.Class)]
public class AIBehaviourEditorTargetAttribute : Attribute
{
    private Type target;

    public AIBehaviourEditorTargetAttribute(Type target)
    {
        this.target = target;
    }

    #region [Getter / Setter]
    public Type GetTarget()
    {
        return target;
    }

    public void SetTarget(Type value)
    {
        target = value;
    }
    #endregion
}
