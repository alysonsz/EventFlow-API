﻿using EventFlow_API.Models;

namespace EventFlow_API.Repository.Interfaces;

public interface IOrganizerRepository
{
    Task<Organizer> PostAsync(Organizer organizer);
    Task<Organizer> UpdateAsync(Organizer organizer);
    Task<int> DeleteAsync(int id);
    Task<Organizer?> GetOrganizerByIdAsync(int id);
    Task<List<Organizer>> GetAllOrganizersAsync();
}
