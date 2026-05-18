import { Component, inject, Input, OnInit, signal, Output, EventEmitter } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatTableModule } from '@angular/material/table';
import { MatButtonModule } from '@angular/material/button';
import { FormsModule } from '@angular/forms'; 
import { ProductService } from '../../services/product';
import { ProductDetailDto } from '../../models/product.dto';

@Component({
  selector: 'app-product-detail',
  standalone: true,
  imports: [CommonModule, MatTableModule, MatButtonModule, FormsModule], 
  templateUrl: './product-detail.html',
  styleUrls: ['./product-detail.scss']
})
export class ProductDetail implements OnInit {
  private readonly productService = inject(ProductService);

  @Input() productId!: number;
  @Output() back = new EventEmitter<void>();
  
  product = signal<ProductDetailDto | null>(null);
  
  editedData: any = {}; 
  isDirty: boolean = false; 

  displayedColumns: string[] = ['warehouse', 'location', 'quantity'];

  ngOnInit(): void {
    if (this.productId) {
      this.loadProduct();
    }
  }

  loadProduct(): void {
    this.productService.getProductById(this.productId).subscribe({
      next: (data) => {
        this.product.set(data);
        this.editedData = { 
          name: data.name, 
          sku: data.sku, 
          unitPrice: data.unitPrice 
        };
        this.isDirty = false; 
      },
      error: (err) => console.error('Hiba a betöltéskor:', err)
    });
  }

  onFieldChange(): void {
    const original = this.product();
    if (!original) return;

    this.isDirty = 
      this.editedData.name !== original.name ||
      this.editedData.sku !== original.sku ||
      this.editedData.unitPrice !== original.unitPrice;
  }

  saveChanges(): void {
    if (!this.isDirty) return;

    this.productService.updateProduct(this.productId, this.editedData).subscribe({
      next: () => {
        this.loadProduct(); 
        alert('Product updated successfully!');
      },
      error: (err) => console.error('Hiba mentéskor:', err)
    });
  }

  goBack(): void {
    this.back.emit();
  }
}