﻿using Grpc.Net.Client;

namespace Basket.API.RPC.Clients;

public class ProductRpcClient
{
  private readonly ProductRpc.ProductRpcClient _client;

  public ProductRpcClient(Func<string> option)
  {
    _client = new(GrpcChannel.ForAddress(option.Invoke()));
  }

  public async Task<GetProductReply> GetProduct(string productId)
  {
    return await _client.GetProductAsync(new GetProductDetailRequest { ProductId = productId });
  }

  public async Task<GetProductInBasketReply> GetProducts(params string[] productIds)
  {
    var request = new GetProductInBasketRequest();
    request.ProductIds.AddRange(productIds);

    return await _client.GetProductInBasketAsync(request);
  }
}
