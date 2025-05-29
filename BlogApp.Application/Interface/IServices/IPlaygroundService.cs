using BlogApp.Application.DTOs;
using BlogApp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogApp.Application.Interface.IServices
{
    public interface IPlaygroundService
    {
        PlaygroundModel ChangeData(PlaygroundDTO dto);
        PlaygroundModel PrintData();
    }
}
