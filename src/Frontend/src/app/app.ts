import { Component, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Dashboard } from './components/dashboard/dashboard'; 
import { ProductDetail } from './components/product-detail/product-detail';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [CommonModule, Dashboard, ProductDetail], 
  templateUrl: './app.html',
  styleUrls: ['./app.scss'] 
})
export class App {
  title = 'Warehouse Management System';
  
  selectedProductId = signal<number | null>(null);

  selectProduct(id: number): void {
    this.selectedProductId.set(id);
  }

  backToDashboard(): void {
    this.selectedProductId.set(null);
  }
}