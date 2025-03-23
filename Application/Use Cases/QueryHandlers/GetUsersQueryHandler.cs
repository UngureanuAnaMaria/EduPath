using MediatR;
using Application.DTOs;
using AutoMapper;
using Domain.Entities;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Application.Use_Cases.Queries;
using Domain.Repositories;

public class GetUsersQueryHandler : IRequestHandler<GetUsersQuery, List<UserDTO>>
{
    private readonly IUserRepository userRepository;
    private readonly IMapper mapper;

    public GetUsersQueryHandler(IUserRepository userRepository, IMapper mapper)
    {
        this.userRepository = userRepository;
        this.mapper = mapper;
    }

    public async Task<List<UserDTO>> Handle(GetUsersQuery request, CancellationToken cancellationToken)
    {
        var users = await userRepository.GetAllUsersAsync();
        return mapper.Map<List<UserDTO>>(users);
    }
}
