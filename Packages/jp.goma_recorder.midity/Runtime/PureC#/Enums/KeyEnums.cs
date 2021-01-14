using System;
using static Midity.NoteKey;
using static Midity.KeyAccidentalSign;
using static Midity.Tonality;

namespace Midity
{
    public enum NoteKey : byte
    {
        CFlatMajor,
        AFlatMinor,
        GFlatMajor,
        EFlatMinor,
        DFlatMajor,
        BFlatMinor,
        AFlatMajor,
        FMinor,
        EFlatMajor,
        CMinor,
        BFlatMajor,
        GMinor,
        FMajor,
        DMinor,
        CMajor,
        AMinor,
        GMajor,
        EMinor,
        DMajor,
        BMinor,
        AMajor,
        FSharpMinor,
        EMajor,
        CSharpMinor,
        BMajor,
        GSharpMinor,
        FSharpMajor,
        DSharpMinor,
        CSharpMajor,
        ASharpMinor
    }

    /// <summary>The number of sharps or flats in the key signature.</summary>
    public enum KeyAccidentalSign : sbyte
    {
        /// <summary>Key has 7 flats.</summary>
        Flat7 = -7,

        /// <summary>Key has 6 flats.</summary>
        Flat6 = -6,

        /// <summary>Key has 5 flats.</summary>
        Flat5 = -5,

        /// <summary>Key has 4 flats.</summary>
        Flat4 = -4,

        /// <summary>Key has 3 flats.</summary>
        Flat3 = -3,

        /// <summary>Key has 2 flats.</summary>
        Flat2 = -2,

        /// <summary>Key has 1 flat.</summary>
        Flat1 = -1,

        /// <summary>Key has no sharps or flats.</summary>
        NoFlatsOrSharps = 0,

        /// <summary>Key has 1 sharp.</summary>
        Sharp1 = 1,

        /// <summary>Key has 2 sharps.</summary>
        Sharp2 = 2,

        /// <summary>Key has 3 sharps.</summary>
        Sharp3 = 3,

        /// <summary>Key has 4 sharps.</summary>
        Sharp4 = 4,

        /// <summary>Key has 5 sharps.</summary>
        Sharp5 = 5,

        /// <summary>Key has 6 sharps.</summary>
        Sharp6 = 6,

        /// <summary>Key has 7 sharps.</summary>
        Sharp7 = 7
    }

    /// <summary>The tonality of the key signature (major or minor).</summary>
    public enum Tonality : byte
    {
        /// <summary>Key is major.</summary>
        Major = 0,

        /// <summary>Key is minor.</summary>
        Minor = 1
    }

    public static class NoteKeyExtension
    {
        public static NoteKey ToKey(this (KeyAccidentalSign, Tonality) value)
        {
            var (keyAccidentalSign, tonality) = value;
            if (tonality == Major)
                switch (keyAccidentalSign)
                {
                    case Flat7:
                        return CFlatMajor;
                    case Flat6:
                        return GFlatMajor;
                    case Flat5:
                        return DFlatMajor;
                    case Flat4:
                        return AFlatMajor;
                    case Flat3:
                        return EFlatMajor;
                    case Flat2:
                        return BFlatMajor;
                    case Flat1:
                        return FMajor;
                    case NoFlatsOrSharps:
                        return CMajor;
                    case Sharp1:
                        return GMajor;
                    case Sharp2:
                        return DMajor;
                    case Sharp3:
                        return AMajor;
                    case Sharp4:
                        return EMajor;
                    case Sharp5:
                        return BMajor;
                    case Sharp6:
                        return FSharpMajor;
                    case Sharp7:
                        return CSharpMajor;
                }
            else
                switch (keyAccidentalSign)
                {
                    case Flat7:
                        return AFlatMinor;
                    case Flat6:
                        return EFlatMinor;
                    case Flat5:
                        return BFlatMinor;
                    case Flat4:
                        return FMinor;
                    case Flat3:
                        return CMinor;
                    case Flat2:
                        return GMinor;
                    case Flat1:
                        return DMinor;
                    case NoFlatsOrSharps:
                        return AMinor;
                    case Sharp1:
                        return EMinor;
                    case Sharp2:
                        return BMinor;
                    case Sharp3:
                        return FSharpMinor;
                    case Sharp4:
                        return CSharpMinor;
                    case Sharp5:
                        return GSharpMinor;
                    case Sharp6:
                        return DSharpMinor;
                    case Sharp7:
                        return ASharpMinor;
                }

            throw new Exception();
        }

        public static (KeyAccidentalSign keyAccidentalSign, Tonality tonality) ToKeyAccidentalSign_Tonality(
            this NoteKey noteKey)
        {
            switch (noteKey)
            {
                case CFlatMajor:
                    return (Flat7, Major);
                case AFlatMinor:
                    return (Flat7, Minor);
                case GFlatMajor:
                    return (Flat6, Major);
                case EFlatMinor:
                    return (Flat6, Minor);
                case DFlatMajor:
                    return (Flat5, Major);
                case BFlatMinor:
                    return (Flat5, Minor);
                case AFlatMajor:
                    return (Flat4, Major);
                case FMinor:
                    return (Flat4, Minor);
                case EFlatMajor:
                    return (Flat3, Major);
                case CMinor:
                    return (Flat3, Minor);
                case BFlatMajor:
                    return (Flat2, Major);
                case GMinor:
                    return (Flat2, Minor);
                case FMajor:
                    return (Flat1, Major);
                case DMinor:
                    return (Flat1, Minor);
                case CMajor:
                    return (NoFlatsOrSharps, Major);
                case AMinor:
                    return (NoFlatsOrSharps, Minor);
                case GMajor:
                    return (Sharp1, Major);
                case EMinor:
                    return (Sharp1, Minor);
                case DMajor:
                    return (Sharp2, Major);
                case BMinor:
                    return (Sharp2, Minor);
                case AMajor:
                    return (Sharp3, Major);
                case FSharpMinor:
                    return (Sharp3, Minor);
                case EMajor:
                    return (Sharp4, Major);
                case CSharpMinor:
                    return (Sharp4, Minor);
                case BMajor:
                    return (Sharp5, Major);
                case GSharpMinor:
                    return (Sharp5, Minor);
                case FSharpMajor:
                    return (Sharp6, Major);
                case DSharpMinor:
                    return (Sharp6, Minor);
                case CSharpMajor:
                    return (Sharp7, Major);
                case ASharpMinor:
                    return (Sharp7, Minor);
                default:
                    throw new Exception();
            }
        }
    }
}