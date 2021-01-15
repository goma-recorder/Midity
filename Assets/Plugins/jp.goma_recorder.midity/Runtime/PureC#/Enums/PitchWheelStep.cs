namespace Midity
{
    /// <summary>Half and whole value steps for the pitch wheel.</summary>
    public enum PitchWheelStep : short
    {
        /// <summary>A complete whole step down.</summary>
        WholeStepDown = 0x0 - 8192,

        /// <summary>3/4 steps down.</summary>
        ThreeQuarterStepDown = 0x500 - 8192,

        /// <summary>1/2 step down.</summary>
        HalfStepDown = 0x1000 - 8192,

        /// <summary>1/4 step down.</summary>
        QuarterStepDown = 0x1500 - 8192,

        /// <summary>No movement.</summary>
        NoStep = 0x2000 - 8192,

        /// <summary>1/4 step up.</summary>
        QuarterStepUp = 0x2500 - 8192,

        /// <summary>1/2 step up.</summary>
        HalfStepUp = 0x3000 - 8192,

        /// <summary>3/4 steps up.</summary>
        ThreeQuarterStepUp = 0x3500 - 8192,

        /// <summary>A complete whole step up.</summary>
        WholeStepUp = 0x3FFF - 8192
    }
}