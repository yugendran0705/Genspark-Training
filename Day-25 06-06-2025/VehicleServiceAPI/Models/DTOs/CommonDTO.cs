namespace VehicleServiceAPI.DTOs
{
    public class CommonDTO
    {
        public bool Success { get; set; }
        public string Message { get; set; }
    }

    public class PaginationDTO
    {
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
        public int TotalRecords { get; set; }
        public int TotalPages { get; set; }
    }
}
