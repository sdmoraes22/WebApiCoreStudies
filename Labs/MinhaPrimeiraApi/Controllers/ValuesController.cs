using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MinhaPrimeiraApi.Controllers
{
    [Route("api/[controller]")]
    [ApiConventionType(typeof(DefaultApiConventions))]
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
        // [ApiConventionMethod( typeof(DefaultApiConventions), nameof(DefaultApiConventions.Post))]
        public ActionResult Post(Product product)
        {
            if(product.Id == 0) return BadRequest();

            //add no banco
            // return Ok();
            return CreatedAtAction(nameof(Post), product);
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        [HttpPost]
        // [ApiConventionMethod( typeof(DefaultApiConventions), nameof(DefaultApiConventions.Put))]
        public ActionResult Put(int id, [FromForm] Product product)
        {
            if(!ModelState.IsValid) return BadRequest();

            if(id != product.Id)
                return NotFound();

            return NoContent();
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
