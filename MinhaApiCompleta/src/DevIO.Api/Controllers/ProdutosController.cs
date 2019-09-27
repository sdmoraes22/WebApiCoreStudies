using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using AutoMapper;
using DevIO.Api.ViewModels;
using DevIO.Business.Interfaces;
using DevIO.Business.Models;
using Microsoft.AspNetCore.Mvc;

namespace DevIO.Api.Controllers
{
    [Route("api/[controller]")]
    public class ProdutosController : MainController
    {
        private readonly IProdutoService _produtosService;
        private readonly IProdutoRepository _produtoRepository;
        private readonly IMapper _mapper;
        public ProdutosController(IProdutoService produtoService, 
                                  IProdutoRepository produtoRepository,
                                  IMapper mapper,
                                  INotificador notificador) : base(notificador)
        {
            _produtosService = produtoService;
            _produtoRepository = produtoRepository;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IEnumerable<ProdutoViewModel>> ObterTodos()
        {
            return  _mapper.Map<IEnumerable<ProdutoViewModel>>(await _produtoRepository.ObterProdutosFornecedores());
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<ProdutoViewModel>> ObterPorId(Guid id)
        {
            var produtoViewModel = await ObterProduto(id);

            if(produtoViewModel == null) return NotFound();

            return Ok(produtoViewModel);
        }

        [HttpPost]
        public async Task<ActionResult<ProdutoViewModel>> CriarProduto(ProdutoViewModel produtoViewModel)
        {
            if(!ModelState.IsValid) return CustomResponse(ModelState);

            var imagemNome = Guid.NewGuid() + "_" + produtoViewModel.Imagem;

            if(!UploadArquivo(produtoViewModel.ImagemUpload, imagemNome))
            {
                return CustomResponse(produtoViewModel);
            }

            produtoViewModel.Imagem = imagemNome;

            await _produtosService.Adicionar(_mapper.Map<Produto>(produtoViewModel));

            return CustomResponse(produtoViewModel);

        }

        [HttpPut("{id:guid}")]
        public async Task<ActionResult<ProdutoViewModel>> CriarProduto(Guid id, ProdutoViewModel produtoViewModel)
        {
            if(id != produtoViewModel.Id) return BadRequest();

            if(!ModelState.IsValid) return CustomResponse(ModelState);

            var imagemNome = Guid.NewGuid() + "_" + produtoViewModel.Imagem;

            if(!UploadArquivo(produtoViewModel.ImagemUpload, imagemNome))
            {
                return CustomResponse(produtoViewModel);
            }

            produtoViewModel.Imagem = imagemNome;

            await _produtosService.Atualizar(_mapper.Map<Produto>(produtoViewModel));

            return CustomResponse(produtoViewModel);

        }

        [HttpDelete]
        public async Task<ActionResult<ProdutoViewModel>> Excluir(Guid id)
        {
            var produtoViewModel = ObterProduto(id);
            
            if (produtoViewModel == null) return NotFound();

            await _produtosService.Remover(id);

            return CustomResponse(produtoViewModel);
        }

        private async Task<ProdutoViewModel> ObterProduto(Guid id)
        {
            return _mapper.Map<ProdutoViewModel>(await _produtoRepository.ObterPorId(id));
        }

        private bool UploadArquivo(string arquivo, string imgNome)
        {

            if(string.IsNullOrEmpty(arquivo))
            {
                NotificarErro("Forneça uma imagem para este produto!");
                return false;
            }
            
            var imgByteDataArray = Convert.FromBase64String(arquivo);

            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/imagens", imgNome);
            
            if(System.IO.File.Exists(filePath))
            {
                NotificarErro("Já existe um arquivo com esse nome");
                return false;
            }

            System.IO.File.WriteAllBytes(filePath, imgByteDataArray);

            return true;
        }
    }
}