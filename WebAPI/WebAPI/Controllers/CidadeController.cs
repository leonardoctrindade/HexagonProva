
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebAPI.Data;
using WebAPI.Model;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController, Authorize]
    public class CidadeController : ControllerBase
    {
        private readonly DbContextClass _context;

        public CidadeController(DbContextClass context)
        {
            _context = context;
        }

        [HttpGet]
        [Route("CidadesList")]
        public async Task<ActionResult<IEnumerable<Cidade>>> Get()
        {
            try
            {
                var data = await _context.Cidades.ToListAsync();
                return data;
            }
            catch (Exception ex)
            {
                return BadRequest("Erro ao Listar as Cidades - " + ex.Message);
            }

        }



        [HttpGet]
        [Route("CidadesListByEstado")]
        public async Task<ActionResult<IEnumerable<Cidade>>> CidadesListByEstado(int estadoId)
        {
            try
            {
                var resultData = await _context.Cidades.Where(x => x.EstadoId == estadoId).OrderBy(x => x.Nome).ToListAsync();

                if (resultData == null)
                    return NotFound();

                return resultData;
            }
            catch (Exception ex)
            {
                return BadRequest("Erro ao Listar as Cidades Por Estado - " + ex.Message);
            }
        }


        [HttpGet]
        [Route("CidadesDetail")]
        public async Task<ActionResult<Cidade>> Get(int id, ActionResult<Cidade> data)
        {
            try
            {
                var resultData = await _context.Cidades.FindAsync(id);

                if (resultData == null)
                    return NotFound();

                return data;
            }
            catch (Exception ex)
            {
                return BadRequest("Erro ao Listar as Cidades - " + ex.Message);
            }
        }


        [HttpPost]
        [Route("CreateCidade")]
        public async Task<ActionResult<Cidade>> POST(Cidade data)
        {
            try
            {
                if (data == null)
                {
                    return NotFound();
                }

                _context.Cidades.Add(data);
                await _context.SaveChangesAsync();
                return CreatedAtAction(nameof(Get), new { id = data.CidadeId }, data);
            }
            catch (Exception ex)
            {
                return BadRequest("Erro ao Criar a Cidade - " + ex.Message);
            }
        }

        [HttpPost]
        [Route("DeleteCidade")]
        public async Task<ActionResult<IEnumerable<Cidade>>> Delete(int id)
        {
            try
            {
                var data = await _context.Cidades.FindAsync(id);
                if (data == null)
                {
                    return NotFound();
                }
                _context.Cidades.Remove(data);

                await _context.SaveChangesAsync();

                return await _context.Cidades.ToListAsync();
            }
            catch (Exception ex)
            {
                return BadRequest("Erro ao Deletar a Cidade - " + ex.Message);
            }
        }


        [HttpPost]
        [Route("UpdateCidade")]
        public async Task<ActionResult<IEnumerable<Cidade>>> Update(int id, Cidade data)
        {
            try
            {
                if (id != data.CidadeId)
                {
                    return BadRequest();
                }

                var resultData = await _context.Cidades.FindAsync(id);
                if (resultData == null)
                {
                    return NotFound();
                }

                resultData.Nome = data.Nome;
                resultData.EstadoId = data.EstadoId;
                await _context.SaveChangesAsync();
                return await _context.Cidades.ToListAsync();
            }
            catch (Exception ex)
            {
                return BadRequest("Erro ao Atualizar a Cidade - " + ex.Message);
            }
        }
    }
}
