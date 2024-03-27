import { Component } from '@angular/core';
import { Cart, CartItem, CartService, CartUpdatedInfo } from '../../services/cartService';

@Component({
  selector: 'app-cart',
  templateUrl: './cart.component.html',
  styleUrl: './cart.component.css'
})
export class CartComponent {
  cart: Cart = {
    customerId: '',
    items: [],
    totalPrice: 0
  };
  isCartEmpty: boolean = true;

  constructor(
    private cartService: CartService
  ){
  }

  ngOnInit() {
    this.cartService.getCart()
      .subscribe(res => {
        if(res.isSuccess) {
          this.cart = res.data
          this.isCartEmpty = res.data.items.length == 0;
        }
      })
  }

  onIncreaseQuantityClick(productId: string): void {
    const item = this.getCartItem(productId);
    const quantity = item.quantity + 1;

    this.cartService.addOrUpdate({
      productId: item.productId,
      quantity: quantity
    }).subscribe(res => {
      if(res.isSuccess) {
        this.reUpdateCartInfo(res.data);
      }
    })
  }

  onReduceQuantityClick(productId: string): void {
    const item = this.getCartItem(productId);
    if(item.quantity <= 1) return;

    const quantity = item.quantity - 1;

    this.cartService.addOrUpdate({
      productId: item.productId,
      quantity: quantity
    }).subscribe(res => {
      if(res.isSuccess) {
        this.reUpdateCartInfo(res.data);
      }
    })
  }

  onQuantityInputChange(productId: string, event: any): void {
    this.cartService.addOrUpdate({
      productId: productId,
      quantity: event.target.value
    }).subscribe(res => {
      if(res.isSuccess) {
        this.reUpdateCartInfo(res.data);
      }
    })
  }

  onUpdateCartItemClick(productId: string, isAdd: boolean): void {
    const item = this.getCartItem(productId);
    if(isAdd) {
      item.quantity++;
    } else {
      item.quantity--;
    }
  }

  onRemoveItemClick(productId: string): void {
    this.cartService.removeItem(productId)
      .subscribe(res => {
        if(res.isSuccess) {
          this.cart.items = this.cart.items.filter(e => e.productId !== productId);
          this.reUpdateCartInfo(res.data);
        }
      })
  }

  getCartItem(productId: string): CartItem {
    for(let i = 0; i < this.cart.items.length; i++) {
      if(this.cart.items[i].productId === productId) {
        return this.cart.items[i];
      }
    }

    return {} as CartItem
  }

  reUpdateCartInfo(cart: CartUpdatedInfo): void {
    const item = this.getCartItem(cart.productId);
    this.cart.totalPrice = cart.newTotalBasketPrice;
    if(item !== null) {
      item.quantity = cart.newQuantity;
    }
  }
}
