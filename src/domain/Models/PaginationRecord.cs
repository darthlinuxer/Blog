namespace Domain.Models;

public record PaginationRecord(int page,
                               int count,
                               bool descending,
                               bool asNoTracking);