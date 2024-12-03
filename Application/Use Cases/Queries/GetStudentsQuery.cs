using Application.Responses;
using MediatR;

namespace Application.Use_Cases.Queries
{
    public class GetStudentsQuery : IRequest<GetStudentsResponse>
    {
        public string? Name { get; set; }
        public bool? Status { get; set; }
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 2;
    }

}