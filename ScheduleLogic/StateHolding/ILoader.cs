namespace ScheduleLogic.StateHolding
{
    public interface ILoader
    {
        ScheduleEngine LoadObj(string file);
        void SaverObj(string file, object obj);
    }
}
