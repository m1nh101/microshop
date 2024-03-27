import { Component } from '@angular/core';
import { FilerSelectOption, FilterProductRequest, Product, ProductService } from '../../services/productService';
import { CartService } from '../../services/cartService';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrl: './home.component.css'
})
export class HomeComponent {
  name: string = "";
  brandId: string = "";
  typeId: string = "";
  brandOptions: Array<FilerSelectOption> = [];
  typeOptions: Array<FilerSelectOption> = [];
  filterPayload: FilterProductRequest = {
    name: "",
    brandId: "",
    typeId: ""
  }
  products: Array<Product> = [];

  constructor(
    private productService: ProductService,
    private cartService: CartService) {
   }


  ngOnInit() {
    this.productService.GetListSelectOption()
      .subscribe(res => {
        if (res.isSuccess) {
          this.brandOptions = res.data.brandOptions;
          this.typeOptions = res.data.typeOptions;
        }
      });

    this.searchProduct();
  }

  onAddToCartClick(productId: string): void {
    this.cartService.addOrUpdate({
      productId: productId,
      quantity: 1
    }).subscribe(res => {
      if(res.isSuccess) {
        console.log("them gio hang thanh cong");
      }
    });
  }

  searchProduct(): void {
    this.productService.GetProduct(this.filterPayload)
      .subscribe(res => {
        if (res.isSuccess) {
          this.products = res.data;
        }
      })
  }
}
