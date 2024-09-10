namespace EDRSM.API.DTOs
{
    public class CreateFaqDto
    {
        public string Category { get; set; }
        public int ByCategorySorter { get; set; }
        public string Question { get; set; }
        public string Answer { get; set; }
    }
}
