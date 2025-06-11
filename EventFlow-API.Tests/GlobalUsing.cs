global using Xunit;
global using Moq;
global using FluentAssertions;

global using Microsoft.EntityFrameworkCore;
global using Microsoft.AspNetCore.Mvc;

global using EventFlow.Core.Commands;
global using EventFlow.Core.Models;
global using EventFlow.Core.Models.DTOs;
global using EventFlow.Core.Services.Interfaces;
global using EventFlow.Application.Services;
global using EventFlow.Application.Validators;
global using EventFlow.Presentation.Controllers;