using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test.Core.DTOS
{
    public record CreateCategoryDTO
    {
        //[Required]
        //[MinLength(3)]
        //[MaxLength(50)]
        public string Name { get; init; }

        public string Description { get; init; }

    }
    public record CategoryDTO : CreateCategoryDTO
    {
        public int Id { get; set; }
        public int ProductCount { get; init; }
    }
}
