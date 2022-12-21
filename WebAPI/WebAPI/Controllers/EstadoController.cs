
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebAPI.Data;
using WebAPI.Model;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController, Authorize]
    public class EstadoController : ControllerBase
    {
        private readonly DbContextClass _context;

        public EstadoController(DbContextClass context)
        {
            _context = context;
        }

        [HttpGet]
        [Route("EstadosList")]
        public async Task<ActionResult<IEnumerable<Estado>>> Get()
        {
            try
            {
                var data = await _context.Estados.ToListAsync();
                return data;
            }
            catch (Exception ex)
            {
                return BadRequest("Erro ao Listar os Estados - " + ex.Message);
            }

        }


        [HttpGet]
        [Route("EstadosDetail")]
        public async Task<ActionResult<Estado>> Get(int id, ActionResult<Estado> data)
        {
            try
            {
                var resultData = await _context.Estados.FindAsync(id);

                if (resultData == null)
                    return NotFound();

                return data;
            }
            catch (Exception ex)
            {
                return BadRequest("Erro ao Obter o Estado - " + ex.Message);
            }
        }


        [HttpPost]
        [Route("CreateEstado")]
        public async Task<ActionResult<Estado>> POST(Estado data)
        {
            try
            {
                if (data == null)
                {
                    return NotFound();
                }

                _context.Estados.Add(data);
                await _context.SaveChangesAsync();
                return CreatedAtAction(nameof(Get), new { id = data.EstadoId }, data);
            }
            catch (Exception ex)
            {
                return BadRequest("Erro ao Criar o Estado - " + ex.Message);
            }
        }

        [HttpPost]
        [Route("DeleteEstado")]
        public async Task<ActionResult<IEnumerable<Estado>>> Delete(int id)
        {
            try
            {
                var data = await _context.Estados.FindAsync(id);
                if (data == null)
                {
                    return NotFound();
                }
                _context.Estados.Remove(data);

                await _context.SaveChangesAsync();

                return await _context.Estados.ToListAsync();
            }
            catch (Exception ex)
            {
                return BadRequest("Erro ao Deletar o Estado - " + ex.Message);
            }
        }


        [HttpPost]
        [Route("UpdateEstado")]
        public async Task<ActionResult<IEnumerable<Estado>>> Update(int id, Estado data)
        {
            try
            {
                if (id != data.EstadoId)
                {
                    return BadRequest();
                }

                var resultData = await _context.Estados.FindAsync(id);
                if (resultData == null)
                {
                    return NotFound();
                }

                resultData.Nome = data.Nome;
                await _context.SaveChangesAsync();
                return await _context.Estados.ToListAsync();
            }
            catch (Exception ex)
            {
                return BadRequest("Erro ao Atualizar o Estado - " + ex.Message);
            }
        }
    }
}
