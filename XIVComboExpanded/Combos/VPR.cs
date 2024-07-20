using Dalamud.Game.ClientState.JobGauge.Types;

using DreadCombo = Dalamud.Game.ClientState.JobGauge.Enums.DreadCombo;

namespace XIVComboExpandedPlugin.Combos;

internal static class VPR
{
    public const byte JobID = 41;

    public const uint
            SteelFangs = 34606,
            DreadFangs = 34607,
            HuntersSting = 34608,
            SwiftskinsSting = 34609,
            FlankstingStrike = 34610,
            FlanksbaneFang = 34611,
            HindstingStrike = 34612,
            HindsbaneFang = 34613,

            SteelMaw = 34614,
            DreadMaw = 34615,
            HuntersBite = 34616,
            SwiftskinsBite = 34617,
            JaggedMaw = 34618,
            BloodiedMaw = 34619,

            Dreadwinder = 34620,
            HuntersCoil = 34621,
            SwiftskinsCoil = 34622,
            PitOfDread = 34623,
            HuntersDen = 34624,
            SwiftskinsDen = 34625,

            SerpentsTail = 35920,
            DeathRattle = 34634,
            LastLash = 34635,
            Twinfang = 35921,
            Twinblood = 35922,
            TwinfangBite = 34636,
            TwinfangThresh = 34638,
            TwinbloodBite = 34637,
            TwinbloodThresh = 34639,

            UncoiledFury = 34633,
            UncoiledTwinfang = 34644,
            UncoiledTwinblood = 34645,

            SerpentsIre = 34647,
            Reawaken = 34626,
            FirstGeneration = 34627,
            SecondGeneration = 34628,
            ThirdGeneration = 34629,
            FourthGeneration = 34630,
            Ouroboros = 34631,
            FirstLegacy = 34640,
            SecondLegacy = 34641,
            ThirdLegacy = 34642,
            FourthLegacy = 34643,

            WrithingSnap = 34632,
            Slither = 34646;

    public static class Buffs
    {
        public const ushort
            FlankstungVenom = 3645,
            FlanksbaneVenom = 3646,
            HindstungVenom = 3647,
            HindsbaneVenom = 3648,
            GrimhuntersVenom = 3649,
            GrimskinsVenom = 3650,
            HuntersVenom = 3657,
            SwiftskinsVenom = 3658,
            FellhuntersVenom = 3659,
            FellskinsVenom = 3660,
            PoisedForTwinfang = 3665,
            PoisedForTwinblood = 3666,
            HuntersInstinct = 3668, // Double check, might also be 4120
            Swiftscaled = 3669,     // Might also be 4121
            Reawakened = 3670,
            ReadyToReawaken = 3671;
    }

    public static class Debuffs
    {
        public const ushort
            NoxiousGash = 3667;
    }

    public static class Levels
    {
        public const byte
            SteelFangs = 1,
            HuntersSting = 5,
            DreadFangs = 10,
            WrithingSnap = 15,
            SwiftskinsSting = 20,
            SteelMaw = 25,
            Single3rdCombo = 30, // Includes Flanksting, Flanksbane, Hindsting, and Hindsbane
            DreadMaw = 35,
            Slither = 40,
            HuntersBite = 40,
            SwiftskinsBike = 45,
            AoE3rdCombo = 50,    // Jagged Maw and Bloodied Maw
            DeathRattle = 55,
            LastLash = 60,
            Dreadwinder = 65,    // Also includes Hunter's Coil and Swiftskin's Coil
            PitOfDread = 70,     // Also includes Hunter's Den and Swiftskin's Den
            TwinsSingle = 75,    // Twinfang Bite and Twinblood Bite
            TwinsAoE = 80,       // Twinfang Thresh and Twinblood Thresh
            UncoiledFury = 82,
            UncoiledTwins = 92,  // Uncoiled Twinfang and Uncoiled Twinblood
            SerpentsIre = 86,
            EnhancedRattle = 88, // Third stack of Rattling Coil can be accumulated
            Reawaken = 90,       // Also includes First Generation through Fourth Generation
            Ouroboros = 96,
            Legacies = 100;      // First through Fourth Legacy
    }
}

