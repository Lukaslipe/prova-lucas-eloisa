using System.Runtime.InteropServices;
using EloisaSantos.Models;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<AppDataContext>();
var app = builder.Build();

app.MapGet("/", () => "API de consumo de água");

//POST: /api/produto/cadastrar
app.MapPost("/api/produto/cadastrar",
    ([FromBody] Consumo consumo,
    [FromServices] AppDataContext ctx) =>
{

    // Produto? resultado =
    //     ctx.Produtos.FirstOrDefault(x => x.Nome == produto.Nome);
    // if (resultado is null)
    // {
    //     ctx.Produtos.Add(produto);
    //     ctx.SaveChanges();
    //     return Results.Created("", produto);
    // }
    // else
    // {
    //     return Results.Conflict("Esse produto já existe!");
    // }
});

//GET:/api/consumo/listar
app.MapGet("/api/consumo/listar",
    ([FromServices] AppDataContext ctx) =>
{
    if (ctx.Consumos.Any())
    {
        return Results.Ok(ctx.Consumos.ToList());
    } 

    return Results.NotFound("Lista vazia!");
});

//GET: /api/consumo/buscar/nome_do_produto
app.MapGet("/api/consumo/buscar/{cpf}/{mes}/{ano}",
    ([FromRoute]string cpf, [FromRoute]string mes, [FromRoute]string ano,
    [FromServices] AppDataContext ctx) =>
{
    //expressão lambda
    // Produto? resultado = ctx.Produtos.FirstOrDefault(x => x.Nome == nome);
    // if (resultado == null)
    // {
    //     return Results.NotFound("Produto não encontrado!");
    // }
    // return Results.Ok(resultado);
});



app.MapDelete("/api/produto/remover/{cpf}/{mes}/{ano}",
    ([FromRoute]string cpf, [FromRoute]string mes, [FromRoute]string ano,
    [FromServices] AppDataContext ctx) =>
{
    // Produto? resultado = ctx.Produtos.Find(id);
    // if (resultado == null)
    // {
    //     return Results.NotFound("Produto não encontrado!");
    // }

    // ctx.Produtos.Remove(resultado);
    // ctx.SaveChanges();
    // return Results.Ok("Produto removido com sucesso!");
});

//GET:/api/consumo/listar
app.MapGet("/api/consumo/total-geral",
    ([FromServices] AppDataContext ctx) =>
{
    // if (ctx.Consumos.Any())
    // {
    //     return Results.Ok(ctx.Consumos.ToList());
    // } 

    // return Results.NotFound("Lista vazia!");
});

app.Run();