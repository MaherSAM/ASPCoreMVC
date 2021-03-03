import { __decorate } from "tslib";
import { Component } from '@angular/core';
let ProductList = class ProductList {
    constructor(dataService) {
        this.dataService = dataService;
        this.products = [];
    }
    ngOnInit() {
        this.dataService.loadProducts().subscribe(success => {
            if (success) {
                this.products = this.dataService.products;
            }
        });
    }
    addProduct(product) {
        this.dataService.addToOrder(product);
    }
};
ProductList = __decorate([
    Component({
        selector: 'product-list',
        templateUrl: "./productList.component.html",
        styleUrls: ["./productList.component.css"]
    })
], ProductList);
export { ProductList };
//# sourceMappingURL=productList.component.js.map