using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebSockets.Internal;
using StarterProject.Api.Data;
using StarterProject.Api.Data.Entites;
using StarterProject.Api.Features.Eqiupments.Dtos;
using StarterProject.Api.Features.Equipments;
using StarterProject.Api.Features.Equipments.Dtos;
using StarterProject.Api.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;

namespace StarterProject.Api.Controllers
{
    [Authorize]
    [ApiController]
    public class EquipmentController : ControllerBase
    {
        private readonly IEquipmentRepository _equipmentRepository;
        private readonly IValidator<EquipmentEditDto> _equipmentEditDtoValidator;
        private readonly IValidator<EquipmentCreateDto> _equipmentCreateDtoValidator;
        private readonly IValidator<EquipmentGetDto> _equipmentGetDtoValidator;
        private readonly IValidator<EquipmentDeleteDto> _equipmentDeleteDtoValidator;

        public EquipmentController(IEquipmentRepository equipmentRepository,
            IValidator<EquipmentEditDto> equipmentEditDtoValidator,
            IValidator<EquipmentCreateDto> equipmentCreateDtoValidator,
            IValidator<EquipmentGetDto> equipmentGetDtoValidator,
            IValidator<EquipmentDeleteDto> equipmentDeleteDtoValidator)
        {
            _equipmentRepository = equipmentRepository;
            _equipmentEditDtoValidator = equipmentEditDtoValidator;
            _equipmentCreateDtoValidator = equipmentCreateDtoValidator;
            _equipmentGetDtoValidator = equipmentGetDtoValidator;
            _equipmentDeleteDtoValidator = equipmentDeleteDtoValidator;
        }

        [HttpGet("[controller]")]
        [ProducesResponseType(typeof(List<EquipmentGetDto>), (int)HttpStatusCode.OK)]
        public IActionResult GetAll()
        {
            var userIdString = User.FindFirst(ClaimTypes.Name)?.Value;
            var userId = int.Parse(userIdString);

            var equipment = _equipmentRepository.GetAllEquipments(userId);
            return Ok(equipment);
        }

        [HttpGet("[controller]/{equipmentId:int}")]
        [ProducesResponseType(typeof(EquipmentGetDto), (int)HttpStatusCode.OK)]
        public IActionResult Get(int equipmentId)
        {
            var userIdString = User.FindFirst(ClaimTypes.Name)?.Value;
            var userId = int.Parse(userIdString);

            var equipmentGetDto = new EquipmentGetDto { Id = equipmentId, UserId = userId };
            var validationResult = _equipmentGetDtoValidator.Validate(equipmentGetDto);

            if (!validationResult.IsValid)
                return BadRequest(ValidationHelpers.ConvertValidationResult(validationResult));

            var equipment = _equipmentRepository.GetEquipment(equipmentId);
            return Ok(equipment);
        }

        [HttpPost("[controller]")]
        [ProducesResponseType(typeof(EquipmentGetDto), (int)HttpStatusCode.Created)]
        public IActionResult Post([FromBody] EquipmentCreateDto equipmentCreateDto)
        {
            var userIdString = User.FindFirst(ClaimTypes.Name)?.Value;
            var userId = int.Parse(userIdString);

            equipmentCreateDto.UserId = userId;

            var validationResult = _equipmentCreateDtoValidator.Validate(equipmentCreateDto);

            if (!validationResult.IsValid)
                return BadRequest(ValidationHelpers.ConvertValidationResult(validationResult));

            var equipment = _equipmentRepository.CreateEquipment(equipmentCreateDto);
            return Created("[controller]", equipment);
        }

        [HttpPut("[controller]/{equipmentId:int}")]
        [ProducesResponseType(typeof(EquipmentGetDto), (int)HttpStatusCode.OK)]
        public IActionResult Put(int equipmentId, [FromBody] EquipmentEditDto equipmentEditDto)
        {
            equipmentEditDto.Id = equipmentId;
            equipmentEditDto.UserId = int.Parse(User.FindFirst(ClaimTypes.Name).Value);

            var validationResult = _equipmentEditDtoValidator.Validate(equipmentEditDto);

            if (!validationResult.IsValid)
                return BadRequest(ValidationHelpers.ConvertValidationResult(validationResult));

            var routine = _equipmentRepository.EditEquipment(equipmentId, equipmentEditDto);
            return Ok(routine);
        }

        [HttpDelete("[controller]/{equipmentId:int}")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public IActionResult Delete(int equipmentId)
        {
            EquipmentDeleteDto equipmentDeleteDto = new EquipmentDeleteDto { Id = equipmentId };

            var userIdString = User.FindFirst(ClaimTypes.Name)?.Value;
            var userId = int.Parse(userIdString);

            equipmentDeleteDto.UserId = userId;

            var validationResult = _equipmentDeleteDtoValidator.Validate(equipmentDeleteDto);

            if (!validationResult.IsValid)
                return BadRequest(ValidationHelpers.ConvertValidationResult(validationResult));

            _equipmentRepository.DeleteEquipment(equipmentId);
            return Ok();
        }
    }
}