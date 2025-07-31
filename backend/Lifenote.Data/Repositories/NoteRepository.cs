﻿using Microsoft.EntityFrameworkCore;
using Lifenote.Core.Interfaces;
using Lifenote.Core.Models;
using Lifenote.Data.Data;

namespace Lifenote.Data.Repositories
{
    public class NoteRepository : INoteRepository
    {
        private readonly LifenoteDbContext _context;

        public NoteRepository(LifenoteDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Note>> GetAllAsync(int userId)
        {
            return await _context.Notes
                .Where(n => n.UserId == userId && n.IsArchived == false)
                .OrderByDescending(n => n.CreatedAt)
                .ToListAsync();
        }

        public async Task<Note?> GetByIdAsync(int id)
        {
            return await _context.Notes.FindAsync(id);
        }

        public async Task<Note> CreateAsync(Note note)
        {
            note.CreatedAt = DateTime.UtcNow;
            note.UpdatedAt = DateTime.UtcNow;

            _context.Notes.Add(note);
            await _context.SaveChangesAsync();
            return note;
        }

        public async Task<Note> UpdateAsync(Note note)
        {
            note.UpdatedAt = DateTime.UtcNow;

            _context.Entry(note).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return note;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var note = await _context.Notes.FindAsync(id);
            if (note == null) return false;

            _context.Notes.Remove(note);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<Note>> GetByCategoryAsync(int userId, string category)
        {
            return await _context.Notes
                .Where(n => n.UserId == userId && n.Category == category && n.IsArchived == false)
                .OrderByDescending(n => n.CreatedAt)
                .ToListAsync();
        }

        public async Task<IEnumerable<Note>> GetPinnedAsync(int userId)
        {
            return await _context.Notes
                .Where(n => n.UserId == userId && n.IsPinned == true && n.IsArchived == false)
                .OrderByDescending(n => n.CreatedAt)
                .ToListAsync();
        }

        public async Task<IEnumerable<Note>> SearchAsync(int userId, string searchTerm)
        {
            return await _context.Notes
                .Where(n => n.UserId == userId &&
                           n.IsArchived == false &&
                           (n.Title.Contains(searchTerm) || n.Content.Contains(searchTerm)))
                .OrderByDescending(n => n.CreatedAt)
                .ToListAsync();
        }

        public async Task<bool> ExistsAsync(int id, int userId)
        {
            return await _context.Notes
                .AnyAsync(n => n.Id == id && n.UserId == userId);
        }
    }
}
