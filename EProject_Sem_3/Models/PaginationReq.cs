using System.ComponentModel.DataAnnotations;

namespace EProject_Sem_3.Models;

public class PaginationReq
{
    public int PageNo { get; set; } = 1;

    public int PerPage { get; set; } = 10;
}