namespace Midity.Editor
{
    public abstract class MetaEventLabel : MTrkEventLabel
    {
        public MetaEventLabel() : base()
        {
        }

        public MetaEventLabel(MetaEvent onNoteEvent) : base(onNoteEvent)
        {
        }
    }
}