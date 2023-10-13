using ContactBook.Model;

namespace ContactBook.Data.DTOs
{
    public class PaginationDTO
    {
        public int TotalUsers { get; set; }
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
        public List<User> Users { get; set; }


    }
}
