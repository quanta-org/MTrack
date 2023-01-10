using asp.net.Models;
using asp.net.Services;
using System;

using Microsoft.AspNetCore.Mvc;

namespace asp.net.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PizzaController : ControllerBase
{
    [HttpGet("{id}")]
    public ActionResult<Pizza> Get(int id)
    {
        var pizza = PizzaService.Get(id);

        if(pizza == null){
            return NotFound();
        }

        return pizza;
    }

    [HttpGet]
    public ActionResult<List<Pizza>> GetAll() => PizzaService.GetAll();

    [HttpPost]
    public void PostPizza(Pizza pizza) => PizzaService.Add(pizza.Name, pizza.IsGlutenFree);
}