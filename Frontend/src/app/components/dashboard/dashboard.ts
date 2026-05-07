import { Component, inject, OnInit, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatTableModule } from '@angular/material/table';
import { ProductService } from '../../services/product'; // Pontos név a kép alapján
import { ProductDashboardDto } from '../../models/product.dto'; // Pontos név a kép alapján

@Component({
  selector: 'app-dashboard',
  standalone: true,
  imports: [CommonModule, MatTableModule],
  templateUrl: './dashboard.html',
  styleUrls: ['./dashboard.scss']
})
export class Dashboard implements OnInit {
  private readonly productService = inject(ProductService);
  
  displayedColumns: string[] = ['id', 'name', 'sku', 'totalQuantity'];
  
  // Signal használata a hiba ellen
  dataSource = signal<ProductDashboardDto[]>([]);

  ngOnInit(): void {
    this.productService.getDashboard().subscribe({
      next: (data) => {
        this.dataSource.set(data); // Adat beállítása
      },
      error: (err) => console.error('Hiba:', err)
    });
  }
}