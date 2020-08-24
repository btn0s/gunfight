/* ================================================================
   ---------------------------------------------------
   Project   :    Aurora FPS
   Publisher :    Infinite Dawn
   Author    :    Tamerlan Favilevich
   ---------------------------------------------------
   Copyright © Tamerlan Favilevich 2020 All rights reserved.
   ================================================================ */

using System;

namespace AuroraFPSRuntime
{
    public interface IAuroraYieldInstruction
    {
        bool IsExecuting();
        bool IsPaused();

        void Pause();
        void Resume();
        void Terminate();

        event Action<AuroraYieldInstruction> OnStartedCallback;
        event Action<AuroraYieldInstruction> OnPausedCallback;
        event Action<AuroraYieldInstruction> OnTerminatedCallback;
        event Action<AuroraYieldInstruction> OnDoneCallback;
    }
}