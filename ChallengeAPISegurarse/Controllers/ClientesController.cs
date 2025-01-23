using ChallengeAPISegurarse.DTOs;
using ChallengeAPISegurarse.Entities;
using ChallengeAPISegurarse.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace ChallengeAPISegurarse.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ClientesController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IClienteValidationService _clienteValidationService;

        public ClientesController(IUnitOfWork unitOfWork, IClienteValidationService clienteValidationService)
        {
            _unitOfWork = unitOfWork;
            _clienteValidationService = clienteValidationService;
        }

        [HttpPost]
        public async Task<IActionResult> Cliente([FromBody] ClienteDTO clienteDto)
        {
            // Llama al servicio de validación
            var respServ = await _clienteValidationService.ValidarClienteAsync(clienteDto.Nombre, clienteDto.Apellido);
            if (!respServ)
            {
                return BadRequest("Validación externa fallida.");
            }
            
            var clienteExistente = await _unitOfWork.Clientes.GetByDniAsync(clienteDto.DNI);
            if (clienteExistente != null)
            {
                return BadRequest("El DNI ya está registrado.");
            }
            var cliente = new Cliente
            {
                Nombre = clienteDto.Nombre,
                Apellido = clienteDto.Apellido,
                DNI = clienteDto.DNI,
                FechaNacimiento = clienteDto.FechaNacimiento
            };

            await _unitOfWork.Clientes.AddAsync(cliente);
            await _unitOfWork.CompleteAsync();

            return CreatedAtAction(nameof(Cliente), new { id = cliente.Id }, cliente);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Cliente(int id, [FromBody] ClienteDTO clienteDto)
        {
            var clienteExistente = await _unitOfWork.Clientes.GetByIdAsync(id);
            var clientesTot = await _unitOfWork.Clientes.GetAllAsync();
            bool dniExistente = clientesTot.Any(c => c.DNI == clienteDto.DNI && c.Id != id); 

            if (dniExistente)
            {
                return BadRequest("Ya existe un cliente con el mismo DNI.");
            }
            if (clienteExistente == null)
            {
                return NotFound($"No se encontró un cliente con el ID {id}.");
            }
            
            if (clienteExistente.Nombre != clienteDto.Nombre || clienteExistente.Apellido != clienteDto.Apellido)
            {
                var respServ = await _clienteValidationService.ValidarClienteAsync(clienteDto.Nombre, clienteDto.Apellido);
                if (!respServ)
                {
                    return BadRequest("Validación externa fallida.");
                }
            }
            

            clienteExistente.Nombre = clienteDto.Nombre;
            clienteExistente.Apellido = clienteDto.Apellido;
            clienteExistente.DNI = clienteDto.DNI;
            clienteExistente.FechaNacimiento = clienteDto.FechaNacimiento;

            await _unitOfWork.Clientes.UpdateAsync(clienteExistente);
            await _unitOfWork.CompleteAsync();

            return Ok(clienteExistente);
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> Cliente(int id)
        {
            var clienteExistente = await _unitOfWork.Clientes.GetByIdAsync(id);
            if (clienteExistente == null)
            {
                return NotFound($"No se encontró un cliente con el ID {id}.");
            }

            await _unitOfWork.Clientes.DeleteAsync(clienteExistente.Id);
            await _unitOfWork.CompleteAsync();

            return NoContent(); 
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> ClientePorId(int id)
        {
            var cliente = await _unitOfWork.Clientes.GetByIdAsync(id);
            if (cliente == null)
            {
                return NotFound($"No se encontró un cliente con el ID {id}.");
            }

            return Ok(cliente); 
        }

        [HttpGet]
        public async Task<IActionResult> Clientes()
        {
            var clientes = await _unitOfWork.Clientes.GetAllAsync();
            if (clientes == null || !clientes.Any())
            {
                return NotFound(); 
            }

            return Ok(clientes); 
        }

        [HttpGet("DNI/{dni}")]
        public async Task<IActionResult> ClientePorDNI(int dni)
        {
            var clienteExisDNI = await _unitOfWork.Clientes.GetByDniAsync(dni);
            if (clienteExisDNI == null)
            {
                return NotFound($"No se encontró un cliente con el DNI {dni}.");
            }

            return Ok(clienteExisDNI); 
        }
    }


}
