import { Injectable, inject } from '@angular/core';
import { Observable } from 'rxjs';

import {
  ProductsClient,
  InventoryClient,
  StockMovementsClient,
  WareHousesClient,
  ProductDashboardDto,
  ProductDetailDto,
  CreateProductDto,
  CreateWareHouseDto,
  InventoryItemDto,
  StockMovementDto,
  WareHouseDto
} from '../api/api-client'; 

@Injectable({
  providedIn: 'root',
})
export class ProductService {
  private readonly productsClient = inject(ProductsClient);
  private readonly inventoryClient = inject(InventoryClient);
  private readonly stockMovementsClient = inject(StockMovementsClient);
  private readonly wareHousesClient = inject(WareHousesClient);

  getDashboard(): Observable<ProductDashboardDto[]> {
    return this.productsClient.products_GetDashboard(undefined, undefined);
  }

  getProductById(id: number): Observable<ProductDetailDto> {
    return this.productsClient.products_GetDetails(id, undefined, undefined);
  }

  createProduct(productData: CreateProductDto): Observable<ProductDetailDto> {
    return this.productsClient.products_CreateProduct(undefined, undefined, productData);
  }

  updateProduct(id: number, productData: CreateProductDto): Observable<ProductDetailDto> {
    return this.productsClient.products_UpdateProduct(id, undefined, undefined, productData);
  }

  deleteProduct(id: number): Observable<void> {
    return this.productsClient.products_DeleteProduct(id, undefined, undefined);
  }

  createWarehouse(warehouseData: CreateWareHouseDto): Observable<WareHouseDto> {
    return this.wareHousesClient.wareHouses_CreateWareHouse(undefined, undefined, warehouseData);
  }

  updateStockQuantity(productId: number, warehouseId: number, quantity: number): Observable<void> {
    const payload: InventoryItemDto = {
      productId: productId,
      wareHouseId: warehouseId,
      quantity: quantity
    };
    return this.inventoryClient.inventory_UpsertInventory(undefined, undefined, payload);
  }

  getProductHistory(productId: number): Observable<StockMovementDto[]> {
    return this.stockMovementsClient.stockMovements_GetProductHistory(productId, undefined, undefined);
  }

  getAllWarehouses(): Observable<WareHouseDto[]> {
    return this.wareHousesClient.wareHouses_GetAllWareHouses(undefined, undefined);
  }
}