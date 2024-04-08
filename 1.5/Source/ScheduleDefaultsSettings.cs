using RimWorld;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace ScheduleDefaults
{
    public class ScheduleDefaultsSettings : ModSettings
    {
        private static Vector2 scrollPos;

        public static List<Schedule> DefaultSchedules = new[] { new Schedule() }.ToList();
        private static int Next = Mathf.Abs(Rand.Int);

        public static Schedule GetNextDefaultSchedule()
        {
            if (Next >= DefaultSchedules.Count)
            {
                Next %= DefaultSchedules.Count;
            }
            return DefaultSchedules[Next++];
        }

        public static TimeAssignmentDef GetDef(TimeAssignment timeAssignment)
        {
            switch (timeAssignment)
            {
                case TimeAssignment.Anything: return TimeAssignmentDefOf.Anything;
                case TimeAssignment.Sleep: return TimeAssignmentDefOf.Sleep;
                case TimeAssignment.Work: return TimeAssignmentDefOf.Work;
                case TimeAssignment.Meditate:
                    if (ModsConfig.RoyaltyActive)
                    {
                        return TimeAssignmentDefOf.Meditate;
                    }
                    else
                    {
                        return TimeAssignmentDefOf.Anything;
                    }
                case TimeAssignment.Joy: return TimeAssignmentDefOf.Joy;
            }
            return TimeAssignmentDefOf.Anything;
        }

        private static TimeAssignment GetEnum(TimeAssignmentDef def)
        {
            if (def == TimeAssignmentDefOf.Anything) return TimeAssignment.Anything;
            if (def == TimeAssignmentDefOf.Sleep) return TimeAssignment.Sleep;
            if (def == TimeAssignmentDefOf.Work) return TimeAssignment.Work;
            if (def == TimeAssignmentDefOf.Meditate) return TimeAssignment.Meditate;
            if (def == TimeAssignmentDefOf.Joy) return TimeAssignment.Joy;
            return TimeAssignment.Anything;
        }

        public static void DoSettingsWindowContents(Rect inRect)
        {
            TimeAssignmentSelector.DrawTimeAssignmentSelectorGrid(new Rect(0f, 0f, 191f, 65f));

            float buttonWidth = 160f;
            if (Widgets.ButtonText(new Rect(inRect.x + inRect.width - buttonWidth, 0f, buttonWidth, 30f), "ScheduleDefaults_AddAlternate".Translate()))
            {
                DefaultSchedules.Add(new Schedule());
                SoundDefOf.Click.PlayOneShotOnCamera(null);
            }

            Text.Font = GameFont.Tiny;
            Text.Anchor = TextAnchor.LowerCenter;
            float labelWidth = 160f;
            float x = inRect.x + labelWidth;
            float cellWidth = 540 / 24f;
            float rowHeight = 30f;
            for (int i = 0; i < 24; i++)
            {
                Widgets.Label(new Rect(x, inRect.y, cellWidth, rowHeight), i.ToString());
                x += cellWidth;
            }

            Widgets.BeginScrollView(new Rect(inRect.x, inRect.y + rowHeight, inRect.width, inRect.height - rowHeight), ref scrollPos, new Rect(0f, 0f, inRect.width - 16f, rowHeight * DefaultSchedules.Count));

            Text.Font = GameFont.Small;
            Text.Anchor = TextAnchor.MiddleLeft;
            float y = 0f;
            for (int i = 0; i < DefaultSchedules.Count; i++)
            {
                Widgets.Label(new Rect(0f, y, labelWidth, rowHeight), "ScheduleDefaults_ScheduleName".Translate(i + 1));

                x = inRect.x + labelWidth;
                for (int j = 0; j < 24; j++)
                {
                    DoTimeAssignment(new Rect(x, y, cellWidth, rowHeight), DefaultSchedules[i], j);
                    x += cellWidth;
                }

                if (i > 0)
                {
                    if (Widgets.ButtonImage(new Rect(labelWidth + cellWidth * 24 + 16f, y + (rowHeight - 24f) / 2, 24f, 24f), TexButton.Delete, Color.white, Color.white * GenUI.SubtleMouseoverColor))
                    {
                        DefaultSchedules.Remove(DefaultSchedules[i--]);
                        SoundDefOf.Click.PlayOneShotOnCamera(null);
                    }
                }

                y += rowHeight;
            }

            Text.Font = GameFont.Small;
            Text.Anchor = TextAnchor.UpperLeft;

            Widgets.EndScrollView();
        }

        private static void DoTimeAssignment(Rect rect, Schedule schedule, int hour)
        {
            rect = rect.ContractedBy(1f);
            bool mouseButton = Input.GetMouseButton(0);
            TimeAssignmentDef assignment = GetDef(schedule.GetTimeAssignment(hour));
            GUI.DrawTexture(rect, assignment.ColorTexture);
            if (!mouseButton)
            {
                MouseoverSounds.DoRegion(rect);
            }
            if (Mouse.IsOver(rect))
            {
                Widgets.DrawBox(rect, 2, null);
                if (mouseButton && assignment != TimeAssignmentSelector.selectedAssignment && TimeAssignmentSelector.selectedAssignment != null)
                {
                    SoundDefOf.Designate_DragStandard_Changed_NoCam.PlayOneShotOnCamera(null);
                    schedule.SetTimeAssignment(hour, GetEnum(TimeAssignmentSelector.selectedAssignment));
                }
            }
        }

        public override void ExposeData()
        {
            Scribe_Collections.Look(ref DefaultSchedules, "DefaultSchedule");
            if (DefaultSchedules == null)
            {
                DefaultSchedules = new[] { new Schedule() }.ToList();
            }
        }
    }
}
