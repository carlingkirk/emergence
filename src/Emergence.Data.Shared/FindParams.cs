namespace Emergence.Data.Shared
{
    public class FindParams
    {
        public FindParams()
        {
            Skip = 0;
            Take = 10;
            SortBy = null;
            SortDirection = SortDirection.Ascending;
        }

        public string SearchText { get; set; }
        public int Skip { get; set; }
        public int Take { get; set; }
        public string SortBy { get; set; }
        public SortDirection SortDirection { get; set; }
    }
}
