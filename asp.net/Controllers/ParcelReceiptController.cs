using asp.net.Models;
using asp.net.Services;
using Microsoft.AspNetCore.Mvc;

namespace asp.net.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ParcelReceiptController : ControllerBase
{
    [HttpGet]
    public ActionResult<List<ParcelReceipt>?> GetAll() => ParcelReceiptService.GetAll();

    [HttpPost]
    public void AddReceipt(ParcelReceipt receipt) => ParcelReceiptService.Add(receipt);

    [ApiExplorerSettings(IgnoreApi = true)]
    [Route("/error")]
    public IActionResult HandleError() => Problem();
}