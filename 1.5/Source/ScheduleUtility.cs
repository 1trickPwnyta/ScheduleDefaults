using RimWorld;

namespace ScheduleDefaults
{
    public static class ScheduleUtility
    {
        public static void UpdateTimetableTracker(Pawn_TimetableTracker t)
        {
            Debug.Log("called");
            Schedule schedule = ScheduleDefaultsSettings.GetNextDefaultSchedule();
            t.times.Clear();
            for (int i = 0; i < 24; i++)
            {
                t.times.Add(ScheduleDefaultsSettings.GetDef(schedule.GetTimeAssignment(i)));
            }
        }
    }
}
