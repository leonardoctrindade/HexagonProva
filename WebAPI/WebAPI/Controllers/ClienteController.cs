
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using WebAPI.Data;
using WebAPI.Model;
using WebAPI.Util;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController, Authorize]
    public class ClienteController : ControllerBase
    {
        private readonly DbContextClass _context;

        public ClienteController(DbContextClass context)
        {
            _context = context;
        }

        [HttpGet]
        [Route("ClientesList")]
        public async Task<ActionResult<IEnumerable<Cliente>>> Get()
        {
            try
            {
                var data = await _context.Clientes.ToListAsync();
                return data;
            }
            catch (Exception ex)
            {
                return BadRequest("Erro ao Listar os Clientes - " + ex.Message);
            }

        }


        [HttpGet]
        [Route("ClienteDetail")]
        public async Task<ActionResult<Cliente>> ClienteDetail(int id)
        {
            try
            {
                var resultData = await _context.Clientes.FindAsync(id);

                if (resultData == null)
                    return NotFound();

                return resultData;
            }
            catch (Exception ex)
            {
                return BadRequest("Erro ao Retornar o Cliente - " + ex.Message);
            }
        }


        [HttpGet]
        [Route("ClienteDetailEstadoCidade")]
        public async Task<ActionResult<Object>> ClienteDetailEstadoCidade(int id)
        {
            try
            {
                var query = (from c in _context.Clientes
                             join b in _context.Cidades on c.CidadeId equals b.CidadeId
                             join d in _context.Estados on b.EstadoId equals d.EstadoId
                             join e in _context.EstadoCivils on c.EstadoCivilId equals e.EstadoCivilId
                             where
                             c.CidadeId == b.CidadeId
                             && b.EstadoId == d.EstadoId
                             && c.EstadoCivilId == e.EstadoCivilId
                             && c.ClienteId == id
                             select new
                             {
                                 ClienteId = c.ClienteId,
                                 Nome = c.Nome,
                                 Cpf = c.Cpf,
                                 Idade = c.Idade,
                                 EstadoCivilId = c.EstadoCivilId,
                                 EstadoCivil = e.Nome,
                                 CidadeId = c.CidadeId,
                                 Cidade = b.Nome,
                                 EstadoId = d.EstadoId,
                                 Estado = d.Nome
                             }
                     );

                var resultData = await query.FirstAsync().ConfigureAwait(false);

                if (resultData == null)
                    return NotFound();

                return resultData;
            }
            catch (Exception ex)
            {
                return BadRequest("Erro ao Retornar o Cliente - " + ex.Message);
            }
        }


        [HttpGet]
        [Route("ClienteListEstadoCidade")]
        public async Task<ActionResult<IEnumerable<Object>>> ClienteListEstadoCidade()
        {
            try
            {
                var query = (from c in _context.Clientes
                             join b in _context.Cidades on c.CidadeId equals b.CidadeId
                             join d in _context.Estados on b.EstadoId equals d.EstadoId
                             join e in _context.EstadoCivils on c.EstadoCivilId equals e.EstadoCivilId
                             where
                             c.CidadeId == b.CidadeId
                             && b.EstadoId == d.EstadoId
                             && c.EstadoCivilId == e.EstadoCivilId
                             select new
                             {
                                 ClienteId = c.ClienteId,
                                 Nome = c.Nome,
                                 Cpf = c.Cpf,
                                 Idade = c.Idade,
                                 EstadoCivilId = c.EstadoCivilId,
                                 EstadoCivil = e.Nome,
                                 CidadeId = c.CidadeId,
                                 Cidade = b.Nome,
                                 EstadoId = d.EstadoId,
                                 Estado = d.Nome
                             }
                     );

                var resultData = await query.ToListAsync().ConfigureAwait(false);

                if (resultData == null)
                    return NotFound();

                return resultData;
            }
            catch (Exception ex)
            {
                return BadRequest("Erro ao Retornar a lista de Clientes - " + ex.Message);
            }
        }


        [HttpPost]
        [Route("CreateCliente")]
        public async Task<ActionResult<Cliente>> POST(Cliente data)
        {
            try
            {
                if (data == null)
                {
                    return NotFound();
                }

                if (data.Cpf != null)
                {
                    var searchCpf = _context.Clientes.Where(x => x.Cpf == data.Cpf);

                    if (!CpfValidation.CpfCnpjUtils.IsValid(data.Cpf))
                    {
                        return BadRequest("CPF Inválido!!!");
                    }

                    if (searchCpf.Any())
                    {
                        return BadRequest("CPF Já Cadastrado na Base de Dados!!!");
                    }
                }

                _context.Clientes.Add(data);
                await _context.SaveChangesAsync();
                return CreatedAtAction(nameof(Get), new { id = data.ClienteId }, data);
            }
            catch (Exception ex)
            {
                return BadRequest("Erro ao Cadastrar o Cliente - " + ex.Message);
            }
        }

        [HttpPost]
        [Route("DeleteCliente")]
        public async Task<ActionResult<IEnumerable<Cliente>>> Delete(int id)
        {
            try
            {
                var data = await _context.Clientes.FindAsync(id);
                if (data == null)
                {
                    return NotFound();
                }
                _context.Clientes.Remove(data);

                await _context.SaveChangesAsync();

                return await _context.Clientes.ToListAsync();
            }
            catch (Exception ex)
            {
                return BadRequest("Erro ao Deletar o Cliente - " + ex.Message);
            }
        }


        [HttpPost]
        [Route("UpdateCliente")]
        public async Task<ActionResult<IEnumerable<Cliente>>> Update(int id, Cliente data)
        {
            try
            {
                if (id != data.ClienteId)
                {
                    return BadRequest();
                }

                var resultData = await _context.Clientes.FindAsync(id);
                if (resultData == null)
                {
                    return NotFound();
                }

                if (resultData != null)
                {
                    if (resultData.Cpf != data.Cpf)
                    {

                        if (data.Cpf != null)
                        {
                            var searchCpf = _context.Clientes.Where(x => x.Cpf == data.Cpf);

                            if (!CpfValidation.CpfCnpjUtils.IsValid(data.Cpf))
                            {
                                return BadRequest("CPF Inválido");
                            }

                            if (searchCpf.Any())
                            {
                                return BadRequest("CPF Já Cadastrado na Base de Dados!!!");
                            }

                        }
                    }

                    resultData.Nome = data.Nome;
                    resultData.Idade = data.Idade;
                    resultData.EstadoCivilId = data.EstadoCivilId;
                    resultData.CidadeId = data.CidadeId;
                    resultData.Cpf = data.Cpf;
                }

                await _context.SaveChangesAsync();
                return await _context.Clientes.ToListAsync();
            }
            catch (Exception ex)
            {
                return BadRequest("Erro ao Atualizar o Cliente - " + ex.Message);
            }
        }
    }
}
