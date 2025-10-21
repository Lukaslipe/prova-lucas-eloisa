using System.Runtime.InteropServices;
using EloisaSantos.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<AppDataContext>();
var app = builder.Build();

app.MapGet("/", () => "API de consumo de água");

//POST: /api/consumo/cadastrar"
app.MapPost("/api/consumo/cadastrar", 
    async ([FromBody] Consumo consumo,
    [FromServices] AppDataContext ctx) =>
{
    if (consumo.mes < 1 || consumo.mes > 12)
    {
        return Results.BadRequest("Mês inválido!");
    }

    if (consumo.ano < 2000)
    {
        return Results.BadRequest("Ano inválido!");
    }

    if (consumo.m3Consumidos <= 0)
    {
        return Results.BadRequest("Valor inválido!");
    }

    var encontrado = await ctx.Consumos
        .Where(c => c.cpf.Contains(consumo.cpf))
        .Where(c => c.mes.Equals(consumo.mes))
        .Where(c => c.ano.Equals(consumo.ano))
        .ToListAsync();

    if (encontrado.Any())
    {
        return Results.Conflict("Já cadastrado!");
    }

    if (consumo.m3Consumidos < 10)
    {
        consumo.consumoFaturado = 10;
    }
    else
    {
        consumo.consumoFaturado = consumo.m3Consumidos;
    }

    if (consumo.m3Consumidos < 11)
    {
        consumo.tarifa = 2.50;
    }
    else if (consumo.m3Consumidos > 10 || consumo.m3Consumidos <= 20)
    {
        consumo.tarifa = 3.50;
    }
    else if (consumo.m3Consumidos > 20 || consumo.m3Consumidos <= 50)
    {
        consumo.tarifa = 5.00;
    }
    else
    {
        consumo.tarifa = 6.50;
    }

    consumo.valorAgua = consumo.consumoFaturado * consumo.tarifa;

    if (consumo.bandeira == "verde")
    {
        consumo.adicionalBandeira = 0;
    }
    else if (consumo.bandeira == "amarela")
    {
        consumo.adicionalBandeira = consumo.valorAgua * 0.10;
    }
    else if (consumo.bandeira == "vermelha")
    {
        consumo.adicionalBandeira = consumo.valorAgua * 0.20;
    }

    if (consumo.possuiEsgoto)
    {
        consumo.taxaEsgoto = (consumo.valorAgua + consumo.adicionalBandeira) * 0.80;
    }
    else
    {
        consumo.taxaEsgoto = 0;
    }

    consumo.total = consumo.valorAgua + consumo.adicionalBandeira + consumo.taxaEsgoto;

    ctx.Consumos.Add(consumo);
    return Results.Ok(consumo);
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