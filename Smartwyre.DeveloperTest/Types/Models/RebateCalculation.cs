﻿using Smartwyre.DeveloperTest.Types.Enums;

namespace Smartwyre.DeveloperTest.Types.Models;

public class RebateCalculation
{
    public int Id { get; set; }
    public string Identifier { get; set; }
    public string RebateIdentifier { get; set; }
    public IncentiveType IncentiveType { get; set; }
    public decimal Amount { get; set; }
}
