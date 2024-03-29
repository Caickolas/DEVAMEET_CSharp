﻿using DEVAMEET_CSharp.Dto;
using DEVAMEET_CSharp.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;

namespace DEVAMEET_CSharp.Repository.Impl
{
    public class RoomRepositoryImpl : IRoomRepository
    {
        private readonly DevameetContext _context;

        public RoomRepositoryImpl(DevameetContext context)
        {
            _context = context;
        }

        public async Task DeleteUserPosition(string clientId)
        {
            var room = await _context.Rooms.Where(r =>  r.ClientId == clientId).FirstOrDefaultAsync();

            _context.Rooms.Remove(room);
            await _context.SaveChangesAsync();
        } 

        public async Task<ICollection<PositionDto>> ListUsersPosition(string link)
        {
            var meet = await _context.Meets.Where(m => m.Link == link).FirstOrDefaultAsync();
            var rooms = await _context.Rooms.Where(r => r.MeetId == meet.Id).ToListAsync();

            return rooms.Select(r => new PositionDto
            {
                X = r.X,
                Y = r.Y,
                Orientation = r.Orientation,
                Id = r.Id,
                Name = r.Username,
                Avatar = r.Avatar,
                Muted = r.Muted,
                Meet = r.meet.Link,
                User = r.UserId.ToString(),
                ClientId = r.ClientId
            }).ToList();
        }

        public async Task UpdateUserMute(MuteDto muteDto)
        {
            var meet = await _context.Meets.Where(m => m.Link == muteDto.Link).FirstOrDefaultAsync();
            var room = await _context.Rooms.Where(r => r.MeetId == meet.Id && r.UserId == Int32.Parse(muteDto.UserId)).FirstOrDefaultAsync();

            room.Muted = muteDto.Muted;

            await _context.SaveChangesAsync();
        }

        public async Task UpdateUserPosition(int userId, string link, string clientId, UpdatePositionDto updatePositionDto)
        {
            var meet = await _context.Meets.Where(m => m.Link == link).FirstOrDefaultAsync();
            var user = await _context.Users.Where(u => u.Id == userId).FirstOrDefaultAsync();

            var usersInRoom = await _context.Rooms.Where(r => r.MeetId == meet.Id).ToListAsync();

            if (usersInRoom.Count >= 15)
                throw new Exception("A sala ja está cheia");

            if (usersInRoom.Any(r => r.UserId == userId || r.ClientId == clientId))
            {
                var position = await _context.Rooms.Where(r => r.UserId == userId || r.ClientId == clientId).FirstOrDefaultAsync();
                position.X = updatePositionDto.X;
                position.Y = updatePositionDto.Y;
                position.Orientation = updatePositionDto.Orientation;
            }
            else
            {
                var room = new Room();
                room.Id = userId;
                room.ClientId = clientId;
                room.X = updatePositionDto.X;
                room.Y = updatePositionDto.Y;
                room.Orientation = updatePositionDto.Orientation;
                room.MeetId = meet.Id;
                room.Username = user.Name;
                room.Avatar = user.Avatar;

                await _context.Rooms.AddAsync(room);
            }

            await _context.SaveChangesAsync();
        }

        public async Task UpdateUserStatus(StatusDto statusDto)
        {
            var meet = await _context.Meets.Where(m => m.Link == statusDto.Link).FirstOrDefaultAsync();
            var room = await _context.Rooms.Where(r => r.MeetId == meet.Id && r.UserId == Int32.Parse(statusDto.UserId)).FirstOrDefaultAsync();

            room.Busy = statusDto.Busy;

            await _context.SaveChangesAsync();
        }
    }
}
