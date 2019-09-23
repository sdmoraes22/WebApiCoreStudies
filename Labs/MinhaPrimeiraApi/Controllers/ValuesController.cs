using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MinhaPrimeiraApi.Controllers
{
    [Route("api/[controller]")]
    public class ValuesController : MainController
    {
        // GET api/values
        [HttpGet]
        public ActionResult<IEnumerable<string>> ObterTodos()
        {
            var valores = new string[] { "value1", "value2" };
            if(valores.Length < 5000) 
                return CustomResponse();
            return CustomResponse(valores); 
        }
        
        [HttpGet("obter-resultados")]
        public ActionResult ObterResultado()
        {
            var valores = new string[] { "value1", "value2" };
            if(valores.Length < 5000) 
                return BadRequest();
            return Ok(valores); 
        }

        [HttpGet("obter-valores")]
        public IEnumerable<string> ObterValores()
        {
            var valores = new string[] { "value1", "value2" };
            if(valores.Length < 5000) 
                return null;
            
            return valores; 
        }

        // GET api/values/obter-por-id/5
        [HttpGet("obter-por-id/{id}")]
        public ActionResult<string> Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        [ProducesResponseType(typeof(Product), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult Post(Product product)
        {
            if(product.Id == 0) return BadRequest();
            return CreatedAtAction(nameof(Post), product);
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromForm] Product value)
        {
        }

        // DELETE api/values/5
        [HttpDelete]
        public void Delete([FromQuery]int id)
        {
        }
    }

    [ApiController]
    public abstract class MainController : ControllerBase
    {
        protected ActionResult CustomResponse(object result = null)
        {
            if(OperacaoValida())
                return Ok(new {
                    sucess = true,
                    data = result
                });
            
            return BadRequest(new {
                sucess = false,
                errors = ObterErros()
            });

        }

        protected string ObterErros()
        {
            return "Deu Ruim cachoeira";
        }

        public bool OperacaoValida()
        {
            // as validações

            return true;
        }
        
    }

    public class Product
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }
        
    }
}
