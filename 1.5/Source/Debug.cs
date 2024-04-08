namespace ScheduleDefaults
{
    public static class Debug
    {
        public static void Log(string message)
        {
#if DEBUG
            Verse.Log.Message($"[{ScheduleDefaultsMod.PACKAGE_NAME}] {message}");
#endif
        }
    }
}
