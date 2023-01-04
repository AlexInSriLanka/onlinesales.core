﻿// <copyright file="CustomersTests.cs" company="WavePoint Co. Ltd.">
// Licensed under the MIT license. See LICENSE file in the samples root for full license information.
// </copyright>

using OnlineSales.DTOs;
using OnlineSales.Entities;
using OnlineSales.Tests.TestEntities;
using OnlineSales.Tests.TestEntities.BulkPopulate;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace OnlineSales.Tests;
public class CustomersTests : SimpleTableTests<Customer, TestCustomer, CustomerUpdateDto, TestBulkCustomers>
{
    public CustomersTests()
        : base("/api/customers")
    {
    }

    protected override CustomerUpdateDto UpdateItem(TestCustomer to)
    {
        var from = new CustomerUpdateDto();
        to.Email = from.Email = "Updated" + to.Email;
        return from;
    }
}