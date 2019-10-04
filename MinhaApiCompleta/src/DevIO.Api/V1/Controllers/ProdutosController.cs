using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using AutoMapper;
using DevIO.Api.Controllers;
using DevIO.Api.Extensions;
using DevIO.Api.ViewModels;
using DevIO.Business.Interfaces;
using DevIO.Business.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DevIO.Api.V1.Controllers
{
    [Authorize]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class ProdutosController : MainController
    {
        private readonly IProdutoService _produtosService;
        private readonly IProdutoRepository _produtoRepository;
        private readonly IMapper _mapper;
        public ProdutosController(IProdutoService produtoService, 
                                  IProdutoRepository produtoRepository,
                                  IMapper mapper,
                                  INotificador notificador,
                                  IUser user) : base(notificador, user)
        {
            _produtosService = produtoService;
            _produtoRepository = produtoRepository;
            _mapper = mapper;
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IEnumerable<ProdutoViewModel>> ObterTodos()
        {
            return  _mapper.Map<IEnumerable<ProdutoViewModel>>(await _produtoRepository.ObterProdutosFornecedores());
        }

        [AllowAnonymous]
        [HttpGet("{id:guid}")]
        public async Task<ActionResult<ProdutoViewModel>> ObterPorId(Guid id)
        {
            var produtoViewModel = await ObterProduto(id);

            if(produtoViewModel == null) return NotFound();

            return Ok(produtoViewModel);
        }

        [ClaimsAuthorize("Produto", "Adicionar")]
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

        [ClaimsAuthorize("Produto", "Adicionar")]
        [RequestSizeLimit(40000000)]
        [HttpPost("Adicionar")]
        public async Task<ActionResult<ProdutoViewModel>> CriarProdutoAlt(ProdutoImagemViewModel produtoImagemViewModel, IFormFile ImagemUpload)
        {
            produtoImagemViewModel.ImagemUpload = ImagemUpload;
            if(!ModelState.IsValid) return CustomResponse(ModelState);

            var imgPrefix = Guid.NewGuid() + "_";

            if(!await UploadArquivoAlternativo(produtoImagemViewModel.ImagemUpload, imgPrefix))
            {
                return CustomResponse(produtoImagemViewModel);
            }

            produtoImagemViewModel.Imagem = imgPrefix + produtoImagemViewModel.ImagemUpload.FileName;

            await _produtosService.Adicionar(_mapper.Map<Produto>(produtoImagemViewModel));

            return CustomResponse(produtoImagemViewModel);

        }

        [RequestSizeLimit(40000000)]
        [HttpPost("imagem")]
        public async Task<ActionResult> AdicionarImagem(IFormFile file)
        {
            return Ok();
        }
        
        [ClaimsAuthorize("Produto", "Atualizar")]
        [HttpPut("{id:guid}")]
        public async Task<ActionResult<ProdutoViewModel>> Atualizar(Guid id, ProdutoViewModel produtoViewModel)
        {
            if(id != produtoViewModel.Id) 
            {
                NotificarErro("Os ids informados não são iguais.");
                return CustomResponse();
            }

            var produtoAtualizacao = await ObterProduto(id);

            produtoViewModel.Imagem = produtoAtualizacao.Imagem;

            if(!ModelState.IsValid) return CustomResponse(ModelState);

            if(produtoViewModel.ImagemUpload != null)
            {
                var imagemNome = Guid.NewGuid() + "_" + produtoViewModel.Imagem;
                if(!UploadArquivo(produtoViewModel.ImagemUpload, imagemNome))
                {
                    return CustomResponse(ModelState);
                }
                produtoAtualizacao.Imagem = imagemNome;
            }
            produtoAtualizacao.Nome = produtoViewModel.Nome;
            produtoAtualizacao.Descricao = produtoViewModel.Descricao;
            produtoAtualizacao.Valor = produtoViewModel.Valor;
            produtoAtualizacao.Ativo = produtoViewModel.Ativo;

            await _produtosService.Atualizar(_mapper.Map<Produto>(produtoAtualizacao));

            return CustomResponse(produtoViewModel);
            
        }

        [ClaimsAuthorize("Produto", "Excluir")]
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

        private async Task<bool> UploadArquivoAlternativo(IFormFile arquivo, string ImgPrefixo)
        {

            if(arquivo == null || arquivo.Length == 0)
            {
                NotificarErro("Forneça uma imagem para este produto!");
                return false;
            }
            

            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/imagens", ImgPrefixo + arquivo.FileName);
            
            if(System.IO.File.Exists(filePath))
            {
                NotificarErro("Já existe um arquivo com esse nome");
                return false;
            }

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await arquivo.CopyToAsync(stream);
            }

            return true;
        }
    }
}