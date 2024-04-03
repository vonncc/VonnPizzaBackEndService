namespace VonnPizzaBackEndService.Utilities
{
    public class DRYFunctionLibrary
    {
        // checks if limit should show all or limit it returns true or false
        public bool ShouldShowAll(int limit)
        {
            return limit == 0;
        }
    }
}
