using Verse;
using HarmonyLib;
using UnityEngine;
using RimWorld;

namespace ScheduleDefaults
{
    public class ScheduleDefaultsMod : Mod
    {
        public const string PACKAGE_ID = "scheduledefaults.1trickPonyta";
        public const string PACKAGE_NAME = "Schedule Defaults";

        public static ScheduleDefaultsSettings Settings;

        public ScheduleDefaultsMod(ModContentPack content) : base(content)
        {
            Settings = GetSettings<ScheduleDefaultsSettings>();

            var harmony = new Harmony(PACKAGE_ID);
            harmony.PatchAll();
            harmony.Patch(typeof(Pawn_TimetableTracker).GetConstructor(new[] { typeof(Pawn) }), null, typeof(Patch_Pawn_TimetableTracker_ctor).GetMethod("Postfix"));

            Log.Message($"[{PACKAGE_NAME}] Loaded.");
        }

        public override string SettingsCategory() => PACKAGE_NAME;

        public override void DoSettingsWindowContents(Rect inRect)
        {
            base.DoSettingsWindowContents(inRect);
            ScheduleDefaultsSettings.DoSettingsWindowContents(inRect);
        }
    }
}