internal class SingleTarget : CustomCombo
{
    protected internal override CustomComboPreset Preset { get; } = CustomComboPreset.SingleTarget;

    protected override uint Invoke(uint actionID, uint lastComboMove, float comboTime, byte level)
    {
        if (actionID == VPR.SteelFangs)
        {
            if (OriginalHook(VPR.SerpentsTail) == VPR.DeathRattle)
                return VPR.DeathRattle;
        }

        if (actionID == VPR.DreadFangs)
        {
            if (OriginalHook(VPR.SerpentsTail) == VPR.DeathRattle)
                return VPR.DeathRattle;
        }

        return actionID;
    }
}

internal class AoE : CustomCombo
{
    protected internal override CustomComboPreset Preset { get; } = CustomComboPreset.AoE;

    protected override uint Invoke(uint actionID, uint lastComboMove, float comboTime, byte level)
    {
        if (actionID == VPR.SteelMaw)
        {
            if (OriginalHook(VPR.SerpentsTail) == VPR.LastLash)
                return VPR.LastLash;
        }

        if (actionID == VPR.DreadMaw)
        {
            if (OriginalHook(VPR.SerpentsTail) == VPR.LastLash)
                return VPR.LastLash;
        }

        return actionID;
    }
}

internal class Reawaken : CustomCombo
{
    protected internal override CustomComboPreset Preset { get; } = CustomComboPreset.Reawaken;

    protected override uint Invoke(uint actionID, uint lastComboMove, float comboTime, byte level)
    {
        if (!HasEffect(VPR.Buffs.Reawakened))
            return actionID;

        if (actionID == VPR.Reawaken || actionID == VPR.SteelFangs)
        {
            var gauge = GetJobGauge<VPRGauge>();

            if (level >= VPR.Levels.Legacies)
            {
                if (OriginalHook(VPR.SerpentsTail) == VPR.FirstLegacy ||
                    OriginalHook(VPR.SerpentsTail) == VPR.SecondLegacy ||
                    OriginalHook(VPR.SerpentsTail) == VPR.ThirdLegacy ||
                    OriginalHook(VPR.SerpentsTail) == VPR.FourthLegacy)
                    return OriginalHook(VPR.SerpentsTail);
            }

            var maxtribute = 4;
            if (level >= VPR.Levels.Ouroboros)
                maxtribute = 5;
            if (gauge.AnguineTribute == maxtribute)
                return VPR.FirstGeneration;
            if (gauge.AnguineTribute == maxtribute - 1)
                return VPR.SecondGeneration;
            if (gauge.AnguineTribute == maxtribute - 2)
                return VPR.ThirdGeneration;
            if (gauge.AnguineTribute == maxtribute - 3)
                return VPR.FourthGeneration;
            if (gauge.AnguineTribute == 1 && level >= VPR.Levels.Ouroboros)
                return VPR.Ouroboros;
        }

        return actionID;
    }
}

internal class Uncoiled : CustomCombo
{
    protected internal override CustomComboPreset Preset { get; } = CustomComboPreset.Uncoiled;

    protected override uint Invoke(uint actionID, uint lastComboMove, float comboTime, byte level)
    {
        if (actionID == VPR.UncoiledFury)
        {
            if (OriginalHook(VPR.Twinfang) == VPR.UncoiledTwinfang && HasEffect(VPR.Buffs.PoisedForTwinfang))
                return VPR.UncoiledTwinfang;
            if (level >= VPR.Levels.UncoiledTwins && OriginalHook(VPR.Twinblood) == VPR.UncoiledTwinblood)
                return VPR.UncoiledTwinblood;
        }

        return actionID;
    }
}

internal class Den : CustomCombo
{
    protected internal override CustomComboPreset Preset { get; } = CustomComboPreset.Den;

