﻿using EventFlow_API.Models;

namespace EventFlow_API.Repository.Interfaces;

public interface IEventRepository
{
    Task<Event> PostAsync(Event @event);
    Task<Event> UpdateAsync(Event @event);
    Task<int> DeleteAsync(int id);
    Task<Event?> GetEventByIdAsync(int id);
    Task<List<Event>> GetAllEventsAsync();
}
