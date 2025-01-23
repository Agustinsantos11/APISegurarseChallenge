using ChallengeAPISegurarse.DTOs;
using ChallengeAPISegurarse.Entities;
using ChallengeAPISegurarse.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ChallengeAPISegurarse.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PolizasController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public PolizasController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        
        [HttpPost]
        public async Task<IActionResult> Poliza([FromBody] PolizaRequestDTO polizaDto)
        {
            var cliente = await _unitOfWork.Clientes.GetByIdAsync(polizaDto.ClienteId);
            if (cliente == null)
            {
                return NotFound("Cliente no encontrado.");
            }

            var polizasCliente = await _unitOfWork.Polizas.GetAllAsync();

            var clienteConPoliza = polizasCliente.FirstOrDefault(p => p.ClienteId == cliente.Id);


            if (clienteConPoliza == null)
            {
                return BadRequest("El cliente debe tener una póliza registrada para crear una nueva.");
            }

            var poliza = new Poliza
            {
                ClienteId = polizaDto.ClienteId,
                Auto = polizaDto.Auto,
                Costo = polizaDto.Costo,
                FechaVigencia = polizaDto.FechaVigencia
            };

            await _unitOfWork.Polizas.AddAsync(poliza);
            await _unitOfWork.CompleteAsync();

            return CreatedAtAction(nameof(Poliza), new { id = poliza.Id }, poliza);
        }


        [HttpGet]
        public async Task<IActionResult> Polizas()
        {
            var polizas = await _unitOfWork.Polizas.GetAllAsync();
            return Ok(polizas);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Id(int id)
        {
            var poliza = await _unitOfWork.Polizas.GetByIdAsync(id);
            if (poliza == null)
            {
                return NotFound("Póliza no encontrada.");
            }
            return Ok(poliza);
        }

        [HttpGet("DNI/{dni}")]
        public async Task<IActionResult> Dni(int dni)
        {
            var clienteExisDNI = await _unitOfWork.Clientes.GetByDniAsync(dni);
            
            if (clienteExisDNI == null)
            {
                return NotFound($"No se encontró un cliente con el DNI {dni}.");
            }

            
            var polizas = await _unitOfWork.Polizas.GetAllAsync();
            var polizasConDni = polizas
                .Where(p => p.ClienteId == clienteExisDNI.Id)
                .Select(p => new PolizaResponseDTO
                {
                    Id = p.Id,
                    Auto = p.Auto,
                    Costo = p.Costo,
                    FechaVigencia = p.FechaVigencia
                })
                .ToList();

            if (!polizasConDni.Any())
            {
                return NotFound($"No se encontraron pólizas para el cliente con DNI {dni}.");
            }

            return Ok(polizasConDni);
        }



        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] PolizaRequestDTO polizaDto)
        {
            var poliza = await _unitOfWork.Polizas.GetByIdAsync(id);
            if (poliza == null)
            {
                return NotFound("Póliza no encontrada.");
            }

            poliza.Auto = polizaDto.Auto;
            poliza.Costo = polizaDto.Costo;
            poliza.FechaVigencia = polizaDto.FechaVigencia;

            await _unitOfWork.Polizas.UpdateAsync(poliza);
            await _unitOfWork.CompleteAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var poliza = await _unitOfWork.Polizas.GetByIdAsync(id);
            if (poliza == null)
            {
                return NotFound("Póliza no encontrada.");
            }

            await _unitOfWork.Polizas.DeleteAsync(poliza.Id);
            await _unitOfWork.CompleteAsync();

            return NoContent();
        }
    }
}


