﻿// <copyright file="EmailTemplatesTests.cs" company="WavePoint Co. Ltd.">
// Licensed under the MIT license. See LICENSE file in the samples root for full license information.
// </copyright>

namespace OnlineSales.Tests;

public class EmailTemplatesTests : TableWithFKTests<EmailTemplate, TestEmailTemplate, EmailTemplateUpdateDto, ISaveService<EmailTemplate>>
{
    public EmailTemplatesTests()
        : base("/api/email-templates")
    {
    }

    protected override async Task<(TestEmailTemplate, string)> CreateItem(string uid, int fkId)
    {
        var emailTemplate = new TestEmailTemplate(uid, fkId);

        var emailTemplateUrl = await this.PostTest(this.itemsUrl, emailTemplate);

        return (emailTemplate, emailTemplateUrl);
    }

    protected override async Task<(int, string)> CreateFKItem(string authToken = "Success")
    {
        var fkItemCreate = new TestEmailGroup();

        var fkUrl = await this.PostTest("/api/email-groups", fkItemCreate, HttpStatusCode.Created, authToken);

        var fkItem = await this.GetTest<EmailGroup>(fkUrl);

        fkItem.Should().NotBeNull();

        return (fkItem!.Id, fkUrl);
    }

    protected override EmailTemplateUpdateDto UpdateItem(TestEmailTemplate to)
    {
        var from = new EmailTemplateUpdateDto();
        to.Name = from.Name = to.Name + "Updated";
        return from;
    }
}