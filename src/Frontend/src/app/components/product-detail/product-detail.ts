import { Component, inject, Input, OnInit, signal, Output, EventEmitter, ChangeDetectorRef } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatTableModule } from '@angular/material/table';
import { MatButtonModule } from '@angular/material/button';
import { FormsModule } from '@angular/forms'; 
import { ProductService } from '../../services/product';
import { ProductDetailDto } from '../../models/product.dto';
import { MatIconModule } from '@angular/material/icon';

@Component({
  selector: 'app-product-detail',
  standalone: true,
imports: [CommonModule, MatTableModule, MatButtonModule, MatIconModule, FormsModule],
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

  originalStocks: { [warehouseId: number]: number } = {};
  history = signal<any[]>([]);

  allWarehouses: any[] = [];
  availableWarehouses: any[] = [];
  
  newAssignment = {
    warehouseId: null as number | null,
    quantity: 0
  };


  searchTerm: string = '';
  isDropdownOpen: boolean = false;

  ngOnInit(): void {
    if (this.productId) {
      this.loadAllWarehouses();
      this.loadProduct();
      this.loadHistory();
    }
  }

  loadAllWarehouses(): void {
    this.productService.getAllWarehouses().subscribe({
      next: (data) => {
        this.allWarehouses = data;
        this.updateAvailableWarehouses();
      },
      error: (err) => console.error('Error loading warehouses:', err)
    });
  }

loadProduct(): void {
    this.productService.getProductById(this.productId).subscribe({
      next: (data) => {
        if (data.stocks) {
          data.stocks = data.stocks.filter((s: any) => s.quantity > 0);
        }

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
        
        this.updateAvailableWarehouses();
      },
      error: (err) => console.error('Error loading product:', err)
    });
  }

  updateAvailableWarehouses(): void {
    const currentProduct = this.product();
    if (!currentProduct || this.allWarehouses.length === 0) return;

    const assignedIds = currentProduct.stocks.map((s: any) => s.id);
    this.availableWarehouses = this.allWarehouses.filter(wh => !assignedIds.includes(wh.id));
  }


  filteredWarehouses(): any[] {
    const search = this.searchTerm.toLowerCase();
    return this.availableWarehouses.filter(wh => 
      wh.name.toLowerCase().includes(search) || 
      wh.location.toLowerCase().includes(search)
    );
  }

  onSearchChange(): void {
    this.newAssignment.warehouseId = null;
  }

  selectWarehouse(wh: any): void {
    this.newAssignment.warehouseId = wh.id;
    this.searchTerm = `${wh.name} (${wh.location})`;
    this.isDropdownOpen = false;
  }

  hideDropdown(): void {
    setTimeout(() => {
      this.isDropdownOpen = false;
    }, 200); 
  }


  loadHistory(): void {
    this.productService.getProductHistory(this.productId).subscribe({
      next: (data) => {
        this.history.set(data); 
      },
      error: (err) => console.error('Error loading history:', err)
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
    if (!this.isDirty || this.editedData.unitPrice <= 0) return;

    this.productService.updateProduct(this.productId, this.editedData).subscribe({
      next: () => {
        this.loadProduct(); 
      },
      error: (err) => console.error('Error saving changes:', err)
    });
  }

  isStockDirty(warehouseId: number, currentQuantity: any): boolean {
    const qty = Number(currentQuantity);
    if (currentQuantity === null || currentQuantity === undefined || qty < 0) {
      return false;
    }
    return this.originalStocks[warehouseId] !== qty;
  }

  updateStock(warehouseId: number, currentQuantity: any): void {
    const quantityAsNumber = Number(currentQuantity);
    if (quantityAsNumber < 0) return;

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
  
removeStock(warehouseId: number): void {

    this.productService.updateStockQuantity(this.productId, warehouseId, 0).subscribe({
      next: () => {
        this.loadProduct(); 
        this.loadHistory();
        this.cdr.detectChanges();
      },
      error: (err) => console.error('Error removing stock:', err)
    });
  }

assignToWarehouse(): void {
  
    if (!this.newAssignment.warehouseId || this.newAssignment.quantity <= 0) return;

    this.productService.updateStockQuantity(this.productId, this.newAssignment.warehouseId, this.newAssignment.quantity).subscribe({
      next: () => {
        this.newAssignment = { warehouseId: null, quantity: 0 };
        this.searchTerm = ''; 
        this.loadProduct(); 
        this.loadHistory();
        this.cdr.detectChanges();
      },
      error: (err) => console.error('Error assigning to warehouse:', err)
    });
  }

  goBack(): void {
    this.back.emit();
  }
}