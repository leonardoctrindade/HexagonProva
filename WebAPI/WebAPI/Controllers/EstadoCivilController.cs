
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebAPI.Data;
using WebAPI.Model;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController, Authorize]
    public class EstadoCivilController : ControllerBase
    {
        private readonly DbContextClass _context;

        public EstadoCivilController(DbContextClass context)
        {
            _context = context;
        }

        [HttpGet]
        [Route("EstadoCivilsList")]
        public async Task<ActionResult<IEnumerable<EstadoCivil>>> Get()
        {
            try
            {
                var data = await _context.EstadoCivils.ToListAsync();
                return data;
            }
            catch (Exception ex)
            {
                return BadRequest("Erro ao Listar os Estados Civis - " + ex.Message);
            }

        }


        [HttpGet]
        [Route("EstadoCivilsDetail")]
        public async Task<ActionResult<EstadoCivil>> Get(int id, ActionResult<EstadoCivil> data)
        {
            try
            {
                var resultData = await _context.EstadoCivils.FindAsync(id);

                if (resultData == null)
                    return NotFound();

                return data;
            }
            catch (Exception ex)
            {
                return BadRequest("Erro ao Obter o Estado Civil - " + ex.Message);
            }
        }


        [HttpPost]
        [Route("CreateEstadoCivil")]
        public async Task<ActionResult<EstadoCivil>> POST(EstadoCivil data)
        {
            try
            {
                if (data == null)
                {
                    return NotFound();
                }

                _context.EstadoCivils.Add(data);
                await _context.SaveChangesAsync();
                return CreatedAtAction(nameof(Get), new { id = data.EstadoCivilId }, data);
            }
            catch (Exception ex)
            {
                return BadRequest("Erro ao Criar o Estado Civil - " + ex.Message);
            }
        }

        [HttpPost]
        [Route("DeleteEstadoCivil")]
        public async Task<ActionResult<IEnumerable<EstadoCivil>>> Delete(int id)
        {
            try
            {
                var data = await _context.EstadoCivils.FindAsync(id);
                if (data == null)
                {
                    return NotFound();
                }
                _context.EstadoCivils.Remove(data);

                await _context.SaveChangesAsync();

                return await _context.EstadoCivils.ToListAsync();
            }
            catch (Exception ex)
            {
                return BadRequest("Erro ao Deletar o Estado Civil - " + ex.Message);
            }
        }


        [HttpPost]
        [Route("UpdateEstadoCivil")]
        public async Task<ActionResult<IEnumerable<EstadoCivil>>> Update(int id, EstadoCivil data)
        {
            try
            {
                if (id != data.EstadoCivilId)
                {
                    return BadRequest();
                }

                var resultData = await _context.EstadoCivils.FindAsync(id);
                if (resultData == null)
                {
                    return NotFound();
                }

                resultData.Nome = data.Nome;
                await _context.SaveChangesAsync();
                return await _context.EstadoCivils.ToListAsync();
            }
            catch (Exception ex)
            {
                return BadRequest("Erro ao Atualizar o Estado Civil - " + ex.Message);
            }
        }
    }
}
