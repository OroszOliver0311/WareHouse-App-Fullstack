import { Component, inject, Input, OnInit, signal, Output, EventEmitter } from '@angular/core';
import { MatTableModule } from '@angular/material/table';
import { MatButtonModule } from '@angular/material/button';
import { ProductService } from '../../services/product';
import { ProductDetailDto } from '../../models/product.dto';

@Component({
  selector: 'app-product-detail',
  standalone: true,
  imports: [MatTableModule, MatButtonModule], 
  templateUrl: './product-detail.html',
  styleUrls: ['./product-detail.scss']
})
export class ProductDetail implements OnInit {
  private readonly productService = inject(ProductService);

  @Input() productId!: number;
  @Output() back = new EventEmitter<void>(); 
  
  product = signal<ProductDetailDto | null>(null);
  displayedColumns: string[] = ['warehouse', 'location', 'quantity'];

  ngOnInit(): void {
    if (this.productId) {
      this.productService.getProductById(this.productId).subscribe({
        next: (data) => this.product.set(data),
        error: (err) => console.error('Error loading product details:', err)
      });
    }
  }

  goBack(): void {
    this.back.emit();
  }
}