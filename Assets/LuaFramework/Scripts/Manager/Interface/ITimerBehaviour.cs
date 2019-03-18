using System;
using System.Collections;

namespace LuaFramework
{
    public interface ITimerBehaviour
    {
        void TimerUpdate();
        bool IsDiscard();

        void OnDestory();
    }
}