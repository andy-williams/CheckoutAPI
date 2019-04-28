using System;
using System.Linq;
using System.Threading.Tasks;
using Checkout.RecruitmentTest.API.DTOs.Requests;
using Checkout.RecruitmentTest.API.DTOs.Responses;
using Checkout.RecruitmentTest.API.Handlers.Commands;
using Checkout.RecruitmentTest.API.Handlers.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Checkout.RecruitmentTest.API.Controllers
{
    [Route("basket")]
    public class BasketController : ControllerBase
    {
        private readonly IMediator _mediatr;

        public BasketController(IMediator mediatr)
        {
            _mediatr = mediatr;
        }

        [HttpPost]
        public async Task<IActionResult> CreateBasket()
        {
            var newBasketId = await _mediatr.Send(new CreateBasketCommand());

            return Ok(new BasketCreatedResponse { BasketId = newBasketId });
        }

        [HttpGet("{basketId:guid}")]
        public async Task<IActionResult> GetBasketItems(Guid basketId)
        {
            var basketItems = await _mediatr.Send(new GetBasketItemsQuery { BasketId = basketId });
            return Ok(new GetBasketResponse { BasketItems = basketItems });
        }

        [HttpPost("{basketId:guid}")]
        public async Task<IActionResult> AddBasketItem(Guid basketId, [FromBody] AddBasketItemRequest basketItem)
        {
            if (ModelState.IsValid)
            {
                var basketItemId = await _mediatr.Send(new AddBasketItemCommand
                {
                    BasketId = basketId,
                    Name = basketItem.Name,
                    Price = basketItem.Price,
                    Quantity = basketItem.Quantity
                });

                return Ok(new AddBasketItemResponse { BasketItemId = basketItemId });
            }

            var errorList = ModelState.ToDictionary(
                kvp => kvp.Key,
                kvp => kvp.Value.Errors.Select(e => e.ErrorMessage).ToArray()
            );
            return BadRequest(errorList);

        }

        [HttpPut("{basketId:guid}/{basketItemId:guid}")]
        public async Task<IActionResult> UpdateBasketItem(Guid basketId, Guid basketItemId, [FromBody] UpdateBasketItemRequest basketItem)
        {
            if (ModelState.IsValid)
            {
                await _mediatr.Send(new UpdateBasketItemCommand
                {
                    BasketId = basketId,
                    BasketItemId = basketItemId,
                    Quantity = basketItem.Quantity
                });
                return Ok();
            }

            var errorList = ModelState.ToDictionary(
                kvp => kvp.Key,
                kvp => kvp.Value.Errors.Select(e => e.ErrorMessage).ToArray()
            );
            return BadRequest(errorList);
        }

        [HttpDelete("{basketId:guid}/{basketItemId:guid}")]
        public IActionResult DeleteBasketItem(Guid basketItemId)
        {
            return Ok();
        }
    }
}
