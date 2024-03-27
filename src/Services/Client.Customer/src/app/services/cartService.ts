import { HttpClient, HttpHeaders } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Result } from "./common";
import { AuthStateProvider } from "../authStateProvider";
import { Observable } from "rxjs";

type Cart = {
  customerId: string;
  totalPrice: number;
  items: Array<CartItem>
}

type CartItem = {
  productId: string;
  productName: string;
  price: number;
  quantity: number;
  pictureUri: string;
}

type CartUpdatedInfo = {
  productId: string;
  newQuantity: number;
  newTotalItemPrice: number;
  newTotalBasketPrice: number;
}

type CartUpdatedResponse = Result<CartUpdatedInfo>;

type CartResponse = Result<Cart>

interface AddOrUpdateItem {
  productId: string;
  quantity: number;
}

@Injectable({
  providedIn: 'root'
})
class CartService {
  _rootUrl: string = "https://localhost:7168/basket-api"

  constructor(
    private httpClient: HttpClient,
    private authProvider: AuthStateProvider) {
  }

  getCart(): Observable<CartResponse> {
    const header = new HttpHeaders({
      'Content-Type': 'application/json',
      'Authorization': `Bearer ${this.authProvider.getAccessToken()}`
    });
    return this.httpClient.get<CartResponse>(`${this._rootUrl}/baskets`, { headers: header });
  }

  addOrUpdate(payload: AddOrUpdateItem): Observable<CartUpdatedResponse> {
    const header = new HttpHeaders({
      'Content-Type': 'application/json',
      'Authorization': `Bearer ${this.authProvider.getAccessToken()}`
    });
    return this.httpClient.post<CartUpdatedResponse>(`${this._rootUrl}/baskets/update`, JSON.stringify(payload), {headers: header});
  }

  removeItem(productId: string): Observable<CartUpdatedResponse> {
    const header = new HttpHeaders({
      'ContentType': 'application/json',
      'Authorization': `Bearer ${this.authProvider.getAccessToken()}`
    });
    return this.httpClient.delete<CartUpdatedResponse>(`${this._rootUrl}/baskets/items/${productId}`, {headers: header});
  }
}

export {
  CartService,
  CartItem,
  Cart,
  CartUpdatedInfo
}