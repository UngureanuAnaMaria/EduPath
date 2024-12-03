﻿using Application.Use_Cases.Commands.Create;
using Domain.Common;
using MediatR;

namespace Application.Use_Cases.Commands.Update
{
    public class UpdateProfessorCommand : CreateProfessorCommand, IRequest<Result<Guid>>
    {
        public Guid Id { get; set; }
    }
}