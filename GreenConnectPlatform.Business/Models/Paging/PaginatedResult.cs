namespace GreenConnectPlatform.Business.Models.Paging;

public class PaginatedResult<T>
{
    public List<T> Data { get; set; } = new();
    public PaginationModel Pagination { get; set; } = new();
}

public class PaginationModel
{
    public PaginationModel()
    {
    }

    public PaginationModel(int totalRecords, int currentPage, int pageSize)
    {
        TotalRecords = totalRecords;
        CurrentPage = currentPage;
        TotalPages = (int)Math.Ceiling((double)totalRecords / pageSize);

        NextPage = CurrentPage < TotalPages ? CurrentPage + 1 : null;
        PrevPage = CurrentPage > 1 ? CurrentPage - 1 : null;
    }

    public int TotalRecords { get; set; }
    public int CurrentPage { get; set; }
    public int TotalPages { get; set; }
    public int? NextPage { get; set; }
    public int? PrevPage { get; set; }
}