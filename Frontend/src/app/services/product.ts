import { HttpClient } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { Observable } from 'rxjs';
import { ProductDashboardDto, ProductDetailDto } from '../models/product.dto';

@Injectable({
  providedIn: 'root',
})
export class ProductService {
private readonly http = inject(HttpClient);
private readonly productApiUrl = 'https://localhost:7122/api/Products';
private readonly inventoryApiUrl = 'https://localhost:7122/api/Inventory';
private readonly stockMovementApiUrl = 'https://localhost:7122/api/StockMovements';
private readonly warehouseApiUrl = 'https://localhost:7122/api/WareHouses';

getDashboard(): Observable<ProductDashboardDto[]> {
  return this.http.get<ProductDashboardDto[]>(`${this.productApiUrl}/dashboard`);
}
getProductById(id: number): Observable< ProductDetailDto> {
  return this.http.get<ProductDetailDto>(`${this.productApiUrl}/${id}`);
}

createProduct(productData: any): Observable<any> {
  return this.http.post(this.productApiUrl, productData);
}
updateProduct(id: number, productData: any): Observable<any> {
  return this.http.put(`${this.productApiUrl}/${id}`, productData);
}
deleteProduct(id: number): Observable<any> {
  return this.http.delete(`${this.productApiUrl}/${id}`);
}
createWarehouse(warehouseData: any): Observable<any> {
  return this.http.post(this.warehouseApiUrl, warehouseData);
}
updateStockQuantity(productId: number, warehouseId: number, quantity: number): Observable<any> {
  const payload = {
    productId: productId,
    wareHouseId: warehouseId,
    quantity: quantity
  };

  return this.http.post(`${this.inventoryApiUrl}/upsert`, payload);
}
getProductHistory(productId: number): Observable<any[]> {
  return this.http.get<any[]>(`${this.stockMovementApiUrl}/product/${productId}`);
}

getAllWarehouses(): Observable<any[]> {
  return this.http.get<any[]>(this.warehouseApiUrl);
}
}
