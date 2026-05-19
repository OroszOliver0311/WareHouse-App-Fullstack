export interface InventoryItemDto {
    productId: number;
    wareHouseId: number;
    quantity: number;
}

export interface StockMovementDto {
    id: number;
    wareHouseLocation: string;
    isIncoming: boolean;
    quantity: number;
    date: string; 
}