/* ==================================================================
   ---------------------------------------------------
   Project   :    Aurora FPS
   Publisher :    Infinite Dawn
   Author    :    Tamerlan Favilevich
   ---------------------------------------------------
   Copyright © Tamerlan Favilevich 2017-2020 All rights reserved.
   ================================================================== */

namespace AuroraFPSEditor
{
    public interface IEditorDelay
    {
        /// <summary>
        ///  Check that delay is done.
        /// </summary>
        bool WaitForSeconds();
    }
}