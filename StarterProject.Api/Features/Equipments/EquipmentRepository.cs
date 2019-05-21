using StarterProject.Api.Data;
using StarterProject.Api.Data.Entites;
using StarterProject.Api.Features.Equipments.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StarterProject.Api.Features.Equipments
{
    public interface IEquipmentRepository
    {
        EquipmentGetDto GetEquipment(int userId);
        List<EquipmentGetDto> GetAllEquipments(int userId);
        EquipmentGetDto CreateEquipment(EquipmentCreateDto equipmentCreateDto);
        EquipmentGetDto EditEquipment(int userId, EquipmentEditDto equipmentUpdateDto);
        void DeleteEquipment(int equipmentId);
    }

    public class EquipmentRepository : IEquipmentRepository
    {
        private readonly DataContext _context;

        public EquipmentRepository(DataContext context)
        {
            _context = context;
        }

        public EquipmentGetDto CreateEquipment(EquipmentCreateDto equipmentCreateDto)
        {
            var equipment = new Equipment
            {
                Name = equipmentCreateDto.Name,
                UserId = equipmentCreateDto.UserId
            };

            _context.Set<Equipment>().Add(equipment);
            _context.SaveChanges();

            var equipmentGetDto = new EquipmentGetDto
            {
                Id = equipment.Id,
                Name = equipment.Name,
                UserId = equipment.UserId
            };

            return equipmentGetDto;
        }

        public EquipmentGetDto GetEquipment(int equipmentId)
        {
            return _context
                .Set<Equipment>()
                .Select(x => new EquipmentGetDto
                {
                    Id = x.Id,
                    Name = x.Name,
                    UserId = x.UserId
                })
                .FirstOrDefault(x => x.Id == equipmentId);
        }

        public EquipmentGetDto EditEquipment(int userId, EquipmentEditDto equipmentEditDto)
        {
            var equipment = _context.Set<Equipment>().Find(userId);

            equipment.Name = equipmentEditDto.Name;

            _context.SaveChanges();

            var equipmentGetDto = new EquipmentGetDto
            {
                Id = equipment.Id,
                Name = equipment.Name,
                UserId = equipment.UserId
            };

            equipmentGetDto.Id = equipment.Id;

            return equipmentGetDto;
        }

        public List<EquipmentGetDto> GetAllEquipments(int userId)
        {
            return _context
                .Set<Equipment>()
                .Where(x => x.UserId == userId)
                .Select(x => new EquipmentGetDto
                {
                    Id = x.Id,
                    Name = x.Name,
                    UserId = x.UserId
                })
                .ToList();
        }

        public void DeleteEquipment(int equipmentId)
        {
            var equipment = _context.Set<Equipment>().Find(equipmentId);
            _context.Set<Equipment>().Remove(equipment);
            _context.SaveChanges();
        }


    }
}
