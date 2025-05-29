using BlogApp.Application.DTOs;
using BlogApp.Application.Interface.IServices;
using BlogApp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogApp.Infrastructure.Services
{
    public class PlaygroundService : IPlaygroundService
    { 
        private readonly PlaygroundModel _model;
        public PlaygroundService()
        {
            _model = new PlaygroundModel();
        }
        public PlaygroundModel ChangeData(PlaygroundDTO dto)
        {
            _model.Name = dto.Name;
            _model.Address = dto.Address;

            return _model;
        }

        public PlaygroundModel PrintData()
        {
            return _model;
        }
    }
}
