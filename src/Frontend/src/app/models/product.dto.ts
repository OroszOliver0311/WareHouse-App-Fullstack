export interface ProductDashboardDto {
    id: number;
    name: string;
    sku: string;
    totalQuantity: number;
}

export interface CreateProductDto {
    name: string;
    sku: string;
    unitPrice: number;
}

export interface UpdateProductDto {
    name: string;
    sku: string;
    unitPrice: number;
}

export interface ProductDetailWareHouseDto {
    id: number;
    name?: string | null;
    location: string;
    quantity: number;
}

export interface ProductDetailDto {
    id: number;
    name: string;
    sku: string;
    unitPrice: number;
    stocks: ProductDetailWareHouseDto[];
}