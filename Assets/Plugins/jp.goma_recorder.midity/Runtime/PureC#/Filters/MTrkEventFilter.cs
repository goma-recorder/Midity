using System;

namespace Midity
{
    [Serializable]
    public abstract class MTrkEventFilter
    {
        public abstract MTrkEvent Filter(MTrkEvent mTrkEvent);
    }
}