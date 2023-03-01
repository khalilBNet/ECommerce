using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Dtos
{
    // Data Transfer object nestaamlouha bach nraj3ou lel client data ndhifa 
    // fi blaset objet Product type fih id w nom ywali toul el nom fi string aussi el picture url nrak7ouha
    public class ProductToReturnDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string PictureUrl { get; set; }
        public string ProductType { get; set; }
        public string ProductBrand { get; set; }
    }
}