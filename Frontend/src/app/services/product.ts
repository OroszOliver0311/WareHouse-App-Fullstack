import { HttpClient } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { Observable } from 'rxjs';
import { ProductDashboardDto, ProductDetailDto } from '../models/product.dto';

@Injectable({
  providedIn: 'root',
})
export class ProductService {
private readonly http = inject(HttpClient);
private readonly apiUrl = 'https://localhost:7122/api/Products';

getDashboard(): Observable<ProductDashboardDto[]> {
    return this.http.get<ProductDashboardDto[]>(`${this.apiUrl}/dashboard`);
  }
getProductById(id: number): Observable< ProductDetailDto> {
  return this.http.get<ProductDetailDto>(`${this.apiUrl}/${id}`);
}



}
