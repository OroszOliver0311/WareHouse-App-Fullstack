import { Component, inject, OnInit, signal, Output, EventEmitter } from '@angular/core'; 
import { CommonModule } from '@angular/common';
import { MatTableModule } from '@angular/material/table';
import { ProductService } from '../../services/product';
import { ProductDashboardDto } from '../../models/product.dto'; 

@Component({
  selector: 'app-dashboard',
  standalone: true,
  imports: [CommonModule, MatTableModule],
  templateUrl: './dashboard.html',
  styleUrls: ['./dashboard.scss']
})
export class Dashboard implements OnInit {
  private readonly productService = inject(ProductService);
  
  @Output() productSelected = new EventEmitter<number>();
  
  displayedColumns: string[] = [ 'name', 'sku', 'totalQuantity'];
  dataSource = signal<ProductDashboardDto[]>([]);

  ngOnInit(): void {
    this.productService.getDashboard().subscribe({
      next: (data) => this.dataSource.set(data),
      error: (err) => console.error('Hiba:', err)
    });
  }

  onRowClick(id: number): void {
    this.productSelected.emit(id);
  }
}