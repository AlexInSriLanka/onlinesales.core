﻿// <copyright file="CommentsController.cs" company="WavePoint Co. Ltd.">
// Licensed under the MIT license. See LICENSE file in the samples root for full license information.
// </copyright>

using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using OnlineSales.Configuration;
using OnlineSales.Data;
using OnlineSales.DTOs;
using OnlineSales.Entities;
using OnlineSales.Helpers;
using OnlineSales.Interfaces;

namespace OnlineSales.Controllers;

[Authorize]
[Route("api/[controller]")]
public class CommentsController : BaseControllerWithImport<Comment, CommentCreateDto, CommentUpdateDto, CommentDetailsDto, CommentImportDto>
{
    private readonly ICommentService commentService;

    public CommentsController(PgDbContext dbContext, IMapper mapper, IOptions<ApiSettingsConfig> apiSettingsConfig, ICommentService commentService, EsDbContext esDbContext)
        : base(dbContext, mapper, apiSettingsConfig, esDbContext)
    {
        this.commentService = commentService;
    }

    [HttpGet]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public override async Task<ActionResult<List<CommentDetailsDto>>> Get([FromQuery] string? query)
    {
        var returnedItems = (await base.Get(query)).Result;

        var items = (List<CommentDetailsDto>)((ObjectResult)returnedItems!).Value!;

        items.ForEach(c =>
        {
            c.AvatarUrl = GravatarHelper.EmailToGravatarUrl(c.AuthorEmail);
        });

        if (this.User.Identity != null && this.User.Identity.IsAuthenticated)
        {
            return this.Ok(items);
        }
        else
        {
            var commentsForAnonymous = this.mapper.Map<List<AnonymousCommentDetailsDto>>(items);

            return this.Ok(commentsForAnonymous);
        }
    }

    // GET api/{entity}s/5
    [HttpGet("{id}")]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public override async Task<ActionResult<CommentDetailsDto>> GetOne(int id)
    {
        var result = (await base.GetOne(id)).Result;

        var commentDetails = (CommentDetailsDto)((ObjectResult)result!).Value!;

        commentDetails!.AvatarUrl = GravatarHelper.EmailToGravatarUrl(commentDetails.AuthorEmail);

        if (this.User.Identity != null && this.User.Identity.IsAuthenticated)
        {
            return this.Ok(commentDetails!);
        }
        else
        {
            var commentForAnonymous = this.mapper.Map<AnonymousCommentDetailsDto>(commentDetails);

            return this.Ok(commentForAnonymous!);
        }
    }

    // POST api/{entity}s
    [HttpPost]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status422UnprocessableEntity)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public override async Task<ActionResult<CommentDetailsDto>> Post([FromBody] CommentCreateDto value)
    {
        var comment = this.mapper.Map<Comment>(value);

        await this.commentService.SaveAsync(comment);

        await this.dbContext.SaveChangesAsync();

        var returnedValue = this.mapper.Map<CommentDetailsDto>(comment);

        return this.CreatedAtAction(nameof(GetOne), new { id = comment.Id }, returnedValue);
    }

    protected override async Task SaveRangeAsync(List<Comment> comments)
    {
        await this.commentService.SaveRangeAsync(comments);
    }
}