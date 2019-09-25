using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using DevIO.Api.ViewModels;
using DevIO.Business.Interfaces;
using DevIO.Business.Models;
using Microsoft.AspNetCore.Mvc;

namespace DevIO.Api.Controllers
{
    [Route("api/[controller]")]
    public class FornecedoresController : MainController
    {
        private readonly IFornecedorRepository _fornecedorRepository;
        private readonly IFornecedorService _fornecedorService;
        private readonly IMapper _mapper;

        
        public FornecedoresController(IFornecedorRepository fornecedorRepository, IFornecedorService fornecedorService, IMapper mapper)
        {
            _fornecedorRepository = fornecedorRepository;
            _fornecedorService = fornecedorService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IEnumerable<FornecedorViewModel>> ObterTodos()
        {
            var fornecedores = _mapper.Map<IEnumerable<FornecedorViewModel>>(await _fornecedorRepository.ObterTodos());

            return fornecedores;
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<FornecedorViewModel>> ObterPorId(Guid id)
        {
            var fornecedor = await ObterFornecedoProdutosEndereco(id);

            if (fornecedor == null) return BadRequest();
            
            return fornecedor;
        }

        [HttpPost]
        public async Task<ActionResult<FornecedorViewModel>> AdicionarFornecedor(FornecedorViewModel fornecedorView)
        {
            if(!ModelState.IsValid) return BadRequest();

            var fornecedor = _mapper.Map<Fornecedor>(fornecedorView);

            var result = await _fornecedorService.Adicionar(fornecedor);

            if(!result) return BadRequest();

            return Ok(fornecedor);
        }
        
        [HttpPut("{id:guid}")]
        public async Task<ActionResult<FornecedorViewModel>> AtualizarFornecedor(Guid id, FornecedorViewModel fornecedorViewModel)
        {
            if(id != fornecedorViewModel.Id) return BadRequest();

            if(!ModelState.IsValid) return BadRequest();

            var fornecedor = _mapper.Map<Fornecedor>(fornecedorViewModel);

            var result = await _fornecedorService.Atualizar(fornecedor);

            if(!result) return BadRequest();

            return Ok(fornecedor);
        }
        
        [HttpDelete("{id:guid}")]
        public async Task<ActionResult<FornecedorViewModel>> Excluir(Guid id)
        {
            var fornecedor =  await ObterFornecedorEndereco(id);
            if (fornecedor == null) return NotFound();

            var result = await _fornecedorService.Remover(id);
            
            if(!result) return BadRequest();

            return Ok(fornecedor);

            
        }        

        public async Task<FornecedorViewModel> ObterFornecedoProdutosEndereco(Guid id)
        {
            return _mapper.Map<FornecedorViewModel>(await _fornecedorRepository.ObterFornecedorProdutosEndereco(id));
        }

        public async Task<FornecedorViewModel> ObterFornecedorEndereco(Guid id)
        {
            return _mapper.Map<FornecedorViewModel>(await _fornecedorRepository.ObterFornecedorEndereco(id));
        }
    }
}