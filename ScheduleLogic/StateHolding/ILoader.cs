namespace ScheduleLogic.StateHolding
{
    public interface ILoader
    {
        Schedule LoadObj(string file);
        void SaverObj(string file, object obj);
    }
}
