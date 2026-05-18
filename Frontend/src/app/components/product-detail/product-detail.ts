import { Component, inject, Input, OnInit, signal, Output, EventEmitter, ChangeDetectorRef } from '@angular/core';
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
  private readonly cdr = inject(ChangeDetectorRef);

  @Input() productId!: number;
  @Output() back = new EventEmitter<void>();
  
  product = signal<ProductDetailDto | null>(null);
  
  editedData: any = {}; 
  isDirty: boolean = false; 

  displayedColumns: string[] = ['warehouse', 'location', 'quantity', 'actions'];
  history = signal<any[]>([]);
  originalStocks: { [warehouseId: number]: number } = {};

  ngOnInit(): void {
    if (this.productId) {
      this.loadProduct();
      this.loadHistory();
    }
  }
  loadHistory(): void {
    this.productService.getProductHistory(this.productId).subscribe({
      next: (data) => {
        this.history.set(data);
      },
      error: (err) => console.error('Error loading history:', err)
    });
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

        this.originalStocks = {};
        if (data.stocks) {
          data.stocks.forEach((stock: any) => {
            this.originalStocks[stock.id] = stock.quantity; 
          });
        }
      },
      error: (err) => console.error('Error loading product:', err)
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
      },
      error: (err) => console.error('Error saving changes:', err)
    });
  }

isStockDirty(warehouseId: number, currentQuantity: any): boolean {
    return this.originalStocks[warehouseId] !== Number(currentQuantity);
  }

  updateStock(warehouseId: number, currentQuantity: any): void {
    const quantityAsNumber = Number(currentQuantity);

    this.productService.updateStockQuantity(this.productId, warehouseId, quantityAsNumber).subscribe({
      next: () => {
        this.originalStocks = {
          ...this.originalStocks,
          [warehouseId]: quantityAsNumber
        };
        
        this.cdr.detectChanges();
        this.loadHistory();
      },
      error: (err) => {
        console.error('Error updating stock:', err);
      }
    });
  }

  goBack(): void {
    this.back.emit();
  }
}