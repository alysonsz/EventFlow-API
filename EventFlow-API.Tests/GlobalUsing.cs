global using Xunit;
global using Moq;
global using FluentAssertions;

global using Microsoft.EntityFrameworkCore;
global using Microsoft.AspNetCore.Mvc;

global using EventFlow.Core.Models;
global using EventFlow.Core.Primitives;
global using EventFlow.Core.Repository;
global using EventFlow.Core.ValueObjects;

global using EventFlow.Application.Abstractions;
global using EventFlow.Application.Commands;
global using EventFlow.Application.DTOs;
global using EventFlow.Application.Services;
global using EventFlow.Application.Validators;

global using EventFlow.Presentation.Controllers;