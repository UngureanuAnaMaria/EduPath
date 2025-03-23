using Application.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Use_Cases.Queries
{
    public class GetUsersQuery : IRequest<List<UserDTO>> { }
}
