import { Component, inject, OnInit, signal, Output, EventEmitter } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatTableModule } from '@angular/material/table';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { FormsModule } from '@angular/forms'; 
import { ProductService } from '../../services/product';
import { ProductDashboardDto } from '../../models/product.dto';

@Component({
  selector: 'app-dashboard',
  standalone: true,
  imports: [CommonModule, MatTableModule, MatButtonModule, MatIconModule, FormsModule],
  templateUrl: './dashboard.html',
  styleUrls: ['./dashboard.scss']
})
export class Dashboard implements OnInit {
  private readonly productService = inject(ProductService);

  @Output() productSelected = new EventEmitter<number>();

  dataSource = signal<ProductDashboardDto[]>([]);
  displayedColumns: string[] = ['name', 'sku', 'totalQuantity', 'actions'];

  newProduct = {
    name: '',
    sku: '',
    unitPrice: null as number | null
  };
  newWarehouse = {
    name: '',
    location: ''
  };
  ngOnInit(): void {
    this.loadDashboard();
  }

  loadDashboard(): void {
    this.productService.getDashboard().subscribe({
      next: (data) => this.dataSource.set(data),
      error: (err) => console.error('Error loading dashboard:', err)
    });
  }

  onRowClick(id: number): void {
    this.productSelected.emit(id);
  }

isFormValid(): boolean {
    return !!this.newProduct.name && 
           !!this.newProduct.sku && 
           this.newProduct.unitPrice !== null && 
           this.newProduct.unitPrice > 0;
  }

  addProduct(): void {
    if (!this.isFormValid()) return;

    this.productService.createProduct(this.newProduct).subscribe({
      next: () => {
        this.newProduct = { name: '', sku: '', unitPrice: null };
        this.loadDashboard(); 
      },
      error: (err) => console.error('Error creating product:', err)
    });
  }

  deleteProduct(id: number, event: Event): void {
    event.stopPropagation();
    
    this.productService.deleteProduct(id).subscribe({
      next: () => {
        this.loadDashboard();
      },
      error: (err) => console.error('Error deleting product:', err)
    });
  }
  isWarehouseFormValid(): boolean {
    return !!this.newWarehouse.name && !!this.newWarehouse.location;
  }

  addWarehouse(): void {
    if (!this.isWarehouseFormValid()) return;

    this.productService.createWarehouse(this.newWarehouse).subscribe({
      next: () => {
        this.newWarehouse = { name: '', location: '' };
        this.loadDashboard();
      },
      error: (err) => console.error('Error creating warehouse:', err)
    });
  }


}