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
    return this.productsClient.products_GetDashboard(undefined);
  }

  getProductById(id: string): Observable<ProductDetailDto> {
    return this.productsClient.products_GetDetails(id, undefined);
  }

  createProduct(productData: CreateProductDto): Observable<ProductDetailDto> {
    return this.productsClient.products_CreateProduct(undefined, productData);
  }

  updateProduct(id: string, productData: CreateProductDto): Observable<ProductDetailDto> {
    return this.productsClient.products_UpdateProduct(id, undefined, productData);
  }

  deleteProduct(id: string): Observable<void> {
    return this.productsClient.products_DeleteProduct(id, undefined);
  }

  createWarehouse(warehouseData: CreateWareHouseDto): Observable<WareHouseDto> {
    return this.wareHousesClient.wareHouses_CreateWareHouse(undefined, warehouseData);
  }

  updateStockQuantity(productId: string, warehouseId: string, quantity: number): Observable<void> {
    const payload: InventoryItemDto = {
      productId: productId,
      wareHouseId: warehouseId,
      quantity: quantity
    };
    return this.inventoryClient.inventory_UpsertInventory(undefined, payload);
  }

  getProductHistory(productId: string): Observable<StockMovementDto[]> {
    return this.stockMovementsClient.stockMovements_GetProductHistory(productId, undefined);
  }

  getAllWarehouses(): Observable<WareHouseDto[]> {
    return this.wareHousesClient.wareHouses_GetAllWareHouses(undefined);
  }
}