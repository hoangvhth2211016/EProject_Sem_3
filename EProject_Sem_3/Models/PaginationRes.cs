using System.ComponentModel.DataAnnotations;

namespace EProject_Sem_3.Models;

public class PaginationRes <T>
{
    public int PageNo { get; set; }

    public int PerPage { get; set; }

    public int LastPage { get; set; }

    public int Total { get; set; }

    public bool HasPreviousPage => PageNo > 1;

    public bool HasNextPage => PageNo < Total;

    public ICollection<T> Items { get; set; } = [];

    public PaginationRes(int pageNo, int perPage, int totalRecords, ICollection<T> items) {
        this.PageNo = pageNo;
        this.PerPage = perPage;
        this.LastPage = (int)Math.Ceiling((double)totalRecords / perPage);
        Total = totalRecords;
        this.Items = items;
    }
    
}