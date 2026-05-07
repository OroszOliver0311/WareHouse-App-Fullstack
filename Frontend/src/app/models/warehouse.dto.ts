export interface WareHouseDto {
    id: number;
    name?: string | null;
    location: string;
}

export interface CreateWareHouseDto {
    name?: string | null;
    location: string;
}