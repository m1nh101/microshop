import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Observable } from "rxjs";
import { Result } from "./common";

type Product = {
  id: string;
  name: string;
  availableStock: number;
  price: number;
  brandName: string;
  typeName: string;
  description: string;
  pictureUrl: string;
}

type FilerSelectOption = {
  value: string;
  label: string;
}

type ProductResponse = Result<Array<Product>>

type FilterOptionResponse = Result<{
  typeOptions: Array<FilerSelectOption>;
  brandOptions: Array<FilerSelectOption>;
}>;

interface FilterProductRequest {
  name: string;
  brandId: string;
  typeId: string;
}

@Injectable({
  providedIn: 'root'
})
class ProductService {
  private _rootUrl: string = "https://localhost:7168/product-api"

  constructor(private httpClient: HttpClient){
  }

  GetListSelectOption(): Observable<FilterOptionResponse> {
    return this.httpClient.get<FilterOptionResponse>(`${this._rootUrl}/products/list-option`);
  }

  GetProduct(payload: FilterProductRequest): Observable<ProductResponse> {
    const url: string = `${this._rootUrl}/products?name=${payload.name}&brands=${payload.brandId}&typeId=${payload.typeId}`;
    return this.httpClient.get<ProductResponse>(url);
  }
}

export { ProductService, FilerSelectOption, FilterProductRequest, Product }