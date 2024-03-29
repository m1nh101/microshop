﻿namespace API.Contract.Products.Requests;

public record EditProductRequest(
  string Id,
  string Name,
  int AvailableStock,
  double Price,
  string PictureUri,
  string BrandId,
  string TypeId,
  string Description = "");
