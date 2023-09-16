namespace Domain.Models;

public record PaginationRecord(string orderby, 
                               int page = 1,
                               int count = 10,
                               bool descending = true,
                               bool asNoTracking = true);