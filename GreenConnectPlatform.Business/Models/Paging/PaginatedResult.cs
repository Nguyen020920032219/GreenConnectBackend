namespace GreenConnectPlatform.Business.Models.Paging;

public class PaginatedResult<T>
{
    public List<T> Data { get; set; } = new();
    public PaginationModel Pagination { get; set; } = new();
}

public class PaginationModel
{
    public int TotalRecords { get; set; }
    public int CurrentPage { get; set; }
    public int TotalPages { get; set; }
    public int? NextPage { get; set; }
    public int? PrevPage { get; set; }
}