    protected override uint Invoke(uint actionID, uint lastComboMove, float comboTime, byte level)
    {
        if (actionID == VPR.PitOfDread)
        {
            var gauge = GetJobGauge<VPRGauge>();
            if (HasEffect(VPR.Buffs.FellhuntersVenom))
                return VPR.TwinfangThresh;
            if (HasEffect(VPR.Buffs.FellskinsVenom))
                return VPR.TwinbloodThresh;
            if (gauge.DreadCombo is DreadCombo.PitOfDread and not DreadCombo.Dreadwinder and not DreadCombo.HuntersCoil and not DreadCombo.SwiftskinsCoil)
                return VPR.HuntersDen;
            if (gauge.DreadCombo is DreadCombo.HuntersDen and not DreadCombo.Dreadwinder and not DreadCombo.HuntersCoil and not DreadCombo.SwiftskinsCoil)
                return VPR.SwiftskinsDen;
            if (gauge.DreadCombo is DreadCombo.SwiftskinsDen and not DreadCombo.Dreadwinder and not DreadCombo.HuntersCoil and not DreadCombo.SwiftskinsCoil)
                return VPR.PitOfDread;
            return VPR.PitOfDread;
        }

        return actionID;
    }
}

internal class Coil : CustomCombo
{
    protected internal override CustomComboPreset Preset { get; } = CustomComboPreset.Coil;

    protected override uint Invoke(uint actionID, uint lastComboMove, float comboTime, byte level)
    {
        if (actionID == VPR.Dreadwinder)
        {
            var gauge = GetJobGauge<VPRGauge>();

            if (HasEffect(VPR.Buffs.HuntersVenom))
                return VPR.TwinfangBite;
            if (HasEffect(VPR.Buffs.SwiftskinsVenom))
                return VPR.TwinbloodBite;
            if (gauge.DreadCombo is DreadCombo.Dreadwinder and not DreadCombo.PitOfDread and not DreadCombo.HuntersDen and not DreadCombo.SwiftskinsDen)
                return VPR.HuntersCoil;
            if (gauge.DreadCombo is DreadCombo.HuntersCoil and not DreadCombo.PitOfDread and not DreadCombo.HuntersDen and not DreadCombo.SwiftskinsDen)
                return VPR.SwiftskinsCoil;
            if (gauge.DreadCombo is DreadCombo.SwiftskinsCoil and not DreadCombo.PitOfDread and not DreadCombo.HuntersDen and not DreadCombo.SwiftskinsDen)
                return VPR.Dreadwinder;
            return VPR.Dreadwinder;
        }

        if (actionID == VPR.HuntersCoil || actionID == VPR.SwiftskinsCoil)
        {
            if (HasEffect(VPR.Buffs.HuntersVenom))
                return VPR.TwinfangBite;
            if (HasEffect(VPR.Buffs.SwiftskinsVenom))
                return VPR.TwinbloodBite;
        }

        return actionID;
    }
}

internal class OffGCD : CustomCombo
{
    protected internal override CustomComboPreset Preset { get; } = CustomComboPreset.OffGCD;

    protected override uint Invoke(uint actionID, uint lastComboMove, float comboTime, byte level)
    {
        if (actionID == VPR.SerpentsTail)
        {
            if (!IsOriginal(VPR.SerpentsTail))
                return OriginalHook(VPR.SerpentsTail);
            if (HasEffect(VPR.Buffs.PoisedForTwinfang) || HasEffect(VPR.Buffs.HuntersVenom) || HasEffect(VPR.Buffs.FellhuntersVenom))
                return OriginalHook(VPR.Twinfang);
            if (HasEffect(VPR.Buffs.PoisedForTwinblood) || HasEffect(VPR.Buffs.SwiftskinsVenom) || HasEffect(VPR.Buffs.FellskinsVenom))
                return OriginalHook(VPR.Twinblood);
            if (!IsOriginal(VPR.Twinfang))
                return OriginalHook(VPR.Twinfang);
            if (!IsOriginal(VPR.Twinblood))
                return OriginalHook(VPR.Twinblood);
        }

        return actionID;
    }
}
