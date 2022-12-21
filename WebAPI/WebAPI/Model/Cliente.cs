using System.ComponentModel.DataAnnotations;

namespace WebAPI.Model
{
    public class Cliente
    {
        public int ClienteId { get; set; }
        public string? Nome { get; set; }
        public string? Cpf { get; set; }
        [Required(ErrorMessage = "A Idade deve ser preenchida")]
        public int Idade { get; set; }
        public int EstadoCivilId { get; set; }
        public int CidadeId { get; set; }

    }
}
