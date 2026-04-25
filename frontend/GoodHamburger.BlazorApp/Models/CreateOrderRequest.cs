using System;
using System.Collections.Generic;

namespace GoodHamburger.BlazorApp.Models;

public class CreateOrderRequest
{
    public List<Guid> ItemIds { get; set; } = new();
}
