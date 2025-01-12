using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Application.DTO.Base
{
    public class PagingBase
    {
        [DefaultValue(1), Required(ErrorMessage = "Obrigatorio informar o numero da página")]
        public int Page { get; set; } = int.MaxValue;
        [DefaultValue(10), Required(ErrorMessage ="Obrigatório informar o tamanho da página")]
        public int PageSize { get; set; } = int.MinValue;
    }
}
