﻿// <copyright file="DomainTests.cs" company="WavePoint Co. Ltd.">
// Licensed under the MIT license. See LICENSE file in the samples root for full license information.
// </copyright>

using FluentAssertions;
using OnlineSales.DTOs;
using OnlineSales.Entities;

namespace OnlineSales.Tests;
public class DomainTests : SimpleTableTests<Domain, TestDomain, DomainUpdateDto>
{
    public DomainTests()
        : base("/api/domains")
    {
    }
                
    [Fact]
    public async Task CreateAndGetItemByNameTest()
    {
        var testDomain = new TestDomain();

        var domainName = testDomain.Name;

        await PostTest(itemsUrl, testDomain);

        var url = itemsUrl + "/verify/" + domainName;

        var item = await GetTest<Domain>(url, HttpStatusCode.OK);

        item.Should().NotBeNull();
    }

    [Fact]
    public async Task GetNewValidDomainTest()
    {
        var domainName = "gmail.com";

        var url = itemsUrl + "/verify/" + domainName;

        var item = await GetTest<Domain>(url, HttpStatusCode.OK);

        item.Should().NotBeNull();

        item!.Name.Should().Be(domainName);
        item!.HttpCheck.Should().BeTrue();
        item!.DnsCheck.Should().BeTrue();
        item!.DnsRecords.Should().NotBeNull();
    }

    [Fact]
    public async Task GetNewInvalidDomainTest()
    {
        var domainName = "SomeIncorrectDomainName";

        var url = itemsUrl + "/verify/" + domainName;

        var item = await GetTest<Domain>(url, HttpStatusCode.OK);

        item.Should().NotBeNull();

        item!.Name.Should().Be(domainName);
        item!.DnsCheck.Should().BeFalse();
        item!.DnsRecords.Should().BeNull();
        item!.HttpCheck.Should().BeNull();        
    }

    protected override DomainUpdateDto UpdateItem(TestDomain to)
    {
        var from = new DomainUpdateDto();
        to.Name = from.Name = to.Name + "Updated";
        return from;
    }
}