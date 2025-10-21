using System.Runtime.InteropServices;
using EloisaSantos.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<AppDataContext>();
var app = builder.Build();

app.MapGet("/", () => "API de consumo de água");

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

    if (encontrado.Count != 0)
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

    if (consumo.bandeira == "Verde" || consumo.bandeira == "verde")
    {
        consumo.adicionalBandeira = 0;
    }
    else if (consumo.bandeira == "Amarela" || consumo.bandeira == "amarela")
    {
        consumo.adicionalBandeira = consumo.valorAgua * 0.10;
    }
    else if (consumo.bandeira == "Vermelha" || consumo.bandeira == "vermelha")
    {
        consumo.adicionalBandeira = consumo.valorAgua * 0.20;
    }
    else
    {
        return Results.BadRequest("Bandeira inválida!");
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
    ctx.SaveChanges();
    return Results.Ok(consumo);
});

app.MapGet("/api/consumo/listar",
    ([FromServices] AppDataContext ctx) =>
{
    if (ctx.Consumos.Any())
    {
        return Results.Ok(ctx.Consumos.ToList());
    } 

    return Results.NotFound("Lista vazia!");
});

app.MapGet("/api/consumo/buscar/{cpf}/{mes}/{ano}", async
    ([FromRoute]string cpf, [FromRoute]int mes, [FromRoute]int ano,
    [FromServices] AppDataContext ctx) =>
{

    var consumo = await ctx.Consumos
        .Where(c => c.cpf.Contains(cpf))
        .Where(c => c.mes.Equals(mes))
        .Where(c => c.ano.Equals(ano))
        .ToListAsync();

    return consumo.Any() ? Results.Ok(consumo) : Results.NotFound("Nenhum consumo encontrado.");
    
});


app.MapDelete("/api/consumo/remover/{cpf}/{mes}/{ano}", async
    ([FromRoute]string cpf, [FromRoute]int mes, [FromRoute]int ano,
    [FromServices] AppDataContext ctx) =>
{
    var consumo = await ctx.Consumos
        .Where(c => c.cpf.Contains(cpf))
        .Where(c => c.mes.Equals(mes))
        .Where(c => c.ano.Equals(ano))
        .ToListAsync();

    var encontrado = consumo.FirstOrDefault();

    if (encontrado != null)
    {
        ctx.Consumos.Remove(encontrado);
        ctx.SaveChanges();
        return Results.Ok("Consumo removido!");
    } else
    {
        return Results.NotFound("Consumo não encontrado.");
    }
});

app.MapGet("/api/consumo/total-geral", async
    ([FromServices] AppDataContext ctx) =>
{
    var somaTotal = await ctx.Consumos.SumAsync(c => c.total);

    return Results.Ok(somaTotal);

});

app.Run();